using UnityEngine;

[AddComponentMenu("Common/Keyboard Input Controller")]
public class Keyboard_Input : BaseInputController
{
	private void LateUpdate()
	{
		// check inputs each LateUpdate() ready for the next tick
		CheckInput();
	}

	protected override void CheckInput()
	{
		// get input data from vertical and horizontal axis
		base.CheckInput();

		// get fire / action buttons
		Fire1 = Input.GetButton("Fire1");
		shouldRespawn = Input.GetButton("Fire3");
	}
}
