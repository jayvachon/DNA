#define DEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//https://gist.github.com/nickgravelyn/4385548

// All pool objects must implement the IPoolable interface
public class ObjectPool : MonoBehaviour {

	private static readonly Dictionary<string, ObjectPool> _poolsByName = new Dictionary<string, ObjectPool> ();
	
	public static ObjectPool GetPool(string name) {
		if (_poolsByName.ContainsKey (name)) {
			return _poolsByName[name];
		}
		return null;
	}
	
	[SerializeField]
	private string _poolName = string.Empty;
	
	[SerializeField]
	private Transform _prefab = null;
	
	[SerializeField]
	private int _initialCount = 0;
	
	[SerializeField]
	private bool _parentInstances = false;
	
	private readonly Stack<Transform> _instances = new Stack<Transform> ();
	
	public void Init (string poolName, Transform prefab) {
		_poolName = poolName;
		_prefab = prefab;
		System.Diagnostics.Debug.Assert(_prefab);
		_poolsByName[_poolName] = this;
		
		for (int i = 0; i < _initialCount; i++) {
			var t = Instantiate(_prefab) as Transform;
			InitializeInstance(t);
			ReleaseInstance(t);
		}
	}
	
	public Transform GetInstance (Vector3 position = new Vector3()) {

		Transform t = null;
		
		if (_instances.Count > 0) {
			t = _instances.Pop();
		} else {
			t = Instantiate(_prefab) as Transform;
		}
		
		t.position = position;
		InitializeInstance (t);
		
		return t;
	}
	
	private void InitializeInstance (Transform instance) {

		if (_parentInstances) {
			instance.parent = transform;
		}
		
		instance.gameObject.SetActive (true);
	}
	
	public void ReleaseInstance (Transform instance) {

		instance.gameObject.SetActive (false);
		_instances.Push (instance);
	}

	public static Transform Instantiate (string poolName, Vector3 position) {
		Transform t = ObjectPool.GetPool (poolName).GetInstance (position);
		#if UNITY_EDITOR && DEBUG
		if (t.GetScript<IPoolable> () == null) {
			Debug.LogError (string.Format ("The object {0} must implement the IPoolable interface", t));
		}
		#endif
		t.GetScript<IPoolable> ().OnCreate ();
		return t;
	}

	public static void Destroy (string poolName, Transform instance) {
		#if UNITY_EDITOR && DEBUG
		if (instance.GetScript<IPoolable> () == null) {
			Debug.LogError (string.Format ("The object {0} must implement the IPoolable interface", instance));
		}
		#endif
		instance.GetScript<IPoolable>().OnDestroy ();
		ObjectPool.GetPool (poolName).ReleaseInstance (instance);
	}
}