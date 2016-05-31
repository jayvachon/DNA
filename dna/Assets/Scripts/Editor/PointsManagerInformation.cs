using UnityEngine;
using UnityEditor;
using System.Collections;
using DNA.Paths;

[CustomEditor (typeof (PointsManager))]
public class PointsManagerInformation : Editor {

	void OnSceneGUI () {
		
		Handles.Label (Vector3.zero, "PointsManager");
		PointsManager pm = target as PointsManager;

		for (int i = 0; i < pm.Points.Count; i ++) {
			string text = GridPointInformation.GetInfo (pm.Points[i], i);
			Handles.Label (pm.Points[i].transform.position, new GUIContent (text, new Texture ()));
		}
	}
}
