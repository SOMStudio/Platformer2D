using SOMStudio.Platformer2D.Scripts.Base;
using UnityEngine;

namespace SOMStudio.Platformer2D.Scripts.Game
{
	public class PlatformFallManager : ExtendedCustomMonoBehaviour2D
	{
		[SerializeField] private float fallDelay = 1.0f;
		[SerializeField] private float killDelay = 4.0f;

		[SerializeField] private SpawnCoinsManager coinSpawner;

		private GameController gameController;

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				if (!gameController)
					Init();
			
				gameController.PlatformDrop(this.gameObject);
			
				Invoke(nameof(Fall), fallDelay);
				Invoke(nameof(Kill), killDelay);
			}
		}

		protected override void Init()
		{
			base.Init();

			if (!coinSpawner)
				coinSpawner = myGameObject.GetComponent<SpawnCoinsManager>();

			if (!gameController)
				gameController = GameController.Instance;
		}

		public void Fall()
		{
			myBody.isKinematic = false;
		}

		public void Kill()
		{
			coinSpawner.Kill();
		
			Destroy(gameObject);
		}

		public void FallWithDelay()
		{
			Invoke(nameof(Fall), fallDelay);
		}

		public void KillWithDelay()
		{
			Invoke(nameof(Kill), killDelay);
		}
	}
}
