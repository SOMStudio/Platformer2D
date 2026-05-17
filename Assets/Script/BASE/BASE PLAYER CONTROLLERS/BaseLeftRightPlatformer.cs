using UnityEngine;

public class BaseLeftRightPlatformer : ExtendedCustomMonoBehaviour2D
{
	[Header("Move settings")]
	[SerializeField] protected float moveXSpeed = 20f;

	[SerializeField] protected float jumpForce = 300f;
	
	[Header("Technic value")]
	[SerializeField] protected float horizontalInput;

	[SerializeField] protected bool facingRight = true;
	[SerializeField] protected bool clickJump;
	[SerializeField] protected bool grounded;

	[Header("Technic references")]
	[SerializeField] protected Transform groundPoint;

	protected KeyboardInput input;
	protected Animator animator;

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

	protected override void Init()
	{
		base.Init();

		didInit = false;

		if (!animator)
		{
			animator = GetComponent<Animator>();
		}

		if (!input)
		{
			input = myGameObject.AddComponent<KeyboardInput>();
		}

		didInit = true;
	}

	public virtual void GameStart()
	{
		canControl = true;
	}

	protected virtual void UpdateCharacter()
	{
		if (!didInit)
			return;

		if (!canControl)
			return;

		GetInput();
	}

	protected virtual void GetInput()
	{
		horizontalInput = input.GetHorizontal();
		input.GetVertical();

		if (Input.GetButtonDown("Jump") && grounded && !clickJump)
		{
			clickJump = true;
		}
	}

	protected virtual void FixedUpdateCharacter()
	{
		if (!didInit)
			return;

		if (!canControl)
			return;

		grounded = Physics2D.Linecast(transform.position, groundPoint.position, 1 << LayerMask.NameToLayer("Ground"));

		animator.SetBool(Grounded, grounded);
		animator.SetFloat(RunSpeed, Mathf.Abs(horizontalInput));

		if (input.Right && !facingRight)
			Flip();
		else if (input.Left && facingRight)
			Flip();

		myBody.velocity = new Vector2(horizontalInput * moveXSpeed, myBody.velocity.y);

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