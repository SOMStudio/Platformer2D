using UnityEngine;
using System.Collections;

public class AutomaticDestroyObject : MonoBehaviour 
{
	[SerializeField]
	private float timeBeforeObjectDestroys;

	// main event
	void Start () {
		// the function destroyGO() will be called in timeBeforeObjectDestroys seconds
		Invoke("destroyGO", timeBeforeObjectDestroys);
	}

	// main logic
	void destroyGO () {
		// destroy this gameObject
		Destroy(gameObject);
	}
}
