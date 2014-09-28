using UnityEngine;
using System.Collections;

public class Step : MonoBehaviour {

	MeshFilter mf;
	MeshRenderer mr;

	private Transform myTransform;

	private void Awake () {
		myTransform = transform;
	}

	public void Init (float width) {
		mf = gameObject.AddComponent<MeshFilter>();
		mr = gameObject.AddComponent<MeshRenderer>();
		mf.mesh = StepMesh.CreateMesh (width);
		SetMaterial(Color.white);
		myTransform.localScale = Structure.v3scale;
		Deactivate ();
	}

	private void SetMaterial (Color color) {
		mr.SetColor(color);
	}

	private void Deactivate () {
		gameObject.SetActive (false);
	}
}
