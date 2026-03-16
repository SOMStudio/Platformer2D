using UnityEngine;

public class PickupCoin_Plt2D : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			string namePlayer = other.GetComponent<BaseUserManager>().GetName();
			int codePlayer = namePlayer.GetHashCode();
			
			GameController_Plt2D.Instance.CoinsTake(this.transform.position, 10, codePlayer);
			
			Destroy(this.gameObject);
		}
	}
}
