using SOMStudio.Platformer2D.Scripts.Game;
using UnityEditor;
using UnityEngine;

namespace SOMStudio.Platformer2D.Scripts.Editor
{
	[CustomEditor(typeof(GameController))]
	public class ButtonForEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			GameController spectate = (GameController)target;

			GUILayout.Space(10);
			GUILayout.Label("Help Buttons", EditorStyles.boldLabel);
		
			if (GUILayout.Button("Set Time Scale = 1"))
			{
				Time.timeScale = 1f;
			}

			if (GUILayout.Button("Set Time Scale = 0.75"))
			{
				Time.timeScale = 0.75f;
			}

			if (GUILayout.Button("Set Time Scale = 0.5"))
			{
				Time.timeScale = 0.5f;
			}

			if (GUILayout.Button("Set Time Scale = 0"))
			{
				Time.timeScale = 0f;
			}
		}
	}
}
