using UnityEngine;

public class PickupCoinManager : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			string namePlayer = other.GetComponent<BaseUserManager>().GetName();
			int codePlayer = namePlayer.GetHashCode();
			
			GameController.Instance.CoinsTake(this.transform.position, 10, codePlayer);
			
			Destroy(this.gameObject);
		}
	}
}
