using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;

public class Road : MBRefs, IPoolable {

	Transform roadRender;
	Transform RoadRender {
		get {
			if (roadRender == null) {
				roadRender = MyTransform.GetChild (0);
			}
			return roadRender;
		}
	}

	Renderer roadRenderer = null;
	Renderer RoadRenderer {
		get {
			if (roadRenderer == null) {
				roadRenderer = RoadRender.GetComponent<Renderer> ();
			}
			return roadRenderer;
		}
	}

	bool CanHighlight {
		get { return (!built && SelectionManager.NoneSelected); }
	}

	bool built = false;

	protected override void Awake () {
		base.Awake ();
		SetVisible (false);	
	}

	public void SetPoints (Vector3 a, Vector3 b) {
		Position = a;
		MyTransform.LookAt (b);
		float distance = Vector3.Distance (a, b);
		RoadRender.SetLocalScaleZ (distance);
		RoadRender.SetLocalPositionZ (distance*0.5f);

		built = true;
		SetVisible (true);
	}

	public void OnHoverEnter () {
		if (CanHighlight)
			SetVisible (true);
	}

	public void OnHoverExit () {
		if (CanHighlight)
			SetVisible (false);
	}

	public void OnClick () {
		if (CanHighlight && Player.Instance.Milkshakes.Count >= 5) {
			Player.Instance.Milkshakes.Remove (5);
			built = true;
			SetVisible (true);
		}
	}

	void SetVisible (bool enabled) {
		RoadRenderer.enabled = enabled;	
	}

	public void OnPoolCreate () {}
	public void OnPoolDestroy () {}
}
