using UnityEngine;
using System.Collections.Generic;

public class BaseMusicController : MonoBehaviour
{
	[SerializeField] protected List<MusicObject> musicList;

	[System.NonSerialized] public static BaseMusicController Instance;
	
	private void Awake()
	{
		Init();
	}

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void Init()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void UpdateVolume()
	{
		foreach (MusicObject item in musicList)
		{
			item.UpdateVolume();
		}
	}

	public void StopMusic(int number)
	{
		MusicObject temp = musicList[number];

		if (temp)
		{
			temp.loopMusic = false;
			temp.FadeOut(15f);
		}
	}

	public void StopMusicButPlayToEnd(int number)
	{
		MusicObject temp = musicList[number];

		if (temp)
		{
			temp.loopMusic = false;
		}
	}

	public void PlayMusic(int number)
	{
		MusicObject temp = musicList[number];

		if (temp)
		{
			temp.loopMusic = true;
			temp.FadeIn(15f);
		}
	}

	public void PlayRememberMusic()
	{
		MusicObject temp = musicList[0];

		if (temp.IsPlaying() == true)
		{
			StopMusic(0);
		}

		PlayMusic(1);
	}

	public void PlayLevelMusic()
	{
		MusicObject temp = musicList[1];

		if (temp.IsPlaying() == true)
		{
			StopMusic(1);
		}

		PlayMusic(0);
	}
}
