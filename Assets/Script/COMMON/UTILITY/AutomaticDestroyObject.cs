using UnityEngine;

public class AutomaticDestroyObject : MonoBehaviour
{
	[SerializeField] private float timeBeforeObjectDestroys;

	private void Start()
	{
		// the function destroyGO() will be called in timeBeforeObjectDestroys seconds
		Invoke(nameof(destroyGO), timeBeforeObjectDestroys);
	}
	
	void destroyGO()
	{
		// destroy this gameObject
		Destroy(gameObject);
	}
}
