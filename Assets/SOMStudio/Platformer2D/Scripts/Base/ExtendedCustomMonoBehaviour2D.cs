using UnityEngine;

namespace SOMStudio.Platformer2D.Scripts.Base
{
	public class ExtendedCustomMonoBehaviour2D : MonoBehaviour
	{
		[Header("Base")] [SerializeField] protected bool didInit;
		[SerializeField] protected bool canControl;

		protected int id;

		protected Transform myTransform;
		protected GameObject myGameObject;
		protected Rigidbody2D myBody;

		private void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			myTransform = transform;

			myGameObject = gameObject;

			myBody = GetComponent<Rigidbody2D>();

			didInit = true;
		}

		public virtual void SetID(int newId)
		{
			id = newId;
		}
	}
}
