using UnityEngine;
using System.Collections;

public class Oil : MonoBehaviour {

	MeshFilter mf;
	MeshRenderer mr;

	private Transform myTransform;

	private void Awake () {
		myTransform = transform;
		Init ();
	}
	
	public void Init () {
		mf = gameObject.AddComponent<MeshFilter>();
		mr = gameObject.AddComponent<MeshRenderer>();
		mf.mesh = CustomMesh.Oil ();
		SetMaterial(Color.red);
		myTransform.localScale = Structure.v3scale * 0.1f;
		Deactivate ();
	}
	
	private void SetMaterial (Color color) {
		mr.SetColor(color);
	}
	
	private void Deactivate () {
		gameObject.SetActive (false);
	}
}
