using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController_Plt2D : BaseGameController
{
	[Header("Main Settings")]
	[SerializeField]
	private bool startGame = false;

	[SerializeField]
	private string mainMenuSceneName = "Menu";
	[SerializeField]
	private GameObject[] playerPrefabList;
	
	[SerializeField]
	private SpawnPlatforms_Plt2D spawnController;
	
	[SerializeField]
	private Transform playerParent;
	[SerializeField]
	private Transform [] startPoints;
    
	[System.NonSerialized]
    public GameObject playerGO1;
	
	private Vector3[] playerStarts;
	private Quaternion[] playerRotations;
	
    private ArrayList playerList;
	private ArrayList playerTransforms;
	
	private Player_Plt2D thePlayerScript;
	private Player_Plt2D focusPlayerScript;
	
	[System.NonSerialized]
	public BaseUserManager mainPlayerDataManager1;
	
	private int numberOfPlayers;

	[Header("Managers")]
	[SerializeField]
	private SpawnPlatforms_Plt2D spawnManager;
	[SerializeField]
	private LevelManager levelManager;
	[SerializeField]
	private UI_Plt2D menuManager;
	[SerializeField]
	private BaseSoundController soundManager;
	
	[System.NonSerialized]
	public static GameController_Plt2D Instance;
	
	[SerializeField]
	private float gameSpeed = 1;

	// main event
	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (this);
	}
	
	public void Start()
	{
		Init();
	}

	public void Update() {
		if (startGame) {
			if (playerGO1) {
				Vector3 playerPos = playerGO1.transform.position;
				Vector3 firstPlatformPos = spawnManager.GetFirstPlatformPossition ();
				if (
					(playerPos.y < firstPlatformPos.y) // not react for jump up
					&& ((playerPos - firstPlatformPos).magnitude > spawnManager.HorizontalMax * 2) // chack distance
				) {
					// player not controll
					thePlayerScript.GameEnd ();

					// show lose life
					menuManager.ShowGameOver ();

					// contunue play
					Invoke ("StartLevelNext", 1);

					startGame = false;
				}
			}
		}
	}

	// main logic
	private void Init()
	{
		// init managers
		InitManagers ();

		SpawnController.Instance.Restart();
		
		numberOfPlayers = playerPrefabList.Length;
		
		// initialize some temporary arrays we can use to set up the players
        Vector3 [] playerStarts = new Vector3 [numberOfPlayers];
        Quaternion [] playerRotations = new Quaternion [numberOfPlayers];

        for ( int i = 0; i < numberOfPlayers; i++ )
        {
            // grab position and rotation values from start position transforms set in the inspector
            playerStarts [i] = (Vector3) startPoints [i].position;
            playerRotations [i] = ( Quaternion ) startPoints [i].rotation;
        }
		
        SpawnController.Instance.SetUpPlayers( playerPrefabList, playerStarts, playerRotations, playerParent, numberOfPlayers );
		
		playerTransforms = new ArrayList ();
		
		// now let's grab references to each player's controller script
		playerTransforms = SpawnController.Instance.GetAllSpawnedPlayers();
		
		playerList = new ArrayList ();
		
		for ( int i = 0; i < numberOfPlayers; i++ )
        {
			Transform tempT= (Transform)playerTransforms[i];
			Player_Plt2D tempController= tempT.GetComponent<Player_Plt2D>();
			playerList.Add(tempController);
		}
		
        // grab a ref to the player's gameobject for later
        playerGO1 = SpawnController.Instance.GetPlayerGO( 0 );

        // grab a reference to the focussed player's car controller script, so that we can
        // do things like access its speed variable
        thePlayerScript = ( Player_Plt2D ) playerGO1.GetComponent<Player_Plt2D>();

        // assign this player the id of 0
        thePlayerScript.SetID( 0 );

        // set player control
        thePlayerScript.SetUserInput( true );

        // as this is the user, we want to focus on this for UI etc.
        focusPlayerScript = thePlayerScript;

		// see if we have a camera target object to look at
		Transform aTarget= playerGO1.transform.Find("CamTarget");

		if(aTarget!=null)
		{
			// if we have a camera target to aim for, instead of the main player, we use that instead
			Camera.main.SendMessage("SetTarget", aTarget );
		} else {
        	// tell the camera script to target the player
			Camera.main.SendMessage("SetTarget", playerGO1.transform );
		}

		// grab a reference to the main player's data manager so we can update its values later on (scoring, lives etc.)
		mainPlayerDataManager1= playerGO1.GetComponent<BasePlayerManager>().GetDataManager();

		//create platforms
		Invoke ("StartLevelFirst", 2);
	}

	private void InitManagers() {
		if (!spawnManager)
			spawnManager = SpawnPlatforms_Plt2D.Instance;

		if (!levelManager)
			levelManager = LevelManager.Instance;

		if (!menuManager)
			menuManager = UI_Plt2D.Instance;

		if (!soundManager)
			soundManager = BaseSoundController.Instance;
	}
	
	private void StartPlayer()
	{
		// all ready to play, let's go!
		thePlayerScript.GameStart();
	}

	private void StartLevelFirst() {
		// all ready to play, let's go!
		StartPlayer ();

		//create platforms
		spawnController.SpawnFirst (playerGO1.transform.position);

		startGame = true;
	}

	private void StartLevelNext() {
		// all ready to play, let's go!
		StartPlayer ();

		// tell main data manager to clear score
		mainPlayerDataManager1.SetScore( 0 );

		// update the score on the UI
		UpdateScoreP1( mainPlayerDataManager1.GetScore() );

		// clear platforms
		spawnController.RemovePlatAll ();

		//create platforms
		spawnController.SpawnFirst (playerGO1.transform.position);

		startGame = true;
	}

	public bool IsGameStart { get { return startGame; } }

	public void PlatformDrop(GameObject val) {
		// tell our sound controller to play an sound
		//soundManager.PlaySoundByIndex( 0, aPosition );

		// remove from list
		spawnManager.RemovePlatform (val);
	}

	public void CoinsTaked ( Vector3 aPosition, int pointsValue, int takeByID )
	{
		// tell our sound controller to play an sound
		soundManager.PlaySoundByIndex( 0, aPosition );
		
		// tell main data manager to add score
		mainPlayerDataManager1.AddScore( pointsValue );

		// update the score on the UI
		UpdateScoreP1( mainPlayerDataManager1.GetScore() );
	}
	
	public void PlayerHit(Transform whichPlayer)
	{
		// play sound
		soundManager.PlaySoundByIndex( 2, whichPlayer.position );
		
		// call the explosion function!
		Explode( whichPlayer.position );
	}
	
	public void PlayerDied(int whichID)
	{
		menuManager.ShowGameOver();
		//Invoke ("Exit",5);
	}
	
	private void Exit()
	{
		levelManager.LoadLevel (mainMenuSceneName);
	}
	
	// UI update calls
	public void UpdateScoreP1( int aScore )
	{
		menuManager.UpdateScoreP1( aScore );
	} 
	
	public void UpdateLivesP1( int aScore )
	{
		menuManager.UpdateLivesP1( aScore );
	}
	
}
