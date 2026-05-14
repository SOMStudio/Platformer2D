using UnityEngine;

[AddComponentMenu("SOMStudio/Platformer2D/Player Data Manager")]
public class BasePlayerManager : MonoBehaviour
{
	[SerializeField] protected bool didInit;

	[SerializeField] protected BaseUserManager dataManager;

	private void Awake()
	{
		Init();
	}

	protected virtual void Init()
	{
		if (!dataManager)
		{
			dataManager = gameObject.GetComponent<BaseUserManager>();

			if (!dataManager)
				dataManager = gameObject.AddComponent<BaseUserManager>();
		}

		dataManager.GetDefaultData();

		didInit = true;
	}

	public BaseUserManager GetDataManager()
	{
		return dataManager;
	}

	public virtual void GameFinished()
	{
		dataManager.SetIsFinished(true);
	}

	public virtual void GameStart()
	{
		dataManager.SetIsFinished(false);
	}
}
