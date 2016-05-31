using UnityEngine;
using UnityEditor;
using System.Collections;
using DNA.Paths;
using DNA.Units;

[CustomEditor (typeof (PointContainer))]
public class GridPointInformation : Editor {

	void OnSceneGUI () {
		string text = GetInfo (target as PointContainer);
		Handles.Label (((PointContainer)target).transform.position, new GUIContent (text, new Texture ()));
	}

	public static string GetInfo (PointContainer container, int index=-1) {

		GridPoint point = container.Point;

		string text = "";
		if (index > -1)
			text += "index: " + index + "\n";

		text += "Object: " + point.Object
		+ "\nConnection count: " + point.Connections.Count
		+ "\nHas road: " + point.HasRoad
		+ "\nHas road construction: " + point.HasRoadConstruction
		+ "\nHas fog: " + point.HasFog;

		IWorkplace workplace = point.Object as IWorkplace;
		if (workplace != null)
			text += "\nAccessible: " + workplace.Accessible
			+ "\nEfficiency: " + workplace.Efficiency;

		return text;
	}
}

[CustomEditor (typeof (GameObject))]
public class GridPointChildInformation : Editor {

	void OnSceneGUI () {
		PointContainer container = (target as GameObject).transform.GetParentOfType<PointContainer> ();
		if (container != null) {
			string text = GridPointInformation.GetInfo (container);
			Handles.Label (container.transform.position, new GUIContent (text, new Texture ()));
		}
	}
}