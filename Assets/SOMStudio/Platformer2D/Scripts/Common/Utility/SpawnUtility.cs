using UnityEngine;
using System.Collections;

public static class SpawnUtility
{
	private static ArrayList PlayerTransforms;
	private static ArrayList PlayerGameObjects;

	private static GameObject[] PlayerPrefabList;
	private static Vector3[] StartPositions;
	private static Quaternion[] StartRotations;

	public static void Restart()
	{
		PlayerTransforms = new ArrayList();
		PlayerGameObjects = new ArrayList();
	}

	public static void SetUpPlayers(GameObject[] playerPrefabs, Vector3[] playerStartPositions,
		Quaternion[] playerStartRotations, Transform theParentObj, int totalPlayers)
	{
		PlayerPrefabList = playerPrefabs;
		StartPositions = playerStartPositions;
		StartRotations = playerStartRotations;
		
		CreatePlayers(theParentObj, totalPlayers);
	}

	public static void CreatePlayers(Transform theParent, int totalPlayers)
	{
		PlayerTransforms = new ArrayList();
		PlayerGameObjects = new ArrayList();

		for (int i = 0; i < totalPlayers; i++)
		{
			Transform tempTransform = Spawn(PlayerPrefabList[i], StartPositions[i], StartRotations[i]);
			
			if (theParent != null)
			{
				tempTransform.parent = theParent;
				tempTransform.localPosition = StartPositions[i];
			}
			
			PlayerTransforms.Add(tempTransform);
			
			PlayerGameObjects.Add(tempTransform.gameObject);
		}
	}

	public static GameObject GetPlayerGameObject(int indexNum)
	{
		return (GameObject)PlayerGameObjects[indexNum];
	}

	public static Transform GetPlayerTransform(int indexNum)
	{
		return (Transform)PlayerTransforms[indexNum];
	}

	public static Transform Spawn(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		GameObject tempGameObject = SpawnGameObject(anObject, aPosition, aRotation);
		Transform tempTrans = tempGameObject.transform;
		
		return tempTrans;
	}
	
	public static GameObject SpawnGameObject(GameObject anObject, Vector3 aPosition, Quaternion aRotation)
	{
		GameObject tempGameObject = Object.Instantiate(anObject, aPosition, aRotation);
		
		return tempGameObject;
	}

	public static ArrayList GetAllSpawnedPlayers()
	{
		return PlayerTransforms;
	}
}
