using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour {

	[SerializeField]
	private GameObject pauseWindow;
	[SerializeField]
	private BaseGameController gameController;

	// main event
	void Start () {
		pauseWindow.SetActive (false);
	}

	void Update () {
		if (!gameController.Paused) {
			if (Input.GetButtonDown ("Cancel")) {
				Pause ();
			}
		}
	}

	// main logic
	void Pause(){
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
		SceneManager.LoadScene( "Menu" );
	}
}
