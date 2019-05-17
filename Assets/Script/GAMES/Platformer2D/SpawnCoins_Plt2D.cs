using UnityEngine;
using System.Collections.Generic;

public class SpawnCoins_Plt2D : MonoBehaviour {

	[SerializeField]
	private Transform[] pointCoins;
	[SerializeField]
	private GameObject coinPref;

	private List<GameObject> listCoins = new List<GameObject>();

	// main event
	void Start () {
		Spawn ();
	}

	// main logic
	void Spawn()
	{
		for (int i = 0; i < pointCoins.Length; i++)
		{
			int coinShow = Random.Range(0, 2);

			if (coinShow > 0) {
				var coinCur = SpawnController.Instance.SpawnGO (coinPref, pointCoins [i].position, Quaternion.identity);

				// add in list
				listCoins.Add(coinCur);
			}
		}
	}

	public void Kill() {
		foreach (var item in listCoins) {
			Destroy (item);
		}
	}
}
