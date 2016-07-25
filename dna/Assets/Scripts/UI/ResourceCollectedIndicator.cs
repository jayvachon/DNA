using UnityEngine;
using System.Collections;
using DNA.Units;

public class ResourceCollectedIndicator : MBRefs {

	UnitRenderer visual;

	public static ResourceCollectedIndicator Init (string id, Transform parent) {

		ResourceCollectedIndicator r = ObjectPool.Instantiate<ResourceCollectedIndicator> ();
		r.Parent = parent;
		r.MyTransform.Reset ();

		string renderer = UnitRenderer.GetRenderer (id);
		r.visual = ObjectPool.Instantiate (renderer) as UnitRenderer;
		r.visual.transform.parent = r.MyTransform;
		r.visual.transform.localPosition = Vector3.zero;
		r.visual.transform.localRotation = Quaternion.identity;
		r.visual.SetScale (0.5f);

		return r;
	}

	public void OnEnable () {
		
		float startY = 0f;
		float endY = 3f;

		Co2.StartCoroutine (1.5f, (float p) => {
			MyTransform.SetLocalPositionY (Mathf.Lerp (startY, endY, Mathf.SmoothStep (0f, 1f, p)));
		}, () => {
			ObjectPool.Destroy<ResourceCollectedIndicator> (MyTransform);
		});
	}

	public void OnDisable () {
		ObjectPool.Destroy (visual);
	}
}
