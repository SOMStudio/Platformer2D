using UnityEngine;
using System.Collections.Generic;

public class SpawnPlatforms_Plt2D : MonoBehaviour {

	public int maxPlatforms = 5;
	public GameObject platformPref;

	public float horizontalMin = 9f;
	public float horizontalMax = 17f;

	public float verticalMin = -4f;
	public float verticalMax = 4f;

	public List<GameObject> listPlatforms = new List<GameObject>();
	private Vector3 lastPosition;

	[System.NonSerialized]
	public static SpawnPlatforms_Plt2D Instance;

	private GameController_Plt2D gameController;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (this);
	}

	void Start() {
		Init ();
	}

	void Update () {
		if (!gameController)
			Init ();
			
		if (gameController.startGame) {
			if (listPlatforms.Count < maxPlatforms) {
				SpawnNext ();
			}
		}
	}

	// init
	private void Init() {
		if (!gameController)
			gameController = GameController_Plt2D.Instance;
	}

	// list Platform
	public void AddPlatform(GameObject val) {
		listPlatforms.Add (val);
	}

	public void RemovePlatform(GameObject val) {
		listPlatforms.Remove (val);
	}

	public void RemovePlatAll() {
		// destroy platforms
		foreach (GameObject item in listPlatforms) {
			Destroy (item);
		}

		// destroy coins
		GameObject[] coinList = GameObject.FindGameObjectsWithTag ("Coin");
		foreach (GameObject item in coinList) {
			Destroy (item);
		}

		listPlatforms.Clear ();
	}

	public Vector3 GetFirstPlatformPossition() {
		return listPlatforms[0].transform.position;
	}

	public Vector3 GetLastPlatformPossition() {
		return lastPosition;
	}

	// spawn controll
	public void SpawnFirst(Vector3 startPos)
	{
		//spawn
		Vector3 pos = new Vector3(startPos.x, startPos.y - verticalMax, 0);
		GameObject go = SpawnController.Instance.SpawnGO (platformPref, pos, Quaternion.identity);
		go.transform.parent = this.transform;

		// set possition
		lastPosition = pos;

		// set for controll
		listPlatforms.Add (go);

		SpawnGrope ();
	}

	void SpawnGrope()
	{
		for (int i = 1; i < maxPlatforms; i++)
		{
			SpawnNext ();
		}
	}

	void SpawnNext()
	{
		//spawn
		Vector3 pos = lastPosition + new Vector3(Random.Range(horizontalMin, horizontalMax), Random.Range(verticalMin, verticalMax), 0);
		GameObject go = SpawnController.Instance.SpawnGO (platformPref, pos, Quaternion.identity);
		go.transform.parent = this.transform;

		// set possition
		lastPosition = pos;

		// set for controll
		listPlatforms.Add (go);
	}
}
