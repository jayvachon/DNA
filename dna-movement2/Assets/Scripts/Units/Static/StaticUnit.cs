using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticUnit : Unit, IPathPoint {

	Vector3 position = ExtensionMethods.NullPosition;
	public Vector3 Position {
		get { 
			if (position == ExtensionMethods.NullPosition) {
				position = StartPosition;
			}
			return position; 
		}
	}

	protected List<Path> paths = new List<Path> ();
	public List<Path> Paths {
		get { return paths; }
		set { paths = value; }
	}

	public override void OnAwake () {
		base.OnAwake ();
		MyActionsList.Add (new DefaultAction (this));
	}

	protected void ChangePoint (IPathPoint newPoint) {
		foreach (Path p in paths) {
			p.ChangePoint (this, newPoint);
		}
	}

	public override void LeftClick () {
		if (!Enabled)
			return;
		if (SelectedUnit == null || Selected) {
			base.LeftClick ();
		} else {
			Path p = PathManager.instance.AddPoint (this);
			if (p != null)
				paths.Add (p);
		}
	}

	public override void RightClick () {
		if (!Enabled)
			return;
		if (SelectedUnit == null || Selected) {
			base.RightClick ();
		} else {
			Path p = PathManager.instance.RemovePoint (this);
			if (p != null)
				paths.Remove (p);
		}
	}
	
	protected override void OnNullClickEvent (NullClickEvent e) {}
}
