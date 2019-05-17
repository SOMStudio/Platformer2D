using UnityEngine;

public class Player_Plt2D : BaseLeftRightPlatformer
{
	[SerializeField]
	private Transform cameraOffsetPoint;
	[SerializeField]
	private Vector2 cameraOffsetValue;
	private float myVelocityYNorm = 0f;

	[SerializeField]
	private BasePlayerManager myPlayerManager;
	[SerializeField]
	private BaseUserManager myDataManager;

	private bool isInvulnerable;
	private bool isRespawning;

	[SerializeField]
	private bool godMode =false;
	
	[SerializeField]
	private bool isFinished;

	// main logic
	public override void Init ()
	{
		base.Init ();
		
		// do god mode, if needed)
		if(!godMode)
		{
			MakeVulnerable();
		} else {
			MakeInvulnerable();
		}
		
		// start out with no control from the player
		canControl=false;

		// if a player manager is not set in the editor, let's try to find one
		if(myPlayerManager==null)
			myPlayerManager= myGO.GetComponent<BasePlayerManager>();
		
		// set up the data for our player
		myDataManager= myPlayerManager.GetDataManager();
		myDataManager.SetName("Player1");
		myDataManager.SetHealth(3);
		
		isFinished= false;
		
		// get a ref to the player manager
		GameController_Plt2D.Instance.UpdateLivesP1(myDataManager.GetHealth());
	}

	protected override void UpdateCharacter ()
	{
		// don't do anything until Init() has been run
		if(!didInit)
			return;

		// check to see if we're supposed to be controlling the player before moving it
		if(!canControl)
			return;
		
		base.UpdateCharacter ();

		//camera offset point
		if (cameraOffsetPoint) {
			myVelocityYNorm = Mathf.Lerp (myVelocityYNorm, Mathf.Clamp(myBody.velocity.y, -1.0f, 1.0f), Time.deltaTime);
			cameraOffsetPoint.localPosition = new Vector3 (cameraOffsetValue.x * Mathf.Abs(horizontal_input), cameraOffsetValue.y * myVelocityYNorm, 0);
		}
	}

	protected override void GetInput()
	{
		if (isFinished || isRespawning)
		{
			horizontal_input=0;
			vertical_input=0;
			return;
		}

		// get main controll (hor, ver move and jump)
		base.GetInput ();
	}

	public void GameEnd()
	{
		// this function is called by the game controller to tell us when we can start moving
		canControl=false;
	}
	
	void LostLife()
	{
		isRespawning=true;
				
		// blow us up!
		GameController_Plt2D.Instance.PlayerHit( myTransform );
			
		// reduce lives by one
		myDataManager.ReduceHealth(1);
		
		// as our ID is 1, we must be player 1
		GameController_Plt2D.Instance.UpdateLivesP1( myDataManager.GetHealth() );
		
		if(myDataManager.GetHealth()<1) // <- game over
		{
			// stop movement, as long as rigidbody is not kinematic (otherwise it will have no velocity and we
			// will generate an error message trying to set it)
			Rigidbody rb = GetComponent<Rigidbody>();
			if (!rb.isKinematic)
				rb.velocity=Vector3.zero;
			
			// hide body
			myGO.SetActive(false);
			
			// do anything we need to do at game finished
			PlayerFinished();
		} else {
			// hide body
			myGO.SetActive(false);
			
			// respawn 
			Invoke("Respawn",2f);
		}
	}
	
	void Respawn()
	{
		// reset the 'we are respawning' variable
		isRespawning= false;
		
		// we need to be invulnerable for a little while
		MakeInvulnerable();
		
		Invoke ("MakeVulnerable",3);

		// show body again
		myGO.SetActive(true);
	}
	
	void OnCollisionEnter(Collision collider)
	{
		// MAKE SURE that weapons don't have colliders
		// if you are using primitives, only use a single collider on the same gameobject which has this script on
		
		// when something collides with our ship, we check its layer to see if it is on 11 which is our projectiles
		// (Note: remember when you add projectiles to set the layer correctly!)
		if(collider.gameObject.layer==11 && !isRespawning && !isInvulnerable)
		{
			LostLife();
		}
	}

	public void SetUserInput( bool setInput )
	{
		canControl= setInput;
	}

	void MakeInvulnerable()
	{
		isInvulnerable=true;
	}
	
	void MakeVulnerable()
	{
		isInvulnerable=false;
	}
	
	public void PlayerFinished()
	{
		// tell the player controller that we have finished
		GameController_Plt2D.Instance.PlayerDied( id );
		
		isFinished=true;
	}
	
}
