using UnityEngine;
using System.Collections;

public class TopDown_Camera1 : MonoBehaviour {

	public Transform followTarget;
	public Vector3 targetOffset;
	public float moveSpeed= 2f;

	private Transform myTransform;
	
	void Start ()
	{
		myTransform = transform;
	}
	
	public void SetTarget( Transform aTransform )
	{
		followTarget = aTransform;
	}

	public void SetPosition( Vector3 val )
	{
		myTransform.position = val;
	}

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
}
