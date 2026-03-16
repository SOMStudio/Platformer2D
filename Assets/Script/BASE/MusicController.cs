using UnityEngine;

public class MusicController : MonoBehaviour
{
	[SerializeField] private string gamePrefsName = "DefaultGame";

	[Range(0, 1)]
	public float volume;
	public AudioClip music;
	public bool loopMusic;

	private AudioSource source;
	private GameObject sourceGO;

	private int fadeState;
	private int targetFadeState;

	private float volumeON;
	private float targetVolume;

	public float fadeTime = 15f;
	public bool shouldFadeInAtStart = true;
	
	private void Start()
	{
		string stKey = $"{gamePrefsName}_MusicVol";
		if (PlayerPrefs.HasKey(stKey))
		{
			volumeON = PlayerPrefs.GetFloat(stKey);
		}
		else
		{
			volumeON = 0.1f;
		}
		
		sourceGO = new GameObject("Music_AudioSource");
		source = sourceGO.AddComponent<AudioSource>();
		source.name = "MusicAudioSource";
		source.playOnAwake = true;
		source.clip = music;
		source.volume = volume;
		DontDestroyOnLoad(sourceGO);
		
		if (shouldFadeInAtStart)
		{
			fadeState = 0;
			volume = 0;
		}
		else
		{
			fadeState = 1;
			volume = volumeON;
		}
		
		targetFadeState = 1;
		targetVolume = volumeON;
		source.volume = volume;
	}

	private void Update()
	{
		if (!source.isPlaying && loopMusic)
			source.Play();
		
		if (fadeState != targetFadeState)
		{
			if (targetFadeState == 1)
			{
				if (volume == volumeON)
					fadeState = 1;
			}
			else
			{
				if (volume == 0)
					fadeState = 0;
			}

			volume = Mathf.Lerp(volume, targetVolume, Time.deltaTime * fadeTime);
			source.volume = volume;
		}
	}
	
	public void UpdateVolume(float fadeAmount = 2f)
	{
		if (source)
		{
			volume = source.volume;
			fadeState = 0;
			targetFadeState = 1;
			volumeON = PlayerPrefs.GetFloat($"{gamePrefsName}_MusicVol");
			targetVolume = volumeON;
			fadeTime = fadeAmount;
		}
	}

	public void FadeIn(float fadeAmount)
	{
		volume = 0;
		fadeState = 0;
		targetFadeState = 1;
		targetVolume = volumeON;
		fadeTime = fadeAmount;
	}

	public void FadeOut(float fadeAmount)
	{
		volume = volumeON;
		fadeState = 1;
		targetFadeState = 0;
		targetVolume = 0;
		fadeTime = fadeAmount;
	}

	public bool IsPlaying()
	{
		return source.isPlaying;
	}
}
