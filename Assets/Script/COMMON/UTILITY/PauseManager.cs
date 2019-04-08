using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour {

	public GameObject pauseWindow;

	private GameController_Plt2D gameController;

	// Use this for initialization
	void Start () {
		if (!gameController)
			gameController = GameController_Plt2D.Instance;

		pauseWindow.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameController.startGame) {
			if (Input.GetButtonDown ("Cancel")) {
				Pause ();
			}
		}
	}

	public void Pause(){
		pauseWindow.SetActive (!pauseWindow.activeSelf);

		gameController.Paused = (Time.timeScale == 1 ? true : false);
	}

	public void Quit(){
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	public void MainMenuScene(){
		//unpause game
		Pause ();

		//load menu
		Application.LoadLevel( "Menu" );
	}
}
