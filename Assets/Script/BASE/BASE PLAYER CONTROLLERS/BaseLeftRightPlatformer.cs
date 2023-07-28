using UnityEngine;

[AddComponentMenu("Base/Character/Left Right Platform")]
public class BaseLeftRightPlatformer : ExtendedCustomMonoBehaviour2D
{
	[Header("Move settings")]
	[SerializeField] protected float moveXSpeed = 20f;

	[SerializeField] protected float jumpForce = 300f;

	[Header("Technic value")]
	[SerializeField] protected float horizontal_input;

	[SerializeField] protected bool facingRight = true;
	[SerializeField] protected bool clickJump;
	[SerializeField] protected bool grounded;

	[Header("Technic references")] [SerializeField]
	protected Transform groundPoint;

	protected Keyboard_Input defaultInput;
	protected Animator myAnimator;
	
	private static readonly int Grounded = Animator.StringToHash("grounded");
	private static readonly int RunSpeed = Animator.StringToHash("runSpeed");

	private void Update()
	{
		UpdateCharacter();
	}

	private void FixedUpdate()
	{
		FixedUpdateCharacter();
	}
	
	public override void Init()
	{
		// base init
		base.Init();

		didInit = false;

		// cache refs to our transform and gameObject
		if (!myAnimator)
		{
			myAnimator = GetComponent<Animator>();
		}

		// add default keyboard input
		if (!defaultInput)
		{
			defaultInput = myGO.AddComponent<Keyboard_Input>();
		}

		// set a flag so that our Update function knows when we are OK to use
		didInit = true;
	}
	
	public virtual void GameStart()
	{
		// we are good to go, so let's get moving!
		canControl = true;
	}
	
	protected virtual void UpdateCharacter()
	{
		// don't do anything until Init() has been run
		if (!didInit)
			return;

		// check to see if we're supposed to be controlling the player before moving it
		if (!canControl)
			return;

		GetInput();
	}
	
	protected virtual void GetInput()
	{
		// this is just a 'default' function that (if needs be) should be overridden in the glue code
		horizontal_input = defaultInput.GetHorizontal();
		defaultInput.GetVertical();

		if (Input.GetButtonDown("Jump") && grounded && !clickJump)
		{
			clickJump = true;
		}
	}
	
	protected virtual void FixedUpdateCharacter()
	{
		// don't do anything until Init() has been run
		if (!didInit)
			return;

		// check to see if we're supposed to be controlling the player before moving it
		if (!canControl)
			return;

		grounded = Physics2D.Linecast(transform.position, groundPoint.position, 1 << LayerMask.NameToLayer("Ground"));

		// animation
		myAnimator.SetBool(Grounded, grounded);
		myAnimator.SetFloat(RunSpeed, Mathf.Abs(horizontal_input));

		if (defaultInput.Right && !facingRight)
			Flip();
		else if (defaultInput.Left && facingRight)
			Flip();

		//in rigidbody must set interpolate for line velocity
		myBody.velocity = new Vector2(horizontal_input * moveXSpeed, myBody.velocity.y);

		// jump force
		if (clickJump && grounded)
		{
			myBody.AddForce(new Vector2(0f, jumpForce));
			clickJump = false;
		}
	}
	
	protected virtual void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = myTransform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}