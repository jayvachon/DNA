using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiralGenerator : MonoBehaviour {

	LineRenderer lineRenderer = null;
	LineRenderer LineRenderer {
		get {
			if (lineRenderer == null) {
				lineRenderer = GetComponent<LineRenderer> ();
			}
			return lineRenderer;
		}
	}

	public Transform floret;
	public Transform pivot;

	int floretCount = 250;
	Vector3 center = Vector3.zero;

	List<Transform> florets = new List<Transform> ();
	float time = 0f;
	float speed = 0.1f;
	float height = 24f;

	void Awake () {
		
		float minScale = 1f;
		float maxScale = 5f;
		float goldenRatio = 137.508f;
		for (int i = 0; i < floretCount; i ++) {
			float r = (float)i * goldenRatio * Mathf.Deg2Rad;
			float radius = 1f * Mathf.Sqrt (r);
			Vector3 position = new Vector3 (
				center.x + radius * Mathf.Sin (r),
				center.y,
				center.z + radius * Mathf.Cos (r)
			);
			Vector3 relativePosition = position - center;
			Transform t = (Transform)Instantiate (floret, position, Quaternion.LookRotation (relativePosition));
			t.SetParent (pivot);

			float p = (float)i / (float)floretCount;
			t.SetLocalScale (Mathf.Lerp (minScale, maxScale, p));

			float h = p + 0.5f;
			if (h > 1f) h -= 1f;
			t.renderer.SetColor (new HSBColor (h, 1f, 1f).ToColor ());
			florets.Add (t);
		}
	}

	void Update () {
		time += Time.deltaTime * speed;
		for (int i = 0; i < florets.Count; i ++) {
			Transform t = florets[i];
			float offset = (float)i / (float)florets.Count;
			float y = height * Mathf.Sin ((time + offset) * Mathf.PI * 2f);
			t.SetLocalPositionY (y);
		}
		UpdateLine ();

		if (Input.GetKey (KeyCode.Q)) {
			if (speed < 10f)
				speed += 0.01f;
		}
		if (Input.GetKey (KeyCode.W)) {
			if (speed > 0f)
				speed -= 0.01f;
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			if (height < 30f)
				height += 1f;
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			if (height > 0f)
				height -= 1f;
		}
	}

	void UpdateLine () {
		List<Vector3> positions = new List<Vector3> ();
		foreach (Transform t in florets) {
			Vector3 position = t.position;
			//position.y -= 30f;
			positions.Add (position);
		}
		LineRenderer.SetVertexPositions (positions);
	}
}
