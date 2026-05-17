using SOMStudio.Platformer2D.Scripts.Base;
using UnityEngine;

namespace SOMStudio.Platformer2D.Scripts.Common.Input
{
	[AddComponentMenu("SOMStudio/Platformer2D/Keyboard Input Controller")]
	public class KeyboardInput : BaseInputController
	{
		private void LateUpdate()
		{
			CheckInput();
		}

		protected override void CheckInput()
		{
			base.CheckInput();
		
			fire1 = UnityEngine.Input.GetButton("Fire1");
			shouldRespawn = UnityEngine.Input.GetButton("Fire3");
		}
	}
}
