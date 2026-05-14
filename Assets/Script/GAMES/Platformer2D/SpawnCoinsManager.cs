using UnityEngine;
using System.Collections.Generic;

public class SpawnCoinsManager : MonoBehaviour
{
	[SerializeField] private Transform[] pointCoins;
	[SerializeField] private GameObject coinPref;

	private readonly List<GameObject> coins = new List<GameObject>();
	
	private void Start()
	{
		Spawn();
	}

	private void Spawn()
	{
		for (int i = 0; i < pointCoins.Length; i++)
		{
			int coinShow = Random.Range(0, 2);

			if (coinShow > 0)
			{
				var coinCur = SpawnUtility.SpawnGameObject(coinPref, pointCoins[i].position, Quaternion.identity);
				
				coins.Add(coinCur);
			}
		}
	}

	public void Kill()
	{
		foreach (var item in coins)
		{
			Destroy(item);
		}
	}
}
