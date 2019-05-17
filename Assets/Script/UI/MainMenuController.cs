using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("UI/Generic Main Menu")]

public class MainMenuController : MonoBehaviour
{
	[SerializeField]
	private int whichMenu= 0;

	[Header("Game Settings")]
	[SerializeField]
	private GUISkin menuSkin;
	
	[SerializeField]
	private string gameDisplayName= "- DEFAULT GAME NAME -";
	[SerializeField]
	private string gamePrefsName= "DefaultGame";
	
	[SerializeField]
	private float default_width= 720;
	[SerializeField]
	private float default_height= 480;
	
	[SerializeField]
	private float audioSFXSliderValue = 1;
	[SerializeField]
	private float audioMusicSliderValue = 1;
	
	[SerializeField]
	private float graphicsSliderValue = 6;
	private int detailLevels= 6;

	[Header("Def Scene")]
	[SerializeField]
	private string singleGameStartScene;
	[SerializeField]
	private string coopGameStartScene;

	[Header("Level Manager")]
	[SerializeField]
	private bool useLevelManagerToStartGame;
	
	[SerializeField]
	private string[] gameLevels;
	[SerializeField]
	private LevelManager levelManager;

	[SerializeField]
	private bool isLoading;

	[SerializeField]
	private BaseSoundController soundManager;

	[SerializeField]
	private BaseMusicController musicManager;

	// main event
	void Start()
	{
		// set up default options, if they have been saved out to prefs already
		if(PlayerPrefs.HasKey(gamePrefsName+"_SFXVol"))
		{
			audioSFXSliderValue= PlayerPrefs.GetFloat(gamePrefsName+"_SFXVol");
		} else {
			// if we are missing an SFXVol key, we won't got audio defaults set up so let's do that now
			string[] names = QualitySettings.names;
			detailLevels = names.Length;
			graphicsSliderValue = detailLevels;
			// save defaults
			SaveOptionsPrefs();
		}
		if(PlayerPrefs.HasKey(gamePrefsName+"_MusicVol"))
		{
			audioMusicSliderValue= PlayerPrefs.GetFloat(gamePrefsName+"_MusicVol");
		}
		if(PlayerPrefs.HasKey(gamePrefsName+"_GraphicsDetail"))
		{
			graphicsSliderValue= PlayerPrefs.GetFloat(gamePrefsName+"_GraphicsDetail");
		}

		Debug.Log ("quality="+graphicsSliderValue);

		// set the quality setting
		QualitySettings.SetQualityLevel( (int)graphicsSliderValue, true);

		// level manager
		if (levelManager == null) {
			if (gameLevels.Length > 0) {
				levelManager.LevelNames = gameLevels;
			}
		}

		// sound manager
		if (soundManager == null) {
			soundManager = BaseSoundController.Instance;

			soundManager.UpdateVolume ();
		}

		// music manager
		if (musicManager == null) {
			musicManager = BaseMusicController.Instance;

			musicManager.UpdateVolume ();
		}

	}
	
	void OnGUI()
	{
		float scaleX = Screen.width / default_width; 
		float scaleY = Screen.height / default_height; 
		GUI.matrix = Matrix4x4.TRS (new Vector3(0, 0, 0), Quaternion.identity, new Vector3 (scaleX, scaleY, 1));
				
		// set the GUI skin to use our custom menu skin
		GUI.skin= menuSkin;

		switch(whichMenu)
		{
		case 0:	
			GUI.BeginGroup (new Rect (default_width / 2 - 150, default_height / 2 - 250, 500, 500));

			// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.
					
			GUI.Label(new Rect( 0, 50, 300, 50 ), gameDisplayName, "textarea");
			
			if(GUI.Button(new Rect( 0, 200, 300, 40 ),"START SINGLE", "button") && !isLoading)
			{
				PlayerPrefs.SetInt( "totalPlayers", 1 );
				if(!useLevelManagerToStartGame)
				{
					isLoading=true;
					Debug.Log ("Telling level Manager to load single scene mode..");
					LoadLevel( singleGameStartScene );
				} else {
					isLoading=true;
					Debug.Log ("Telling level Manager to load next level..");
					levelManager.GoNextLevel();	
				}
			}
			
			if(coopGameStartScene!="")
			{
				if(!isLoading && GUI.Button(new Rect(0, 250, 300, 40 ),"START CO-OP"))
				{
					PlayerPrefs.SetInt( "totalPlayers", 2 );
					
					if(!useLevelManagerToStartGame)
					{
						LoadLevel( coopGameStartScene );
					} else {
						isLoading=true;
						levelManager.GoNextLevel();	
					}
				
				}
				
				if(GUI.Button(new Rect(0, 300, 300, 40 ),"OPTIONS"))
				{
					ShowOptionsMenu();
				}
			} else {
				if(GUI.Button(new Rect(0, 250, 300, 40 ),"OPTIONS"))
				{
					ShowOptionsMenu();
				}
			}
			
			
			
			if(GUI.Button(new Rect(0, 400, 300, 40 ),"EXIT"))
			{
				ConfirmExitGame();
			}
			
			// End the group we started above. This is very important to remember!
			GUI.EndGroup ();
			
		break;
		
		case 1:
			// Options menu
			GUI.BeginGroup (new Rect (default_width / 2 - 150, default_height / 2 - 250, 500, 500));

			// Are you sure you want to exit?
			GUI.Label(new Rect( 0, 50, 300, 50 ), "OPTIONS", "textarea");
			
			if(GUI.Button(new Rect(0, 250, 300, 40 ),"AUDIO OPTIONS"))
			{
				ShowAudioOptionsMenu();
			}
			
			if(GUI.Button(new Rect(0, 300, 300, 40 ),"GRAPHICS OPTIONS"))
			{
				ShowGraphicsOptionsMenu();
			}
			
			if(GUI.Button(new Rect(0, 400, 300, 40 ),"BACK TO MAIN MENU"))
			{
				GoMainMenu();
			}
			
			GUI.EndGroup ();
			
		break;
		
		case 2:
			GUI.BeginGroup (new Rect (default_width / 2 - 150, default_height / 2 - 250, 500, 500));

			// Are you sure you want to exit?
			GUI.Label(new Rect( 0, 50, 300, 50 ), "Are you sure you want to exit?", "textarea");
			
			if(GUI.Button(new Rect(0, 250, 300, 40 ),"YES, QUIT PLEASE!"))
			{
				ExitGame();
			}
			
			if(GUI.Button(new Rect(0, 300, 300, 40 ),"NO, DON'T QUIT"))
			{
				GoMainMenu();
			}
			
			GUI.EndGroup ();
			
		break;
		
		case 3:
			// AUDIO OPTIONS
			GUI.BeginGroup (new Rect (default_width / 2 - 150, default_height / 2 - 250, 500, 500));

			// Are you sure you want to exit?
			GUI.Label (new Rect (0, 50, 300, 50), "AUDIO OPTIONS", "textarea");
			
			GUI.Label (new Rect (0, 170, 300, 20), "SFX volume:");
			float audioSFXSliderValue_new = GUI.HorizontalSlider (new Rect (0, 200, 300, 50), audioSFXSliderValue, 0.0f, 1f);

			GUI.Label (new Rect (0, 270, 300, 20), "Music volume:");
			float audioMusicSliderValue_new = GUI.HorizontalSlider (new Rect (0, 300, 300, 50), audioMusicSliderValue, 0.0f, 1f);

			if (audioSFXSliderValue_new != audioSFXSliderValue) {
				audioSFXSliderValue = audioSFXSliderValue_new;

				if (soundManager != null) {
					SaveOptionsPrefs();

					soundManager.UpdateVolume ();
				}
			}

			if (audioMusicSliderValue_new != audioMusicSliderValue) {
				audioMusicSliderValue = audioMusicSliderValue_new;

				if (musicManager != null) {
					SaveOptionsPrefs();

					musicManager.UpdateVolume ();
				}
			}

			if(GUI.Button(new Rect(0, 400, 300, 40 ),"BACK TO MAIN MENU"))
			{
				SaveOptionsPrefs();
				ShowOptionsMenu();
			}
			
			GUI.EndGroup ();
		break;
		
		case 4:
			// GRAPHICS OPTIONS
			GUI.BeginGroup (new Rect (default_width / 2 - 150, default_height / 2 - 250, 500, 500));

			// Are you sure you want to exit?
			GUI.Label(new Rect( 0, 50, 300, 50 ), "GRAPHICS OPTIONS", "textarea");
			
			GUI.Label(new Rect(0, 170, 300, 20), "Graphics quality:");
			graphicsSliderValue = Mathf.RoundToInt(GUI.HorizontalSlider (new Rect( 0, 200, 300, 50 ), graphicsSliderValue, 0, detailLevels));

			
			if(GUI.Button(new Rect(0, 400, 300, 40 ),"BACK TO MAIN MENU"))
			{
				SaveOptionsPrefs();
				ShowOptionsMenu();
			}
			
			GUI.EndGroup ();
		break;
		
		} // <- end switch	
	}

	// main logic
	void LoadLevel( string whichLevel )
	{
		// tell the levelManager object to deal with loading the level
		levelManager.LoadLevel ( whichLevel );
	}
	
	void GoMainMenu()
	{
		whichMenu=0;	
	}
	
	void ShowOptionsMenu()
	{
		whichMenu=1;
	}
	
	void ShowAudioOptionsMenu()
	{
		whichMenu= 3;
	}
	
	void ShowGraphicsOptionsMenu()
	{
		whichMenu= 4;
	}
	
	void SaveOptionsPrefs()
	{
		PlayerPrefs.SetFloat(gamePrefsName+"_SFXVol", audioSFXSliderValue);
		PlayerPrefs.SetFloat(gamePrefsName+"_MusicVol", audioMusicSliderValue);
		PlayerPrefs.SetFloat(gamePrefsName+"_GraphicsDetail", graphicsSliderValue);
		
		// set the quality setting
		QualitySettings.SetQualityLevel( (int)graphicsSliderValue, true);
	}
	
	void ConfirmExitGame()
	{
		whichMenu=2;
	}
	
	void ExitGame()
	{
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
