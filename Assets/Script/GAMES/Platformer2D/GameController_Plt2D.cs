using UnityEngine;
using System.Collections;

public class GameController_Plt2D : BaseGameController
{
	[Header("Main Settings")] [SerializeField]
	private bool startGame;

	[SerializeField] private string mainMenuSceneName = "Menu";
	[SerializeField] private GameObject[] playerPrefabList;

	[SerializeField] private SpawnPlatforms_Plt2D spawnController;

	[SerializeField] private Transform playerParent;
	[SerializeField] private Transform[] startPoints;

	[System.NonSerialized] public GameObject playerGO1;

	private Vector3[] playerStarts;
	private Quaternion[] playerRotations;

	private ArrayList playerList;
	private ArrayList playerTransforms;

	private Player_Plt2D thePlayerScript;
	private Player_Plt2D focusPlayerScript;

	[System.NonSerialized] public BaseUserManager mainPlayerDataManager1;

	private int numberOfPlayers;

	[Header("Managers")] [SerializeField] private SpawnPlatforms_Plt2D spawnManager;
	[SerializeField] private LevelManager levelManager;
	[SerializeField] private UI_Plt2D menuManager;
	[SerializeField] private BaseSoundController soundManager;

	[System.NonSerialized] public static GameController_Plt2D Instance;

	[SerializeField] private float gameSpeed = 1;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	public void Start()
	{
		Init();
	}

	public void Update()
	{
		if (startGame)
		{
			if (playerGO1)
			{
				Vector3 playerPos = playerGO1.transform.position;
				Vector3 firstPlatformPos = spawnManager.GetFirstPlatformPosition();
				if (
					playerPos.y < firstPlatformPos.y
					&& (playerPos - firstPlatformPos).magnitude > spawnManager.HorizontalMax * 2
				)
				{
					thePlayerScript.GameEnd();

					menuManager.ShowGameOver();

					Invoke(nameof(StartLevelNext), 1);

					startGame = false;
				}
			}
		}
	}

	private void Init()
	{
		InitManagers();

		SpawnController.Instance.Restart();

		numberOfPlayers = playerPrefabList.Length;

		Vector3[] playerStarts = new Vector3 [numberOfPlayers];
		Quaternion[] playerRotations = new Quaternion [numberOfPlayers];

		for (int i = 0; i < numberOfPlayers; i++)
		{
			playerStarts[i] = startPoints[i].position;
			playerRotations[i] = startPoints[i].rotation;
		}

		SpawnController.Instance.SetUpPlayers(playerPrefabList, playerStarts, playerRotations, playerParent,
			numberOfPlayers);

		playerTransforms = new ArrayList();

		playerTransforms = SpawnController.Instance.GetAllSpawnedPlayers();

		playerList = new ArrayList();

		for (int i = 0; i < numberOfPlayers; i++)
		{
			Transform tempT = (Transform)playerTransforms[i];
			Player_Plt2D tempController = tempT.GetComponent<Player_Plt2D>();
			playerList.Add(tempController);
		}

		playerGO1 = SpawnController.Instance.GetPlayerGO(0);

		thePlayerScript = playerGO1.GetComponent<Player_Plt2D>();

		thePlayerScript.SetID(0);

		thePlayerScript.SetUserInput(true);

		focusPlayerScript = thePlayerScript;

		Transform aTarget = playerGO1.transform.Find("CamTarget");

		if (aTarget != null)
		{
			Camera.main.SendMessage("SetTarget", aTarget);
		}
		else
		{
			Camera.main.SendMessage("SetTarget", playerGO1.transform);
		}

		mainPlayerDataManager1 = playerGO1.GetComponent<BasePlayerManager>().GetDataManager();

		Invoke(nameof(StartLevelFirst), 2);
	}

	private void InitManagers()
	{
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
		thePlayerScript.GameStart();
	}

	private void StartLevelFirst()
	{
		StartPlayer();

		spawnController.SpawnFirst(playerGO1.transform.position);

		startGame = true;
	}

	private void StartLevelNext()
	{
		StartPlayer();

		mainPlayerDataManager1.SetScore(0);

		UpdateScoreP1(mainPlayerDataManager1.GetScore());

		spawnController.RemovePlatAll();

		spawnController.SpawnFirst(playerGO1.transform.position);

		startGame = true;
	}

	public bool IsGameStart => startGame;

	public void PlatformDrop(GameObject val)
	{
		spawnManager.RemovePlatform(val);
	}

	public void CoinsTake(Vector3 aPosition, int pointsValue, int takeByID)
	{
		soundManager.PlaySoundByIndex(0, aPosition);

		mainPlayerDataManager1.AddScore(pointsValue);

		UpdateScoreP1(mainPlayerDataManager1.GetScore());
	}

	public void PlayerHit(Transform whichPlayer)
	{
		soundManager.PlaySoundByIndex(2, whichPlayer.position);

		Explode(whichPlayer.position);
	}

	public void PlayerDied(int whichID)
	{
		menuManager.ShowGameOver();
	}

	private void Exit()
	{
		levelManager.LoadLevel(mainMenuSceneName);
	}

	public void UpdateScoreP1(int aScore)
	{
		menuManager.UpdateScoreP1(aScore);
	}

	public void UpdateLivesP1(int aScore)
	{
		menuManager.UpdateLivesP1(aScore);
	}
}
