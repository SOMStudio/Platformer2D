using UnityEngine;

public class ExtendedCustomMonoBehaviour : MonoBehaviour
{
	[Header("Base")]
	[SerializeField] protected bool didInit;
	[SerializeField] protected bool canControl;

	protected int id;

	protected Transform myTransform;
	protected GameObject myGameObject;
	protected Rigidbody myBody;

	private void Start()
	{
		Init();
	}

	protected virtual void Init()
	{
		myTransform = transform;

		myGameObject = gameObject;

		myBody = GetComponent<Rigidbody>();

		didInit = true;
	}

	public virtual void SetID(int newId)
	{
		id = newId;
	}
}
