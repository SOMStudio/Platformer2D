using UnityEngine;

[AddComponentMenu("Base/Character/Left Right Platformer")]

public class BaseLeftRightPlatformer : ExtendedCustomMonoBehaviour2D
{
	[Header("Move settings")]
	[SerializeField]
	protected float moveXSpeed = 20f;
	[SerializeField]
	protected float jumpForce = 300f;

	[Header("Technic value")]
	[SerializeField]
	protected float horizontal_input = 0f;
	[SerializeField]
	protected float vertical_input = 0f;
	[SerializeField]
	protected bool facingRight = true;
	[SerializeField]
	protected bool clickJump = false;
	[SerializeField]
	protected bool grounded = false;

	[Header("Technic referance")]
	[SerializeField]
	protected Transform groundPoint;
	protected Keyboard_Input default_input;
	protected Animator myAnimator;

	// main event
	void Update ()
	{
		UpdateCharacter ();
	}

	void FixedUpdate()
	{
		FixedUpdateCharacter ();
	}

	// main logiv

	/// <summary>
	/// Init main instance (myTransform, myGO, myBody, myAnimator, KeybordInput), def. in Start.
	/// </summary>
	public override void Init ()
	{	
		// base init
		base.Init ();

		didInit=false;

		// cache refs to our transform and gameObject
		if (!myAnimator) {
			myAnimator = GetComponent<Animator> ();
		}

		// add default keyboard input
		if (!default_input) {
			default_input = myGO.AddComponent<Keyboard_Input> ();
		}
		
		// set a flag so that our Update function knows when we are OK to use
		didInit=true;
	}

	/// <summary>
	/// Games the start (canControl = true).
	/// </summary>
	public virtual void GameStart ()
	{
		// we are good to go, so let's get moving!
		canControl=true;
	}

	/// <summary>
	/// Updates the character input Control, def. invoke in Update.
	/// </summary>
	protected virtual void UpdateCharacter ()
	{
		// don't do anything until Init() has been run
		if(!didInit)
			return;

		// check to see if we're supposed to be controlling the player before moving it
		if(!canControl)
			return;
		
		GetInput();
	}

	/// <summary>
	/// Gets the input (left, right, jump), def. invoke in UpdateCharacter.
	/// </summary>
	protected virtual void GetInput ()
	{
		// this is just a 'default' function that (if needs be) should be overridden in the glue code
		horizontal_input= default_input.GetHorizontal();
		vertical_input= default_input.GetVertical();

		if (Input.GetButtonDown ("Jump") && grounded && !clickJump)
		{
			clickJump = true;
		}
	}

	/// <summary>
	/// Fixeds the update character phisic Control (velocity, AddForce).
	/// </summary>
	protected virtual void FixedUpdateCharacter ()
	{
		// don't do anything until Init() has been run
		if(!didInit)
			return;

		// check to see if we're supposed to be controlling the player before moving it
		if(!canControl)
			return;

		grounded = Physics2D.Linecast (transform.position, groundPoint.position, 1 << LayerMask.NameToLayer ("Ground"));

		// animation
		myAnimator.SetBool ("grounded", grounded);
		myAnimator.SetFloat ("runSpeed", Mathf.Abs(horizontal_input));

		if (default_input.Right && !facingRight)
			Flip ();
		else if (default_input.Left && facingRight)
			Flip ();

		//in rigidbody must set interpolate for line velocity
		myBody.velocity = new Vector2 (horizontal_input * moveXSpeed, myBody.velocity.y);

		// jump force
		if (clickJump && grounded)
		{
			myBody.AddForce(new Vector2(0f, jumpForce));
			clickJump = false;
		}
	}

	/// <summary>
	/// Flip this instance.
	/// </summary>
	protected virtual void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = myTransform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}




