using UnityEngine;
using System.Collections;

public class FloatingIndicator : MBRefs {

	[SerializeField] float spinSpeed = 15f;
	public Transform mercury;
	public MercuryRender mercuryRender;
	bool spinning = false;

	public float Fill {
		set { mercury.SetLocalScaleY (value); }
	}

	float floatHeight = 1f;

	public void Initialize (Transform parent, float height=-1) {
		floatHeight = (height == -1) ? floatHeight : height;
		Parent = parent;
		LocalPosition = new Vector3 (0, floatHeight, 0);
		StartCoroutine (CoSpin ());
	}

	public virtual void OnEnable () {
		StartSpinning ();
	}

	public virtual void OnDisable () {
		spinning = false;
	}

	protected void SetColor (Color color) {
		mercuryRender.SetColor (color);
	}

	protected void StartSpinning () {
		if (spinning) return;
		spinning = true;
		StartCoroutine (CoSpin ());
	}

	IEnumerator CoSpin () {
		
		float a = 0f;

		while (gameObject.activeSelf) {
			MyTransform.SetLocalEulerAnglesY (a);
			a += spinSpeed * Time.deltaTime;
			yield return null;
		}
	}
}
