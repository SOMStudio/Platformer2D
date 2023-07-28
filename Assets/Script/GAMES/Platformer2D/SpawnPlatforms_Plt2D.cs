using UnityEngine;
using System.Collections.Generic;

public class SpawnPlatforms_Plt2D : MonoBehaviour
{
	[SerializeField] private int maxPlatforms = 5;
	[SerializeField] private GameObject platformPref;

	[SerializeField] private float horizontalMin = 9f;
	[SerializeField] private float horizontalMax = 17f;

	[SerializeField] private float verticalMin = -4f;
	[SerializeField] private float verticalMax = 4f;

	[SerializeField] private List<GameObject> listPlatforms = new List<GameObject>();
	private Vector3 lastPosition;

	[System.NonSerialized] public static SpawnPlatforms_Plt2D Instance;

	private GameController_Plt2D gameController;
	
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		if (!gameController)
			Init();

		if (gameController.IsGameStart)
		{
			if (listPlatforms.Count < maxPlatforms)
			{
				SpawnNext();
			}
		}
	}
	
	private void Init()
	{
		if (!gameController)
			gameController = GameController_Plt2D.Instance;
	}

	public float HorizontalMax
	{
		get { return horizontalMax; }
	}

	private void AddPlatform(GameObject val)
	{
		listPlatforms.Add(val);
	}

	public void RemovePlatform(GameObject val)
	{
		int indexRemove = listPlatforms.IndexOf(val);

		if (indexRemove > 0)
		{
			// we jump over some platform
			for (int i = 0; i < indexRemove; i++)
			{
				// drop platform
				var pltFallGO = listPlatforms[i];
				var pltFallManager = pltFallGO.GetComponent<PlatformFall_Plt2D>();
				pltFallManager.Fall();
				pltFallManager.KillWithDelay();

				// remove from list
				listPlatforms.RemoveAt(i);
			}
		}

		// drop touched platform
		listPlatforms.Remove(val);
	}

	public void RemovePlatAll()
	{
		// destroy platforms
		foreach (GameObject item in listPlatforms)
		{
			Destroy(item);
		}

		// destroy coins
		GameObject[] coinList = GameObject.FindGameObjectsWithTag("Coin");
		foreach (GameObject item in coinList)
		{
			Destroy(item);
		}

		listPlatforms.Clear();
	}

	public Vector3 GetFirstPlatformPosition()
	{
		return listPlatforms[0].transform.position;
	}

	public Vector3 GetLastPlatformPosition()
	{
		return lastPosition;
	}

	public void SpawnFirst(Vector3 startPos)
	{
		//spawn
		Vector3 pos = new Vector3(startPos.x, startPos.y - verticalMax, 0);
		GameObject go = SpawnController.Instance.SpawnGO(platformPref, pos, Quaternion.identity);
		go.transform.parent = this.transform;

		// set position
		lastPosition = pos;

		// set for control
		listPlatforms.Add(go);

		SpawnGrope();
	}

	private void SpawnGrope()
	{
		for (int i = 1; i < maxPlatforms; i++)
		{
			SpawnNext();
		}
	}

	private void SpawnNext()
	{
		//spawn
		Vector3 pos = lastPosition + new Vector3(Random.Range(horizontalMin, horizontalMax),
			Random.Range(verticalMin, verticalMax), 0);
		GameObject go = SpawnController.Instance.SpawnGO(platformPref, pos, Quaternion.identity);
		go.transform.parent = this.transform;

		// set position
		lastPosition = pos;

		// set for control
		listPlatforms.Add(go);
	}
}
