using UnityEngine;
using System.Collections;

public class TopDown_Camera : ExtendedCustomMonoBehaviour {

	[SerializeField]
	private Transform followTarget;
	[SerializeField]
	private Vector3 targetOffset;
	[SerializeField]
	private float moveSpeed= 2f;

	// main event
	void Update ()
	{
		if (followTarget) {
			if (moveSpeed == 0) {
				myTransform.position = followTarget.position + targetOffset;
			} else {
				if ((myTransform.position - (followTarget.position + targetOffset)).magnitude > 0.1f) {
					myTransform.position = Vector3.Lerp (myTransform.position, followTarget.position + targetOffset, moveSpeed * Time.deltaTime);
				}
			}
		}
	}

	// main logic
	public void SetTarget( Transform aTransform )
	{
		followTarget = aTransform;
	}

	public void SetPosition( Vector3 val )
	{
		myTransform.position = val;
	}

}
