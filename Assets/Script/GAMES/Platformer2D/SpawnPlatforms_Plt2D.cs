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
		if (!gameController) gameController = GameController_Plt2D.Instance;
	}

	public float HorizontalMax => horizontalMax;

	private void AddPlatform(GameObject val)
	{
		listPlatforms.Add(val);
	}

	public void RemovePlatform(GameObject val)
	{
		int indexRemove = listPlatforms.IndexOf(val);

		if (indexRemove > 0)
		{
			for (int i = 0; i < indexRemove; i++)
			{
				var platformFallGameObject = listPlatforms[i];
				var platformFallManager = platformFallGameObject.GetComponent<PlatformFall_Plt2D>();
				platformFallManager.Fall();
				platformFallManager.KillWithDelay();

				listPlatforms.RemoveAt(i);
			}
		}

		listPlatforms.Remove(val);
	}

	public void RemovePlatAll()
	{
		foreach (GameObject item in listPlatforms)
		{
			Destroy(item);
		}

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
		Vector3 pos = new Vector3(startPos.x, startPos.y - verticalMax, 0);
		GameObject go = SpawnController.Instance.SpawnGO(platformPref, pos, Quaternion.identity);
		go.transform.parent = transform;

		lastPosition = pos;

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
		Vector3 pos = lastPosition + new Vector3(Random.Range(horizontalMin, horizontalMax),
			Random.Range(verticalMin, verticalMax), 0);
		GameObject go = SpawnController.Instance.SpawnGO(platformPref, pos, Quaternion.identity);
		go.transform.parent = transform;

		lastPosition = pos;

		listPlatforms.Add(go);
	}
}
