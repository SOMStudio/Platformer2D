using UnityEngine;
using System.Collections;

[AddComponentMenu("Sample Game Glue Code/Laser Blast Survival/In-Game UI")]

public class UI_Plt2D : BaseUIDataManager
{
	public GUIStyle myStyle;
	public GameObject gameOverMessage;
	public GameObject getReadyMessage;

	[System.NonSerialized]
	public static UI_Plt2D Instance;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (this);
		
		Init ();
	}
	
	void Init()
	{
		LoadHighScore();

        HideMessages ();

		ShowGetReady ();
		Invoke ("HideMessages", 5);
    }
	
	public void HideMessages()
	{
		gameOverMessage.SetActive(false);
		getReadyMessage.SetActive(false);
	}

	public void ShowGetReady()
	{
		getReadyMessage.SetActive(true);
	}

	public void ShowGameOver()
	{
		// save high_score
		SaveHighScore ();
		
		// show the game over message
		gameOverMessage.SetActive(true);
		Invoke ("HideMessages", 3);
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect (10,10,100,50),"PLAYER: 1", myStyle);
		GUI.Label(new Rect (10,40,100,50),"SCORE: "+player_score, myStyle);
		GUI.Label(new Rect (10,70,200,50),"HIGH SCORE: "+player_highscore, myStyle);
	}
}

