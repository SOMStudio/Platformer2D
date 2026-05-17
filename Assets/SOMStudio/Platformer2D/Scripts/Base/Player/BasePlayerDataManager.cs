using UnityEngine;

namespace SOMStudio.Platformer2D.Scripts.Base.Player
{
	[AddComponentMenu("SOMStudio/Platformer2D/Player Data Manager")]
	public class BasePlayerDataManager : MonoBehaviour
	{
		[SerializeField] protected bool didInit;

		[SerializeField] protected BaseUserManager userManager;

		private void Awake()
		{
			Init();
		}

		protected virtual void Init()
		{
			if (!userManager)
			{
				userManager = gameObject.GetComponent<BaseUserManager>();

				if (!userManager)
					userManager = gameObject.AddComponent<BaseUserManager>();
			}

			userManager.GetDefaultData();

			didInit = true;
		}

		public BaseUserManager GetUserManager()
		{
			return userManager;
		}

		public virtual void GameFinished()
		{
			userManager.SetIsFinished(true);
		}

		public virtual void GameStart()
		{
			userManager.SetIsFinished(false);
		}
	}
}
