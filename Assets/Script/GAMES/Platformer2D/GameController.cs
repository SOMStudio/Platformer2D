using UnityEngine;
using System.Collections;

[AddComponentMenu("SOMStudio/Platformer2D/Game Controller")]
public class GameController : BaseGameController
{
	[Header("Main Settings")]
	[SerializeField] private bool startGame;

	[SerializeField] private string mainMenuSceneName = "Menu";
	[SerializeField] private GameObject[] playerPrefabList;

	[SerializeField] private SpawnPlatformsManager spawnController;

	[SerializeField] private Transform playerParent;
	[SerializeField] private Transform[] startPoints;

	[System.NonSerialized] public GameObject playerGameObject;

	private Vector3[] playerStarts;
	private Quaternion[] playerRotations;

	private ArrayList players;
	private ArrayList playerTransforms;

	private PlayerManager thePlayerScript;

	[System.NonSerialized] public BaseUserManager mainPlayerDataManager1;

	private int numberOfPlayers;

	[Header("Managers")]
	[SerializeField] private SpawnPlatformsManager spawnManager;
	[SerializeField] private LevelManager levelManager;
	[SerializeField] private GameMenu menuManager;
	[SerializeField] private BaseSoundController soundManager;

	[System.NonSerialized] public static GameController Instance;

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
			if (playerGameObject)
			{
				Vector3 playerPos = playerGameObject.transform.position;
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

		SpawnUtility.Restart();

		numberOfPlayers = playerPrefabList.Length;

		Vector3[] playerStartPositions = new Vector3 [numberOfPlayers];
		Quaternion[] playerStartRotations = new Quaternion [numberOfPlayers];

		for (int i = 0; i < numberOfPlayers; i++)
		{
			playerStartPositions[i] = startPoints[i].position;
			playerStartRotations[i] = startPoints[i].rotation;
		}

		SpawnUtility.SetUpPlayers(playerPrefabList, playerStartPositions, playerStartRotations, playerParent,
			numberOfPlayers);

		playerTransforms = new ArrayList();

		playerTransforms = SpawnUtility.GetAllSpawnedPlayers();

		players = new ArrayList();

		for (int i = 0; i < numberOfPlayers; i++)
		{
			Transform tempT = (Transform)playerTransforms[i];
			PlayerManager tempController = tempT.GetComponent<PlayerManager>();
			players.Add(tempController);
		}

		playerGameObject = SpawnUtility.GetPlayerGameObject(0);

		thePlayerScript = playerGameObject.GetComponent<PlayerManager>();

		thePlayerScript.SetID(0);

		thePlayerScript.SetUserInput(true);

		Transform aTarget = playerGameObject.transform.Find("CamTarget");

		if (aTarget != null)
		{
			Camera.main.SendMessage("SetTarget", aTarget);
		}
		else
		{
			Camera.main.SendMessage("SetTarget", playerGameObject.transform);
		}

		mainPlayerDataManager1 = playerGameObject.GetComponent<BasePlayerManager>().GetDataManager();

		Invoke(nameof(StartLevelFirst), 2);
	}

	private void InitManagers()
	{
		if (!spawnManager)
			spawnManager = SpawnPlatformsManager.Instance;

		if (!levelManager)
			levelManager = LevelManager.Instance;

		if (!menuManager)
			menuManager = GameMenu.Instance;

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

		spawnController.SpawnFirst(playerGameObject.transform.position);

		startGame = true;
	}

	private void StartLevelNext()
	{
		StartPlayer();

		mainPlayerDataManager1.SetScore(0);

		UpdateScoreP1(mainPlayerDataManager1.GetScore());

		spawnController.RemovePlatAll();

		spawnController.SpawnFirst(playerGameObject.transform.position);

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
