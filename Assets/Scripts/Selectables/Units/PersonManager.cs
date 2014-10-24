using UnityEngine;
using System.Collections;

public class PersonManager : MonoBehaviour {

	ObjectPool personPool;

	void Start () {
		personPool = ObjectPool.GetPool ("Person");
		Events.instance.AddListener<CreatePersonEvent>(OnCreatePersonEvent);
	}

	void OnCreatePersonEvent (CreatePersonEvent e) {
		CreatePerson (e.position);
	}

	void CreatePerson (Vector3 position) {
		personPool.GetInstance (position);
	}
}
