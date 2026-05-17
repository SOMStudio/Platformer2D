using UnityEngine;
using UnityEngine.SceneManagement;

namespace SOMStudio.Platformer2D.Scripts.Base
{
	public class BaseGameController : MonoBehaviour
	{
		private bool paused;

		[SerializeField] protected GameObject explosionPrefab;
	
		public virtual void PlayerLostLife()
		{
		}

		public virtual void SpawnPlayer()
		{
		}

		public virtual void Respawn()
		{
		}

		public virtual void StartGame()
		{
		}

		public void Explode(Vector3 position)
		{
			Instantiate(explosionPrefab, position, Quaternion.identity);
		}

		public virtual void EnemyDestroyed(Vector3 position, int pointsValue, int hitByID)
		{
		}

		public virtual void BossDestroyed()
		{
		}

		public virtual void RestartGameButtonPressed()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public bool Paused
		{
			get => paused;
			set
			{
				paused = value;

				Time.timeScale = paused ? 0f : 1f;
			}
		}
	}
}