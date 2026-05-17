using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("SOMStudio/Platformer2D/Sound Controller")]
public class BaseSoundController : MonoBehaviour
{
	[SerializeField] private string gamePrefsName = "DefaultGame";

	[SerializeField] protected AudioClip[] gameSounds;

	private int totalSounds;
	private List<SoundObject> soundObjectList;
	private SoundObject tempSoundObject;

	[SerializeField] [Range(0, 1)] private float volume = 1;

	[System.NonSerialized] public static BaseSoundController Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			if (soundObjectList == null)
			{
				Init();
			}
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		if (soundObjectList == null)
		{
			Init();
		}
	}
	
	private void Init()
	{
		DontDestroyOnLoad(gameObject);
		
		string stringKey = $"{gamePrefsName}_SFXVol";
		if (PlayerPrefs.HasKey(stringKey))
		{
			volume = PlayerPrefs.GetFloat(stringKey);
		}
		else
		{
			volume = 0.5f;
		}

		soundObjectList = new List<SoundObject>();
		
		foreach (AudioClip theSound in gameSounds)
		{
			tempSoundObject = new SoundObject(theSound, theSound.name, volume);
			soundObjectList.Add(tempSoundObject);
			
			DontDestroyOnLoad(tempSoundObject.sourceGameObject);

			totalSounds++;
		}
	}

	public float GetVolume()
	{
		return volume;
	}

	public void UpdateVolume()
	{
		if (soundObjectList == null)
		{
			Init();
		}

		string stringKey = $"{gamePrefsName}_SFXVol";
		volume = PlayerPrefs.GetFloat(stringKey);

		for (int i = 0; i < soundObjectList.Count; i++)
		{
			tempSoundObject = soundObjectList[i];
			tempSoundObject.source.volume = volume;
		}
	}

	public void PlaySoundByIndex(int indexNumber, Vector3 position)
	{
		if (indexNumber > soundObjectList.Count)
		{
			Debug.LogWarning(
				"BaseSoundController>Trying to do PlaySoundByIndex with invalid index number. Playing last sound in array, instead.");
			indexNumber = soundObjectList.Count - 1;
		}

		tempSoundObject = soundObjectList[indexNumber];
		tempSoundObject.PlaySound(position);
	}
}

public class SoundObject
{
	public AudioSource source;
	public GameObject sourceGameObject;
	public Transform sourceTransform;

	public AudioClip clip;
	public string name;

	public SoundObject(AudioClip clip, string name, float volume)
	{
		sourceGameObject = new GameObject("AudioSource_" + name);
		sourceTransform = sourceGameObject.transform;
		source = sourceGameObject.AddComponent<AudioSource>();
		source.name = "AudioSource_" + name;
		source.playOnAwake = false;
		source.clip = clip;
		source.volume = volume;
		this.clip = clip;
		this.name = name;
	}

	public void PlaySound(Vector3 atPosition)
	{
		sourceTransform.position = atPosition;
		source.PlayOneShot(clip);
	}
}
