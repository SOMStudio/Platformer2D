using UnityEngine;
using System.Collections;

public class SpawnCoins_Plt2D : MonoBehaviour {

	public Transform[] pointCoins;
	public GameObject coinPref;

	private GameController_Plt2D gameController;

	// Use this for initialization
	void Start () {
		Init ();

		Spawn ();
	}

	// init
	private void Init() {
		if (!gameController)
			gameController = GameController_Plt2D.Instance;
	}

	void Spawn()
	{
		for (int i = 0; i < pointCoins.Length; i++)
		{
			int coinShow = Random.Range(0, 2);

			if (coinShow > 0) {
				GameObject go = SpawnController.Instance.SpawnGO (coinPref, pointCoins [i].position, Quaternion.identity);
			}
		}
	}
}
