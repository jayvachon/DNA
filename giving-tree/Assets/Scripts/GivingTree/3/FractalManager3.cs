using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FractalManager3 : MonoBehaviour {

	public Transform cameraAnchor;
	public GivingTree2 treePrefab;

	int maxIterations = 3;
	List<GivingTree2> visitableTrees = new List<GivingTree2> ();

	GivingTree2 FirstTree {
		get { return visitableTrees[0]; }
	}

	GivingTree2 NextTree {
		get { return visitableTrees[1]; }
	}

	GivingTree2 LastTree {
		get { return visitableTrees[visitableTrees.Count-1]; }
	}

	int treeCount = 0;
	bool iterating = false;
	float targetScale;

	void Awake () {
		targetScale = transform.localScale.x / 10;
		GivingTree2 targetTree = CreateTree (Vector3.zero, transform, true);
		PopulateTrees (targetTree);
		for (int i = 1; i < maxIterations; i ++) {
			targetTree = CreateTree (targetTree.TargetTree.position, targetTree.transform, true);
			PopulateTrees (targetTree);
		}
		cameraAnchor.SetPosition (LastTree.transform.position);
	}

	GivingTree2 CreateTree (Vector3 position, Transform parent, bool visitable, GivingTree2 tree=null) {
		
		if (tree == null) {
			tree = Instantiate (treePrefab) as GivingTree2;
		}

		Transform treeTransform = tree.transform;
		treeTransform.SetPosition (position);
		treeTransform.SetParent (parent);
		treeTransform.SetLocalScale (0.1f);
		
		if (visitable) {
			tree.name = string.Format ("Tree {0}", treeCount);
			treeCount ++;
			visitableTrees.Add (tree);
		}

		return tree;
	}

	void PopulateTrees (GivingTree2 onTree) {
		for (int i = 0; i < onTree.TreeSpawns.Length; i ++) {
			if (i == onTree.TargetTreeIndex) continue;
			Transform t = onTree.TreeSpawns[i];
			GivingTree2 newTree = Instantiate (treePrefab) as GivingTree2;
			Transform newTreeTransform = newTree.transform;
			newTreeTransform.SetPosition (t.position);
			newTreeTransform.SetParent (onTree.transform);
			newTreeTransform.SetLocalScale (0.1f);
		}
	}

	void Iterate () {
		if (iterating) return;
		iterating = true;
		StartCoroutine (CoIterate ());
	}

	IEnumerator CoIterate () {
		
		float time = 1f;
		float eTime = 0f;
		Vector3 startScale = new Vector3 (0.1f, 0.1f, 0.1f);
		Vector3 endScale = Vector3.one;
		Vector3 startPosition = FirstTree.transform.position;
		Vector3 endPosition = startPosition - (NextTree.transform.position / targetScale);

		while (eTime < time) {
			eTime += Time.deltaTime;
			float progress = Mathf.SmoothStep (0, 1, eTime / time);
			FirstTree.transform.localScale = Vector3.Lerp (
				startScale,
				endScale,
				progress
			);
			FirstTree.transform.localPosition = Vector3.Lerp (
				startPosition,
				endPosition,
				progress
			);
			yield return null;
		}

		iterating = false;
		OnEndIterate ();
	}

	void OnEndIterate () {
		GivingTree2 next = FirstTree;
		visitableTrees.Remove (next);
		FirstTree.transform.SetParent (transform);
		FirstTree.transform.SetLocalScale (0.1f);
		FirstTree.transform.SetLocalPosition (Vector3.zero);
		CreateTree (LastTree.TargetTree.position, LastTree.transform, true, next);
	}

	void DebugTrees () {
		for (int i = 0; i < visitableTrees.Count; i ++) {
			Debug.Log (visitableTrees[i].name);
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Iterate ();
		}
	}
}
