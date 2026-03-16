using UnityEngine;

public class AutomaticDestroyObject : MonoBehaviour
{
	[SerializeField] private float timeBeforeObjectDestroys;

	private void Start()
	{
		Invoke(nameof(destroyGO), timeBeforeObjectDestroys);
	}
	
	void destroyGO()
	{
		Destroy(gameObject);
	}
}
