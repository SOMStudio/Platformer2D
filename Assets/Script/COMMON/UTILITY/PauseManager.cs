using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour
{
	[SerializeField] private GameObject pauseWindow;
	[SerializeField] private BaseGameController gameController;
	
	private void Start()
	{
		pauseWindow.SetActive(false);
	}

	private void Update()
	{
		if (!gameController.Paused)
		{
			if (Input.GetButtonDown("Cancel"))
			{
				Pause();
			}
		}
	}

	private void Pause()
	{
		pauseWindow.SetActive(!pauseWindow.activeSelf);

		gameController.Paused = Time.timeScale == 1;
	}

	public void Quit()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void MainMenuScene()
	{
		//unpause game
		Pause();

		//load menu
		SceneManager.LoadScene("Menu");
	}
}
