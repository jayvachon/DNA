﻿using UnityEngine;
using System.Collections;

public class ObjectCreator : MonoBehaviour {

	static ObjectCreator instanceInternal = null;
	static public ObjectCreator Instance {
		get {
			if (instanceInternal == null) {
				instanceInternal = Object.FindObjectOfType (typeof (ObjectCreator)) as ObjectCreator;
				if (instanceInternal == null) {
					GameObject go = new GameObject ("ObjectCreator");
					DontDestroyOnLoad (go);
					instanceInternal = go.AddComponent<ObjectCreator>();
				}
			}
			return instanceInternal;
		}
	}

	public Transform Create<T> () where T : class {
		return Create<T> (Vector3.zero);
	}

	public Transform Create<T> (Vector3 position) where T : class {
		string name = typeof (T).Name;
		name = name.Substring (0, name.Length);
		if (ObjectPool.GetPool (name) == null) {
			CreatePool<T> ();
		}
		return ObjectPool.Instantiate (name, position);
	}

	void CreatePool<T> () where T : class {
		string prefabName = typeof (T).Name;
		GameObject go = new GameObject (prefabName);
		DontDestroyOnLoad (go);
		go.AddComponent<ObjectPool> ().Init (prefabName, ObjectBank.Instance.GetObject (prefabName).transform);
	}
}