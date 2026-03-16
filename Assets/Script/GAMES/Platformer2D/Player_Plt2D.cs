using UnityEngine;

public class Player_Plt2D : BaseLeftRightPlatformer
{
	[SerializeField] private Transform cameraOffsetPoint;
	[SerializeField] private Vector2 cameraOffsetValue;
	private float myVelocityYNorm;

	[SerializeField] private BasePlayerManager myPlayerManager;
	[SerializeField] private BaseUserManager myDataManager;

	private bool isInvulnerable;
	private bool isRespawning;

	[SerializeField] private bool godMode;

	[SerializeField] private bool isFinished;

	public override void Init()
	{
		base.Init();

		if (!godMode)
		{
			MakeVulnerable();
		}
		else
		{
			MakeInvulnerable();
		}

		canControl = false;

		if (myPlayerManager == null)
			myPlayerManager = myGO.GetComponent<BasePlayerManager>();

		myDataManager = myPlayerManager.GetDataManager();
		myDataManager.SetName("Player1");
		myDataManager.SetHealth(3);

		isFinished = false;

		GameController_Plt2D.Instance.UpdateLivesP1(myDataManager.GetHealth());
	}

	protected override void UpdateCharacter()
	{
		if (!didInit)
			return;

		if (!canControl)
			return;

		base.UpdateCharacter();

		if (cameraOffsetPoint)
		{
			myVelocityYNorm = Mathf.Lerp(myVelocityYNorm, Mathf.Clamp(myBody.velocity.y, -1.0f, 1.0f), Time.deltaTime);
			cameraOffsetPoint.localPosition = new Vector3(cameraOffsetValue.x * Mathf.Abs(horizontal_input),
				cameraOffsetValue.y * myVelocityYNorm, 0);
		}
	}

	protected override void GetInput()
	{
		if (isFinished || isRespawning)
		{
			horizontal_input = 0;
			return;
		}

		base.GetInput();
	}

	public void GameEnd()
	{
		canControl = false;
	}

	void LostLife()
	{
		isRespawning = true;

		GameController_Plt2D.Instance.PlayerHit(myTransform);

		myDataManager.ReduceHealth(1);

		GameController_Plt2D.Instance.UpdateLivesP1(myDataManager.GetHealth());

		if (myDataManager.GetHealth() < 1)
		{
			Rigidbody rb = GetComponent<Rigidbody>();
			if (!rb.isKinematic)
				rb.velocity = Vector3.zero;

			myGO.SetActive(false);

			PlayerFinished();
		}
		else
		{
			myGO.SetActive(false);

			Invoke(nameof(Respawn), 2f);
		}
	}

	private void Respawn()
	{
		isRespawning = false;

		MakeInvulnerable();

		Invoke(nameof(MakeVulnerable), 3);

		myGO.SetActive(true);
	}

	private void OnCollisionEnter(Collision otherCollider)
	{
		if (otherCollider.gameObject.layer == 11 && !isRespawning && !isInvulnerable)
		{
			LostLife();
		}
	}

	public void SetUserInput(bool setInput)
	{
		canControl = setInput;
	}

	void MakeInvulnerable()
	{
		isInvulnerable = true;
	}

	void MakeVulnerable()
	{
		isInvulnerable = false;
	}

	public void PlayerFinished()
	{
		GameController_Plt2D.Instance.PlayerDied(id);

		isFinished = true;
	}
}
