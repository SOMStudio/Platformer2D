using UnityEngine;
using UnityEngine.Events;

public class PlatformFall_Plt2D : MonoBehaviour {

	public float fallDelay = 1.0f;
	public float killDelay = 3.0f;

	private Rigidbody2D rb2d;

	private GameController_Plt2D gameController;

	void Awake()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Start() {
		Init ();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			if (!gameController)
				Init ();
			
			// remove from list
			gameController.PlatformDrop (this.gameObject);

			// set kinematic and kill after time
			Invoke("Fall", fallDelay);
			Invoke("KillPlatform", killDelay);
		}
	}

	// init
	private void Init() {
		if (!gameController)
			gameController = GameController_Plt2D.Instance;
	}

	void Fall()
	{
		rb2d.isKinematic = false;
	}

	void KillPlatform()
	{
		Destroy (this.gameObject);
	}
}
