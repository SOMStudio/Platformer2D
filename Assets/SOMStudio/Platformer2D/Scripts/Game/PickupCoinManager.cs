using SOMStudio.Platformer2D.Scripts.Base;
using UnityEngine;

namespace SOMStudio.Platformer2D.Scripts.Game
{
	public class PickupCoinManager : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
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
}
