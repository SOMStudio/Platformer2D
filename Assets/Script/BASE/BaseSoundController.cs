using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Base/Sound Controller")]

public class BaseSoundController : MonoBehaviour
{
	[SerializeField]
	private string gamePrefsName = "DefaultGame"; // DO NOT FORGET TO SET THIS IN THE EDITOR!!

	[SerializeField]
	protected AudioClip[] GameSounds;
	
	private int totalSounds;
	private List<SoundObject> soundObjectList;
	private SoundObject tempSoundObj;

	[SerializeField]
	[Range(0, 1)]
	private float volume = 1;

	[System.NonSerialized]
	public static BaseSoundController Instance;

	// main event
	void Awake()
	{
		// activate instance
		if (Instance == null) {
			Instance = this;

			if (soundObjectList == null) {
				Init ();
			}
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	void Start ()
	{
		if (soundObjectList == null) {
			Init ();
		}
	}

	// main logic
	void Init() {
		// keep this object alive
		DontDestroyOnLoad (this.gameObject);

		// we will grab the volume from PlayerPrefs when this script first starts
		string stKey = string.Format("{0}_SFXVol", gamePrefsName);
		if (PlayerPrefs.HasKey (stKey)) {
			volume = PlayerPrefs.GetFloat (stKey);
		} else {
			volume = 0.5f;
		}

		soundObjectList = new List<SoundObject> ();

		// make sound objects for all of the sounds in GameSounds array
		foreach(AudioClip theSound in GameSounds)
		{
			tempSoundObj= new SoundObject(theSound, theSound.name, volume);
			soundObjectList.Add(tempSoundObj);

			// keep this object alive
			DontDestroyOnLoad (tempSoundObj.sourceGO);

			totalSounds++;
		}
	}

	public float GetVolume() {
		return volume;
	}

	public void UpdateVolume() {
		if (soundObjectList == null) {
			Init ();
		}

		string stKey = string.Format("{0}_SFXVol", gamePrefsName);
		volume= PlayerPrefs.GetFloat(stKey);

		for (int i = 0; i < soundObjectList.Count; i++) {
			tempSoundObj = soundObjectList [i];
			tempSoundObj.source.volume = volume;
		}
	}

	public void PlaySoundByIndex(int anIndexNumber, Vector3 aPosition)
	{
		// make sure we're not trying to play a sound indexed higher than exists in the array
		if(anIndexNumber>soundObjectList.Count)
		{
			Debug.LogWarning("BaseSoundController>Trying to do PlaySoundByIndex with invalid index number. Playing last sound in array, instead.");
			anIndexNumber= soundObjectList.Count-1;
		}
		
		tempSoundObj = soundObjectList [anIndexNumber];
		tempSoundObj.PlaySound(aPosition);
	}
}

public class SoundObject
{
	public AudioSource source;
	public GameObject sourceGO;
	public Transform sourceTR;

	public AudioClip clip;
	public string name;

	public SoundObject(AudioClip aClip, string aName, float aVolume)
	{
		// in this (the constructor) we create a new audio source and store the details of the sound itself
		sourceGO= new GameObject("AudioSource_"+aName);
		sourceTR = sourceGO.transform;
		source = sourceGO.AddComponent<AudioSource> ();
		source.name = "AudioSource_" + aName;
		source.playOnAwake = false;
		source.clip = aClip;
		source.volume = aVolume;
		clip = aClip;
		name = aName;
	}

	public void PlaySound(Vector3 atPosition)
	{
		sourceTR.position= atPosition;
		source.PlayOneShot(clip);
	}
}
