using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	[SerializeField]
	private string[] levelNames;
	[SerializeField]
	private int gameLevelNum;

	[System.NonSerialized]
	public static LevelManager Instance;

	// main event
	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (this);
	}

	void Start ()
	{
		// keep this object alive
		DontDestroyOnLoad (this.gameObject);
	}

	// main logic
	public string[] LevelNames {
		get { return levelNames; }
		set { levelNames = value; }
	}

	public void LoadLevel( string sceneName )
	{
		//Application.LoadLevel( sceneName );
		SceneManager.LoadScene ( sceneName );
	}
		
	public void ResetGame()
	{
		// reset the level index counter
		gameLevelNum = 0;
	}
	
	public void GoNextLevel()
	{
		// if our index goes over the total number of levels in the array, we reset it
		if( gameLevelNum >= levelNames.Length )
			gameLevelNum = 0;
		
		// load the level (the array index starts at 0, but we start counting game levels at 1 for clarity's sake)
		LoadLevel( gameLevelNum );
		
		// increase our game level index counter
		gameLevelNum++;
	}
	
	private void LoadLevel( int indexNum )
	{
		// load the game level
		LoadLevel( levelNames[indexNum] );
	}
}
