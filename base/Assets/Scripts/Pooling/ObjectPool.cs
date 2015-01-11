using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//https://gist.github.com/nickgravelyn/4385548

// All pool objects must implement the IPoolable interface
public class ObjectPool : MonoBehaviour {

	private static readonly Dictionary<string, ObjectPool> _poolsByName = new Dictionary<string, ObjectPool>();
	
	public static ObjectPool GetPool(string name) { return _poolsByName[name]; }
	
	[SerializeField]
	private string _poolName = string.Empty;
	
	[SerializeField]
	private Transform _prefab = null;
	
	[SerializeField]
	private int _initialCount = 10;
	
	[SerializeField]
	private bool _parentInstances = true;
	
	private readonly Stack<Transform> _instances = new Stack<Transform>();
	
	void Awake() {

		System.Diagnostics.Debug.Assert(_prefab);
		_poolsByName[_poolName] = this;
		
		for (int i = 0; i < _initialCount; i++) {
			var t = Instantiate(_prefab) as Transform;
			InitializeInstance(t);
			ReleaseInstance(t);
		}
	}
	
	public Transform GetInstance(Vector3 position = new Vector3()) {

		Transform t = null;
		
		if (_instances.Count > 0) {
			t = _instances.Pop();
		} else {
			//Debug.LogWarning (_poolName + " pool ran out of instances!", this);
			t = Instantiate(_prefab) as Transform;
		}
		
		t.position = position;
		InitializeInstance(t);
		
		return t;
	}
	
	private void InitializeInstance(Transform instance) {

		if (_parentInstances) {
			instance.parent = transform;
		}
		
		instance.gameObject.SetActive(true);
		instance.BroadcastMessage("OnPoolCreate", this, SendMessageOptions.DontRequireReceiver);
	}
	
	public void ReleaseInstance(Transform instance) {

		instance.BroadcastMessage("OnPoolRelease", this, SendMessageOptions.DontRequireReceiver);
		instance.gameObject.SetActive(false);
		_instances.Push(instance);
	}

	public static Transform Instantiate ( string poolName, Vector3 position ) {
		Transform t = ObjectPool.GetPool ( poolName ).GetInstance ( position );
		t.GetScript<IPoolable>().OnCreate ();
		return t;
	}

	public static void Destroy ( string poolName, Transform instance ) {
		instance.GetScript<IPoolable>().OnDestroy ();
		ObjectPool.GetPool ( poolName ).ReleaseInstance ( instance );
	}
}