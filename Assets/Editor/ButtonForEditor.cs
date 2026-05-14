using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameController))]
public class ButtonForEditor : Editor
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
