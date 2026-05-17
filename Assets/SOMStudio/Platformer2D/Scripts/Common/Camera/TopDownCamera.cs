using SOMStudio.Platformer2D.Scripts.Base;
using UnityEngine;

namespace SOMStudio.Platformer2D.Scripts.Common.Camera
{
	public class TopDownCamera : ExtendedCustomMonoBehaviour
	{
		[SerializeField] private Transform followTarget;
		[SerializeField] private Vector3 targetOffset;
		[SerializeField] private float moveSpeed = 2f;
	
		private void Update()
		{
			if (followTarget)
			{
				if (moveSpeed == 0)
				{
					myTransform.position = followTarget.position + targetOffset;
				}
				else
				{
					if ((myTransform.position - (followTarget.position + targetOffset)).magnitude > 0.1f)
					{
						myTransform.position = Vector3.Lerp(myTransform.position, followTarget.position + targetOffset,
							moveSpeed * Time.deltaTime);
					}
				}
			}
		}
	
		public void SetTarget(Transform setTransform)
		{
			followTarget = setTransform;
		}

		public void SetPosition(Vector3 position)
		{
			myTransform.position = position;
		}
	}
}
