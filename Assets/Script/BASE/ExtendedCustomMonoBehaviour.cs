using UnityEngine;

public class ExtendedCustomMonoBehaviour : MonoBehaviour
{
	[Header("Base")]
	[SerializeField] protected bool didInit;
	[SerializeField] protected bool canControl;

	protected int id;

	protected Transform myTransform;
	protected GameObject myGO;
	protected Rigidbody myBody;

	private void Start()
	{
		Init();
	}
	
	public virtual void Init()
	{
		// cache refs to our transform and gameObject
		if (!myTransform)
		{
			myTransform = transform;
		}

		if (!myGO)
		{
			myGO = gameObject;
		}

		if (!myBody)
		{
			myBody = GetComponent<Rigidbody>();
		}

		didInit = true;
	}

	public virtual void SetID(int anID)
	{
		id = anID;
	}
}
