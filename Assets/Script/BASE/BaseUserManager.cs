using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[AddComponentMenu("Base/User Manager")]

[SerializeField]
class PlayerData
{
	public int helth;
	public int experience;
}

public class BaseUserManager : MonoBehaviour
{
	// gameplay specific data
	// we keep these private and provide methods to modify them instead, just to prevent any
	// accidental corruption or invalid data coming in
	private int score;
	private int highScore;
	private int level;
	private int health;
	private bool isFinished;
	
	// this is the display name of the player
	public string playerName ="Anon";
		
	public virtual void GetDefaultData()
	{
		playerName="Anon";
		score=0;
		level=1;
		health=3;
		highScore=0;
		isFinished=false;
	}
	
	public string GetName()
	{
		return playerName;
	}
	
	public void SetName(string aName)
	{
		playerName=aName;
	}
	
	public int GetLevel()
	{
		return level;
	}
	
	public void SetLevel(int num)
	{
		level=num;
	}
	
	public int GetHighScore()
	{
		return highScore;
	}
		
	public int GetScore()
	{
		return score;	
	}
	
	public virtual void AddScore(int anAmount)
	{
		score+=anAmount;
	}
		
	public void LostScore(int num)
	{
		score-=num;
	}
	
	public void SetScore(int num)
	{
		score=num;
	}
	
	public int GetHealth()
	{
		return health;
	}
	
	public void AddHealth(int num)
	{
		health+=num;
	}

	public void ReduceHealth(int num)
	{
		health-=num;
	}
		
	public void SetHealth(int num)
	{
		health=num;
	}
	
	public bool GetIsFinished()
	{
		return isFinished;
	}
		
	public void SetIsFinished(bool aVal)
	{
		isFinished=aVal;
	}

	//=for save data=====================

	/// <summary>
	/// save player data in file with cripting, not use for Web-application (we can't write file)
	/// </summary>
	public void SavePrivateDataPlayer()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerinfo.dat");
		
		PlayerData data = new PlayerData();
		data.helth = health;
		data.experience = score;
		
		bf.Serialize(file, data);
		file.Close();
	}
	
	/// <summary>
	/// restore player data from cripting file.
	/// </summary>
	public void LoadPrivateDataPlayer()
	{
		if (File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
			
			PlayerData data = (PlayerData)bf.Deserialize(file);
			health = data.helth;
			score = data.experience;
			
			file.Close();
		}
	}
}