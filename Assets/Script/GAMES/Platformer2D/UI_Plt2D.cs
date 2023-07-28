using UnityEngine;

public class UI_Plt2D : BaseUIDataManager
{
	[Header("Manu settings")] [SerializeField]
	private GUIStyle myStyle;

	[SerializeField] private GameObject gameOverMessage;
	[SerializeField] private GameObject getReadyMessage;

	[System.NonSerialized] public static UI_Plt2D Instance;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);

		Init();
	}
	
	private void Init()
	{
		LoadHighScore();

		HideMessages();

		ShowGetReady();
		Invoke(nameof(HideMessages), 5);
	}

	private void HideMessages()
	{
		gameOverMessage.SetActive(false);
		getReadyMessage.SetActive(false);
	}

	private void ShowGetReady()
	{
		getReadyMessage.SetActive(true);
	}

	public void ShowGameOver()
	{
		// save high_score
		SaveHighScore();

		// show the game over message
		gameOverMessage.SetActive(true);
		Invoke(nameof(HideMessages), 3);
	}

	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 50), "PLAYER: 1", myStyle);
		GUI.Label(new Rect(10, 40, 100, 50), "SCORE: " + player_score, myStyle);
		GUI.Label(new Rect(10, 70, 200, 50), "HIGH SCORE: " + player_highscore, myStyle);
	}
}

