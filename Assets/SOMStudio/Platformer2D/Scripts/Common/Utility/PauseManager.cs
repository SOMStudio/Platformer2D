using SOMStudio.Platformer2D.Scripts.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SOMStudio.Platformer2D.Scripts.Common.Utility
{
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
				if (UnityEngine.Input.GetButtonDown("Cancel"))
				{
					Pause();
				}
			}
		}

		private void Pause()
		{
			pauseWindow.SetActive(!pauseWindow.activeSelf);

			gameController.Paused = Mathf.Approximately(Time.timeScale, 1);
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
			Pause();
		
			SceneManager.LoadScene("Menu");
		}
	}
}
