using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GizmosDrawer : MonoBehaviour {

	static GizmosDrawer instance = null;
	static public GizmosDrawer Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (GizmosDrawer)) as GizmosDrawer;
				if (instance == null) {
					GameObject go = new GameObject ("GizmosDrawer");
					DontDestroyOnLoad (go);
					instance = go.AddComponent<GizmosDrawer>();
				}
			}
			return instance;
		}
	}

	List<GizmoShape> shapes = new List<GizmoShape> ();

	public void Add (GizmoShape shape) {
		shapes.Add (shape);
	}

	public void Clear () {
		shapes.Clear ();
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.black;
		foreach (GizmoShape shape in shapes) {
			shape.Draw ();
		}
	}
}

public abstract class GizmoShape {
	public abstract void Draw ();
}

public class GizmoSphere : GizmoShape {

	Vector3 position;
	float radius;

	public GizmoSphere (Vector3 position, float radius) {
		this.position = position;
		this.radius = radius;
	}

	public override void Draw () {
		Gizmos.DrawSphere (position, radius);
	}
}

public class GizmoLine : GizmoShape {

	Vector3 from;
	Vector3 to;

	public GizmoLine (Vector3 from, Vector3 to) {
		this.from = from;
		this.to = to;
	}

	public override void Draw () {
		Gizmos.DrawLine (from, to);
	}
}

public class GizmoRay : GizmoShape {

	Vector3 from;
	Vector3 direction;

	public GizmoRay (Vector3 from, Vector3 direction) {
		this.from = from;
		this.direction = direction;
	}

	public override void Draw () {
		Gizmos.DrawRay (from, direction);
	}
}