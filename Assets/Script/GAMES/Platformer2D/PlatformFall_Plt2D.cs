using UnityEngine;

public class PlatformFall_Plt2D : ExtendedCustomMonoBehaviour2D
{
	[SerializeField] private float fallDelay = 1.0f;
	[SerializeField] private float killDelay = 4.0f;

	[SerializeField] private SpawnCoins_Plt2D coinSpawner;

	private GameController_Plt2D gameController;
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!gameController)
				Init();

			// remove from list
			gameController.PlatformDrop(this.gameObject);

			// set kinematic and kill after time
			Invoke(nameof(Fall), fallDelay);
			Invoke(nameof(Kill), killDelay);
		}
	}
	
	public override void Init()
	{
		base.Init();

		if (!coinSpawner)
			coinSpawner = myGO.GetComponent<SpawnCoins_Plt2D>();

		if (!gameController)
			gameController = GameController_Plt2D.Instance;
	}

	public void Fall()
	{
		myBody.isKinematic = false;
	}

	public void Kill()
	{
		// destroy coins
		coinSpawner.Kill();

		// destroy platform
		Destroy(gameObject);
	}

	public void FallWithDelay()
	{
		Invoke(nameof(Fall), fallDelay);
	}

	public void KillWithDelay()
	{
		Invoke(nameof(Kill), killDelay);
	}
}
