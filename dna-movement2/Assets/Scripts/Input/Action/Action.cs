using UnityEngine;
using System.Collections;

public class Action {

	public readonly IPathPoint point;
	public readonly string name;
	public readonly float time;

	public Action (IPathPoint point, string name, float time) {
		this.name = name;
		this.time = time;
	}

	public virtual bool CanSetActive () {
		return true;
	}
	
	public virtual void OnStartAction () {}
	public virtual void PerformAction (float progress, IActionable visitor) {}
	public virtual void OnEndAction () {}
}
