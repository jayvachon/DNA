using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {

	static UI instance = null;
	static public UI Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (UI)) as UI;
			}
			return instance;
		}
	}

	public ConstructPrompt ConstructPrompt;
	List<ProgressBar> progressBars = new List<ProgressBar> ();

	public ProgressBar CreateProgressBar (Vector3 position) {
		return CreateProgressBar (transform, position);
	}

	public ProgressBar CreateProgressBar (Transform parent, Vector3 offset) {
		ProgressBar pbar = ObjectPool.Instantiate<ProgressBar> () as ProgressBar;
		pbar.transform.SetParent (parent);
		pbar.transform.localPosition = new Vector3 (
			offset.x,
			offset.y + 2f,
			offset.z
		);
		progressBars.Add (pbar);
		return pbar;
	}

	public void DestroyProgressBar (ProgressBar pbar) {
		if (pbar != null)
			ObjectPool.Destroy<ProgressBar> (pbar);
	}
}