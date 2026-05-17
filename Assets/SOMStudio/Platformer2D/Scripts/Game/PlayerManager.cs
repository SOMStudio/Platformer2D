using SOMStudio.Platformer2D.Scripts.Base;
using SOMStudio.Platformer2D.Scripts.Base.Player;
using UnityEngine;

namespace SOMStudio.Platformer2D.Scripts.Game
{
	[AddComponentMenu("SomStudio/Platformer2D/Player Manager")]
	public class PlayerManager : BaseLeftRightPlatformer
	{
		[SerializeField] private Transform cameraOffsetPoint;
		[SerializeField] private Vector2 cameraOffsetValue;
		private float velocityNormalized;

		[SerializeField] private BasePlayerDataManager playerDataManager;
		[SerializeField] private BaseUserManager userManager;

		private bool isInvulnerable;
		private bool isRespawning;

		[SerializeField] private bool godMode;

		[SerializeField] private bool isFinished;

		protected override void Init()
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

			if (playerDataManager == null)
				playerDataManager = myGameObject.GetComponent<BasePlayerDataManager>();

			userManager = playerDataManager.GetUserManager();
			userManager.SetName("Player1");
			userManager.SetHealth(3);

			isFinished = false;

			GameController.Instance.UpdateLivesP1(userManager.GetHealth());
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
				velocityNormalized = Mathf.Lerp(velocityNormalized, Mathf.Clamp(myBody.velocity.y, -1.0f, 1.0f), Time.deltaTime);
				cameraOffsetPoint.localPosition = new Vector3(cameraOffsetValue.x * Mathf.Abs(horizontalInput),
					cameraOffsetValue.y * velocityNormalized, 0);
			}
		}

		protected override void GetInput()
		{
			if (isFinished || isRespawning)
			{
				horizontalInput = 0;
				return;
			}

			base.GetInput();
		}

		public void GameEnd()
		{
			canControl = false;
		}

		private void LostLife()
		{
			isRespawning = true;

			GameController.Instance.PlayerHit(myTransform);

			userManager.ReduceHealth(1);

			GameController.Instance.UpdateLivesP1(userManager.GetHealth());

			if (userManager.GetHealth() < 1)
			{
				Rigidbody rb = GetComponent<Rigidbody>();
				if (!rb.isKinematic)
					rb.velocity = Vector3.zero;

				myGameObject.SetActive(false);

				PlayerFinished();
			}
			else
			{
				myGameObject.SetActive(false);

				Invoke(nameof(Respawn), 2f);
			}
		}

		private void Respawn()
		{
			isRespawning = false;

			MakeInvulnerable();

			Invoke(nameof(MakeVulnerable), 3);

			myGameObject.SetActive(true);
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

		private void MakeInvulnerable()
		{
			isInvulnerable = true;
		}

		private void MakeVulnerable()
		{
			isInvulnerable = false;
		}

		public void PlayerFinished()
		{
			GameController.Instance.PlayerDied(id);

			isFinished = true;
		}
	}
}
