using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("UI/Generic Main Menu")]
public class MainMenuController : MonoBehaviour
{
	[SerializeField] private int whichMenu = 0;

	[Header("Game Settings")]
	[SerializeField] private GUISkin menuSkin;

	[SerializeField] private string gameDisplayName = "- DEFAULT GAME NAME -";
	[SerializeField] private string gamePrefsName = "DefaultGame";

	[SerializeField] private float default_width = 720;
	[SerializeField] private float default_height = 480;

	[SerializeField] private float audioSFXSliderValue = 1;
	[SerializeField] private float audioMusicSliderValue = 1;

	[SerializeField] private float graphicsSliderValue = 6;
	private int detailLevels = 6;

	[Header("Def Scene")]
	[SerializeField] private string singleGameStartScene;
	[SerializeField] private string coopGameStartScene;

	[Header("Level Manager")]
	[SerializeField] private bool useLevelManagerToStartGame;

	[SerializeField] private string[] gameLevels;
	[SerializeField] private LevelManager levelManager;

	[SerializeField] private bool isLoading;

	[SerializeField] private BaseSoundController soundManager;

	[SerializeField] private BaseMusicController musicManager;

	private void Start()
	{
		if (PlayerPrefs.HasKey(gamePrefsName + "_SFXVol"))
		{
			audioSFXSliderValue = PlayerPrefs.GetFloat(gamePrefsName + "_SFXVol");
		}
		else
		{
			string[] names = QualitySettings.names;
			detailLevels = names.Length;
			graphicsSliderValue = detailLevels;

			SaveOptionsPrefs();
		}

		if (PlayerPrefs.HasKey(gamePrefsName + "_MusicVol"))
		{
			audioMusicSliderValue = PlayerPrefs.GetFloat(gamePrefsName + "_MusicVol");
		}

		if (PlayerPrefs.HasKey(gamePrefsName + "_GraphicsDetail"))
		{
			graphicsSliderValue = PlayerPrefs.GetFloat(gamePrefsName + "_GraphicsDetail");
		}

		Debug.Log("quality=" + graphicsSliderValue);

		QualitySettings.SetQualityLevel((int)graphicsSliderValue, true);

		if (levelManager == null)
		{
			if (gameLevels.Length > 0)
			{
				levelManager.LevelNames = gameLevels;
			}
		}

		if (soundManager == null)
		{
			soundManager = BaseSoundController.Instance;
			soundManager.UpdateVolume();
		}

		if (musicManager == null)
		{
			musicManager = BaseMusicController.Instance;
			musicManager.UpdateVolume();
		}
	}

	private void OnGUI()
	{
		float scaleX = Screen.width / default_width;
		float scaleY = Screen.height / default_height;
		GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(scaleX, scaleY, 1));
		
		GUI.skin = menuSkin;

		switch (whichMenu)
		{
			case 0:
				GUI.BeginGroup(new Rect(default_width / 2 - 150, default_height / 2 - 250, 500, 500));

				GUI.Label(new Rect(0, 50, 300, 50), gameDisplayName, "textarea");

				if (GUI.Button(new Rect(0, 200, 300, 40), "START SINGLE", "button") && !isLoading)
				{
					PlayerPrefs.SetInt("totalPlayers", 1);
					if (!useLevelManagerToStartGame)
					{
						isLoading = true;
						Debug.Log("Telling level Manager to load single scene mode..");
						LoadLevel(singleGameStartScene);
					}
					else
					{
						isLoading = true;
						Debug.Log("Telling level Manager to load next level..");
						levelManager.GoNextLevel();
					}
				}

				if (coopGameStartScene != "")
				{
					if (!isLoading && GUI.Button(new Rect(0, 250, 300, 40), "START CO-OP"))
					{
						PlayerPrefs.SetInt("totalPlayers", 2);

						if (!useLevelManagerToStartGame)
						{
							LoadLevel(coopGameStartScene);
						}
						else
						{
							isLoading = true;
							levelManager.GoNextLevel();
						}

					}

					if (GUI.Button(new Rect(0, 300, 300, 40), "OPTIONS"))
					{
						ShowOptionsMenu();
					}
				}
				else
				{
					if (GUI.Button(new Rect(0, 250, 300, 40), "OPTIONS"))
					{
						ShowOptionsMenu();
					}
				}
				
				if (GUI.Button(new Rect(0, 400, 300, 40), "EXIT"))
				{
					ConfirmExitGame();
				}
				
				GUI.EndGroup();

				break;

			case 1:
				GUI.BeginGroup(new Rect(default_width / 2 - 150, default_height / 2 - 250, 500, 500));
				
				GUI.Label(new Rect(0, 50, 300, 50), "OPTIONS", "textarea");

				if (GUI.Button(new Rect(0, 250, 300, 40), "AUDIO OPTIONS"))
				{
					ShowAudioOptionsMenu();
				}

				if (GUI.Button(new Rect(0, 300, 300, 40), "GRAPHICS OPTIONS"))
				{
					ShowGraphicsOptionsMenu();
				}

				if (GUI.Button(new Rect(0, 400, 300, 40), "BACK TO MAIN MENU"))
				{
					GoMainMenu();
				}

				GUI.EndGroup();

				break;

			case 2:
				GUI.BeginGroup(new Rect(default_width / 2 - 150, default_height / 2 - 250, 500, 500));
				
				GUI.Label(new Rect(0, 50, 300, 50), "Are you sure you want to exit?", "textarea");

				if (GUI.Button(new Rect(0, 250, 300, 40), "YES, QUIT PLEASE!"))
				{
					ExitGame();
				}

				if (GUI.Button(new Rect(0, 300, 300, 40), "NO, DON'T QUIT"))
				{
					GoMainMenu();
				}

				GUI.EndGroup();

				break;

			case 3:
				GUI.BeginGroup(new Rect(default_width / 2 - 150, default_height / 2 - 250, 500, 500));
				
				GUI.Label(new Rect(0, 50, 300, 50), "AUDIO OPTIONS", "textarea");

				GUI.Label(new Rect(0, 170, 300, 20), "SFX volume:");
				float audioSfxSliderValueNew =
					GUI.HorizontalSlider(new Rect(0, 200, 300, 50), audioSFXSliderValue, 0.0f, 1f);

				GUI.Label(new Rect(0, 270, 300, 20), "Music volume:");
				float audioMusicSliderValueNew =
					GUI.HorizontalSlider(new Rect(0, 300, 300, 50), audioMusicSliderValue, 0.0f, 1f);

				if (audioSfxSliderValueNew != audioSFXSliderValue)
				{
					audioSFXSliderValue = audioSfxSliderValueNew;

					if (soundManager != null)
					{
						SaveOptionsPrefs();

						soundManager.UpdateVolume();
					}
				}

				if (audioMusicSliderValueNew != audioMusicSliderValue)
				{
					audioMusicSliderValue = audioMusicSliderValueNew;

					if (musicManager != null)
					{
						SaveOptionsPrefs();

						musicManager.UpdateVolume();
					}
				}

				if (GUI.Button(new Rect(0, 400, 300, 40), "BACK TO MAIN MENU"))
				{
					SaveOptionsPrefs();
					ShowOptionsMenu();
				}

				GUI.EndGroup();
				break;

			case 4:
				GUI.BeginGroup(new Rect(default_width / 2 - 150, default_height / 2 - 250, 500, 500));
				
				GUI.Label(new Rect(0, 50, 300, 50), "GRAPHICS OPTIONS", "textarea");

				GUI.Label(new Rect(0, 170, 300, 20), "Graphics quality:");
				graphicsSliderValue =
					Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(0, 200, 300, 50), graphicsSliderValue, 0,
						detailLevels));


				if (GUI.Button(new Rect(0, 400, 300, 40), "BACK TO MAIN MENU"))
				{
					SaveOptionsPrefs();
					ShowOptionsMenu();
				}

				GUI.EndGroup();
				break;
		}	
	}
	
	private void LoadLevel(string whichLevel)
	{
		levelManager.LoadLevel(whichLevel);
	}

	private void GoMainMenu()
	{
		whichMenu = 0;
	}

	private void ShowOptionsMenu()
	{
		whichMenu = 1;
	}

	private void ShowAudioOptionsMenu()
	{
		whichMenu = 3;
	}

	private void ShowGraphicsOptionsMenu()
	{
		whichMenu = 4;
	}

	private void SaveOptionsPrefs()
	{
		PlayerPrefs.SetFloat(gamePrefsName + "_SFXVol", audioSFXSliderValue);
		PlayerPrefs.SetFloat(gamePrefsName + "_MusicVol", audioMusicSliderValue);
		PlayerPrefs.SetFloat(gamePrefsName + "_GraphicsDetail", graphicsSliderValue);
		
		QualitySettings.SetQualityLevel((int)graphicsSliderValue, true);
	}

	private void ConfirmExitGame()
	{
		whichMenu = 2;
	}

	static void ExitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
