using UnityEngine;

public class PickupCoin_Plt2D : MonoBehaviour {

	// main event
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			string namePlayer = other.GetComponent<BaseUserManager>().GetName();
			int codePlayer = namePlayer.GetHashCode();

			// add bonus
			GameController_Plt2D.Instance.CoinsTaked(this.transform.position, 10, codePlayer);

			//destroy object
			Destroy(this.gameObject);
		}
	}
}
