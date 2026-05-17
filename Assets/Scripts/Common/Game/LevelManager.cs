using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private string[] levelNames;
	[SerializeField] private int gameLevelNumber;

	[System.NonSerialized] public static LevelManager Instance;
	
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
	
	public string[] LevelNames
	{
		get => levelNames;
		set => levelNames = value;
	}

	public void LoadLevel(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void ResetGame()
	{
		gameLevelNumber = 0;
	}

	public void GoNextLevel()
	{
		if (gameLevelNumber >= levelNames.Length)
			gameLevelNumber = 0;
		
		LoadLevel(gameLevelNumber);
		
		gameLevelNumber++;
	}

	private void LoadLevel(int indexNum)
	{
		LoadLevel(levelNames[indexNum]);
	}
}
