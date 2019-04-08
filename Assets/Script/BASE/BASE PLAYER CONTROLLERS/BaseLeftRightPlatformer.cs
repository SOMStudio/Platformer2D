using UnityEngine;
using System.Collections;

[AddComponentMenu("Base/Character/Left Right Platformer")]

public class BaseLeftRightPlatformer : ExtendedCustomMonoBehaviour2D
{
	private float moveXAmount;
	public float moveXSpeed = 20f;
	public float jumpForce = 300f;
	
	[System.NonSerialized]
	public Keyboard_Input default_input;

	public float horizontal_input = 0f;
	public float vertical_input = 0f;

	public bool facingRight = true;
	public bool clickJump = false;
	public bool grounded = false;

	public Transform groundPoint;

	private Animator anim;
	
	public void Start()
	{
		// we are overriding Start() so as not to call Init, as we want the game controller to do this in this game.
		didInit=false;
		
		Init();
	}
	
	public virtual void Init ()
	{	
		// cache refs to our transform and gameObject
		myTransform= transform;
		myGO= gameObject;
		myBody= GetComponent<Rigidbody2D>();
		anim= GetComponent<Animator>();

		// add default keyboard input
		default_input= myGO.AddComponent<Keyboard_Input>();
		
		// set a flag so that our Update function knows when we are OK to use
		didInit=true;
	}
	
	public void GameStart ()
	{
		// we are good to go, so let's get moving!
		canControl=true;
	}

	public void Update ()
	{
		UpdateCharacter ();
	}
	
	public virtual void UpdateCharacter ()
	{
		// don't do anything until Init() has been run
		if(!didInit)
			return;

		// check to see if we're supposed to be controlling the player before moving it
		if(!canControl)
			return;
		
		GetInput();
	}

	public virtual void GetInput ()
	{
		// this is just a 'default' function that (if needs be) should be overridden in the glue code
		horizontal_input= default_input.GetHorizontal();
		vertical_input= default_input.GetVertical();

		if (Input.GetButtonDown ("Jump") && grounded && !clickJump)
		{
			clickJump = true;
		}
	}

	public void FixedUpdate()
	{
		FixedUpdateCharacter ();
	}

	public virtual void FixedUpdateCharacter ()
	{
		// don't do anything until Init() has been run
		if(!didInit)
			return;

		// check to see if we're supposed to be controlling the player before moving it
		if(!canControl)
			return;

		grounded = Physics2D.Linecast (transform.position, groundPoint.position, 1 << LayerMask.NameToLayer ("Ground"));

		anim.SetBool ("grounded", grounded);
		anim.SetFloat("runSpeed", Mathf.Abs(horizontal_input));

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

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}




