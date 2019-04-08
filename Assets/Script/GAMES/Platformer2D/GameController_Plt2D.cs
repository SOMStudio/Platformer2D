using UnityEngine;
using System.Collections;

[AddComponentMenu("Sample Game Glue Code/My/Game Controller")]

public class GameController_Plt2D : BaseGameController
{
	public bool startGame = false;

	public string mainMenuSceneName = "Menu";
	public GameObject[] playerPrefabList;
	
	public SpawnPlatforms_Plt2D spawnController;
	
	public Transform playerParent;
    public Transform [] startPoints;
    
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
	public SpawnPlatforms_Plt2D spawnManager;
	public UI_Plt2D menuManager;
	public BaseSoundController soundManager;
	
	[System.NonSerialized]
	public static GameController_Plt2D Instance;
	
	public float gameSpeed=1;

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
				if ((playerPos - firstPlatformPos).magnitude > spawnManager.horizontalMax * 2) {
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
	
	public void Init()
	{
		// init managers
		InitManagers ();

		SpawnController.Instance.Restart();
		
		numberOfPlayers= playerPrefabList.Length;
		
		// initialize some temporary arrays we can use to set up the players
        Vector3 [] playerStarts = new Vector3 [numberOfPlayers];
        Quaternion [] playerRotations = new Quaternion [numberOfPlayers];

        // we are going to use the array full of start positions that must be set in the editor, which means we always need to
        // make sure that there are enough start positions for the number of players

        for ( int i = 0; i < numberOfPlayers; i++ )
        {
            // grab position and rotation values from start position transforms set in the inspector
            playerStarts [i] = (Vector3) startPoints [i].position;
            playerRotations [i] = ( Quaternion ) startPoints [i].rotation;
        }
		
        SpawnController.Instance.SetUpPlayers( playerPrefabList, playerStarts, playerRotations, playerParent, numberOfPlayers );
		
		playerTransforms=new ArrayList();
		
		// now let's grab references to each player's controller script
		playerTransforms = SpawnController.Instance.GetAllSpawnedPlayers();
		
		playerList=new ArrayList();
		
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
		mainPlayerDataManager1= playerGO1.GetComponent<BasePlayerManager>().DataManager;

		//create platforms
		Invoke ("StartLevelFirst", 2);
	}

	void InitManagers() {
		if (!spawnManager)
			spawnManager = SpawnPlatforms_Plt2D.Instance;

		if (!menuManager)
			menuManager = UI_Plt2D.Instance;

		if (!soundManager)
			soundManager = BaseSoundController.Instance;
	}
	
	void StartPlayer()
	{
		// all ready to play, let's go!
		thePlayerScript.GameStart();
	}

	void StartLevelFirst() {
		// all ready to play, let's go!
		StartPlayer ();

		//create platforms
		spawnController.SpawnFirst (playerGO1.transform.position);

		startGame = true;
	}

	void StartLevelNext() {
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

	public override void EnemyDestroyed ( Vector3 aPosition, int pointsValue, int hitByID )
	{
		// tell our sound controller to play an explosion sound
		soundManager.PlaySoundByIndex( 0, aPosition );
		
		// tell main data manager to add score
		mainPlayerDataManager1.AddScore( pointsValue );
			
		// update the score on the UI
		UpdateScoreP1( mainPlayerDataManager1.GetScore() );
		
		// play an explosion effect at the enemy position
		Explode ( aPosition );
		
		// tell spawn controller that we're one enemy closer to the next wave
		//spawnController.SpawnFirst();
	}
	
	public void PlayerHit(Transform whichPlayer)
	{
		// tell our sound controller to play an explosion sound
		soundManager.PlaySoundByIndex( 2, whichPlayer.position );
		
		// call the explosion function!
		Explode( whichPlayer.position );
	}
	
	public Player_Plt2D GetMainPlayerScript ()
	{
		return focusPlayerScript;
	}
	
	public Transform GetMainPlayerTransform ()
	{
		return playerGO1.transform;
	}
	
	public GameObject GetMainPlayerGO ()
	{
		return playerGO1;
	}
	
	public void PlayerDied(int whichID)
	{
		// this is a single player game, so just end the game now
		// both players are dead, so end the game
		menuManager.ShowGameOver();
		//Invoke ("Exit",5);
	}
	
	void Exit()
	{
		Application.LoadLevel( mainMenuSceneName );
	}
	
	// UI update calls
	// 
	public void UpdateScoreP1( int aScore )
	{
		menuManager.UpdateScoreP1( aScore );
	} 
	
	public void UpdateLivesP1( int aScore )
	{
		menuManager.UpdateLivesP1( aScore );
	}
	
}
