using UnityEngine;

public class AutomaticDestroyObject : MonoBehaviour
{
	[SerializeField] private float timeBeforeObjectDestroys;

	private void Start()
	{
		Invoke(nameof(DestroyGo), timeBeforeObjectDestroys);
	}

	private void DestroyGo()
	{
		Destroy(gameObject);
	}
}
