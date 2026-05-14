using UnityEngine;

public class MusicObject : MonoBehaviour
{
	[SerializeField] private string gamePrefsName = "DefaultGame";

	[Range(0, 1)]
	public float volume;
	public AudioClip music;
	public bool loopMusic;

	private AudioSource source;
	private GameObject sourceGameObject;

	private int fadeState;
	private int targetFadeState;

	private float volumeOn;
	private float targetVolume;

	public float fadeTime = 15f;
	public bool shouldFadeInAtStart = true;
	
	private void Start()
	{
		string stKey = $"{gamePrefsName}_MusicVol";
		volumeOn = PlayerPrefs.HasKey(stKey) ? PlayerPrefs.GetFloat(stKey) : 0.1f;
		
		sourceGameObject = new GameObject("Music_AudioSource");
		source = sourceGameObject.AddComponent<AudioSource>();
		source.name = "MusicAudioSource";
		source.playOnAwake = true;
		source.clip = music;
		source.volume = volume;
		DontDestroyOnLoad(sourceGameObject);
		
		if (shouldFadeInAtStart)
		{
			fadeState = 0;
			volume = 0;
		}
		else
		{
			fadeState = 1;
			volume = volumeOn;
		}
		
		targetFadeState = 1;
		targetVolume = volumeOn;
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
				if (Mathf.Approximately(volume, volumeOn))
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
			volumeOn = PlayerPrefs.GetFloat($"{gamePrefsName}_MusicVol");
			targetVolume = volumeOn;
			fadeTime = fadeAmount;
		}
	}

	public void FadeIn(float fadeAmount)
	{
		volume = 0;
		fadeState = 0;
		targetFadeState = 1;
		targetVolume = volumeOn;
		fadeTime = fadeAmount;
	}

	public void FadeOut(float fadeAmount)
	{
		volume = volumeOn;
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
