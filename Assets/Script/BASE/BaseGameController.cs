using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("Base/GameController")]

public class BaseGameController : MonoBehaviour
{
	//[Header("Name")] //text head
	//[Space()]  //set one space default or set count space
	//[Tooltip("help text")] public string varStr; //help text when in editor, when cursor over name var
	//[Range(0, 1)] public int varIntOrFl; //slider int and float value
	//[TextArea()] public string varStrBig; //tree-line area for edit string def or set size in parrametr
	//[Multiline()] public string varStrMlt; //tree-line area for edit multilane string or set size parrametr
	//[SerializeField] private int varIntPrivate; //Show hiden parametr in editor (and rule for edit parrametr)
	//[HideInInspector] public int varIntPublic; //Hide in editor the public parrametr

	bool paused;

	[SerializeField]
	protected GameObject explosionPrefab;

	// main logic
	public virtual void PlayerLostLife ()
	{
		// deal with player life lost (update U.I. etc.)
	}
	
	public virtual void SpawnPlayer ()
	{
		// the player needs to be spawned
	}
	
	public virtual void Respawn ()
	{
		// the player is respawning
	}
	
	public virtual void StartGame()
	{
		// do start game functions
	}

	public void Explode ( Vector3 aPosition )
	{		
		// instantiate an explosion at the position passed into this function
		Instantiate( explosionPrefab, aPosition, Quaternion.identity );
	}
	
	public virtual void EnemyDestroyed( Vector3 aPosition, int pointsValue, int hitByID )
	{
		// deal with a enemy destroyed
	}
	
	public virtual void BossDestroyed()
	{
		// deal with the end of a boss battle
	}
	
	public virtual void RestartGameButtonPressed()
	{
		// deal with restart button (default behaviour re-loads the currently loaded scene)
		//Application.LoadLevel(Application.loadedLevelName);
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

    public bool Paused
    {
        get 
        { 
            // get paused
            return paused; 
        }
        set
        {
            // set paused 
            paused = value;

			if (paused)
			{
                // pause time
                Time.timeScale = 0f;
			} else {
                // unpause Unity
				Time.timeScale = 1f;
            }
        }
    }
	
}
