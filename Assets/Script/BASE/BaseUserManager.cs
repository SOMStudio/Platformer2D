using UnityEngine;

[AddComponentMenu("Base/User Manager")]

public class BaseUserManager : MonoBehaviour
{
	protected int score;
	protected int highScore;
	protected int level;
	protected int health;

	protected bool isFinished;
	
	[SerializeField]
	protected string playerName ="Anonim";

	// main Logic
	public virtual void GetDefaultData()
	{
		playerName="Anonim";

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

	public void SetHighScore(int val)
	{
		highScore = val;
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

	//===============================

	public bool GetIsFinished()
	{
		return isFinished;
	}
		
	public void SetIsFinished(bool aVal)
	{
		isFinished=aVal;
	}
}