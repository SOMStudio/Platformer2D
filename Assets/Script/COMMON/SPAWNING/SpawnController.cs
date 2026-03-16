using UnityEngine;
using System.Collections;

[AddComponentMenu("Utility/Spawn Controller")]
public class SpawnController : ScriptableObject
{
	private ArrayList playerTransforms;
	private ArrayList playerGameObjects;

	private Transform tempTrans;
	private GameObject tempGO;

	private GameObject[] playerPrefabList;
	private Vector3[] startPositions;
	private Quaternion[] startRotations;

	private static SpawnController instance;

	public SpawnController()
	{
		if (instance != null)
		{
			Debug.LogWarning("Tried to generate more than one instance of singleton SpawnController.");
			return;
		}
		
		instance = this;
	}

	public static SpawnController Instance
	{
		get
		{
			if (instance == null)
			{
				CreateInstance<SpawnController>();
			}
			
			return instance;
		}
	}

	public void Restart()
	{
		playerTransforms = new ArrayList();
		playerGameObjects = new ArrayList();
	}

	public void SetUpPlayers(GameObject[] playerPrefabs, Vector3[] playerStartPositions,
		Quaternion[] playerStartRotations, Transform theParentObj, int totalPlayers)
	{
		playerPrefabList = playerPrefabs;
		startPositions = playerStartPositions;
		startRotations = playerStartRotations;
		
		CreatePlayers(theParentObj, totalPlayers);
	}

	public void CreatePlayers(Transform theParent, int totalPlayers)
	{
		playerTransforms = new ArrayList();
		playerGameObjects = new ArrayList();

		for (int i = 0; i < totalPlayers; i++)
		{
			tempTrans = Spawn(playerPrefabList[i], startPositions[i], startRotations[i]);
			
			if (theParent != null)
			{
				tempTrans.parent = theParent;
				tempTrans.localPosition = startPositions[i];
			}
			
			playerTransforms.Add(tempTrans);
			
			playerGameObjects.Add(tempTrans.gameObject);
		}
	}

	public GameObject GetPlayerGO(int indexNum)
	{
		return (GameObject)playerGameObjects[indexNum];
	}

	public Transform GetPlayerTransform(int indexNum)
	{
		return (Transform)playerTransforms[indexNum];
	}

	public Transform Spawn(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		tempGO = Instantiate(anObject, aPosition, aRotation);
		tempTrans = tempGO.transform;
		
		return tempTrans;
	}
	
	public GameObject SpawnGO(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		tempGO = (GameObject)Instantiate(anObject, aPosition, aRotation);
		tempTrans = tempGO.transform;
		
		return tempGO;
	}

	public ArrayList GetAllSpawnedPlayers()
	{
		return playerTransforms;
	}
}
