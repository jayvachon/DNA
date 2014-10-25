using UnityEngine;
using System.Collections;

public class PersonManager : Manager {

	ObjectPool personPool;

	void Awake () {
		Events.instance.AddListener<CreatePersonEvent>(OnCreatePersonEvent);
		personPool = ObjectPool.GetPool ("Person");
	}

	public override void Init () {
		CreatePerson (new Vector3 (0f, 20f, 0f));
	}

	void OnCreatePersonEvent (CreatePersonEvent e) {
		CreatePerson (e.position);
	}

	void CreatePerson (Vector3 position) {
		personPool.GetInstance (position);
	}
}
