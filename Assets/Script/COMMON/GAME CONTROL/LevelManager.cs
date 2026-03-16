using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private string[] levelNames;
	[SerializeField] private int gameLevelNum;

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
		gameLevelNum = 0;
	}

	public void GoNextLevel()
	{
		if (gameLevelNum >= levelNames.Length)
			gameLevelNum = 0;
		
		LoadLevel(gameLevelNum);
		
		gameLevelNum++;
	}

	private void LoadLevel(int indexNum)
	{
		LoadLevel(levelNames[indexNum]);
	}
}
