using UnityEngine;

[AddComponentMenu("Base/UI Data Manager")]
public class BaseUIDataManager : MonoBehaviour
{
	[SerializeField] protected string gamePrefsName = "DefaultGame";

	[Header("Base settings")] [SerializeField]
	protected int player_score;

	[SerializeField] protected int player_lives;
	[SerializeField] protected int player_highscore;
	
	public void UpdateScoreP1(int aScore)
	{
		player_score = aScore;
		if (player_score > player_highscore)
			player_highscore = player_score;
	}

	public void UpdateLivesP1(int lifeNum)
	{
		player_lives = lifeNum;
	}

	public void UpdateScore(int aScore)
	{
		player_score = aScore;
	}

	public void UpdateLives(int lifeNum)
	{
		player_lives = lifeNum;
	}

	public void LoadHighScore()
	{
		// grab high score from prefs
		if (PlayerPrefs.HasKey(gamePrefsName + "_highScore"))
		{
			player_highscore = PlayerPrefs.GetInt(gamePrefsName + "_highScore");
		}
	}

	public void SaveHighScore()
	{
		// as we know that the game is over, let's save out the high score too
		PlayerPrefs.SetInt(gamePrefsName + "_highScore", player_highscore);
	}
}
