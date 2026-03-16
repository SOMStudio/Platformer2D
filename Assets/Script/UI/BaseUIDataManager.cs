using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("Base/UI Data Manager")]
public class BaseUIDataManager : MonoBehaviour
{
	[SerializeField] protected string gamePrefsName = "DefaultGame";
	
	[Header("Base settings")]
	[SerializeField] protected int playerScore;
	[SerializeField] protected int playerLives; 
	[SerializeField] protected int playerHighScore;
	
	public void UpdateScoreP1(int aScore)
	{
		playerScore = aScore;
		if (playerScore > playerHighScore)
			playerHighScore = playerScore;
	}

	public void UpdateLivesP1(int lifeNum)
	{
		playerLives = lifeNum;
	}

	public void UpdateScore(int aScore)
	{
		playerScore = aScore;
	}

	public void UpdateLives(int lifeNum)
	{
		playerLives = lifeNum;
	}

	protected void LoadHighScore()
	{
		if (PlayerPrefs.HasKey(gamePrefsName + "_highScore"))
		{
			playerHighScore = PlayerPrefs.GetInt(gamePrefsName + "_highScore");
		}
	}

	protected void SaveHighScore()
	{
		PlayerPrefs.SetInt(gamePrefsName + "_highScore", playerHighScore);
	}
}
