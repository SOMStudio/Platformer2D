using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameController_Plt2D))]
public class ButtonForEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

        GameController_Plt2D spc = (GameController_Plt2D)target;

		if (GUILayout.Button ("Set Time Scale = 1")) {
			Time.timeScale = 1f;
		}

		if (GUILayout.Button ("Set Time Scale = 0.75")) {
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
