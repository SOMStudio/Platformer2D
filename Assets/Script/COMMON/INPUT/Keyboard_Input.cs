using UnityEngine;

[AddComponentMenu("Common/Keyboard Input Controller")]
public class Keyboard_Input : BaseInputController
{
	private void LateUpdate()
	{
		CheckInput();
	}

	protected override void CheckInput()
	{
		base.CheckInput();
		
		Fire1 = Input.GetButton("Fire1");
		shouldRespawn = Input.GetButton("Fire3");
	}
}
