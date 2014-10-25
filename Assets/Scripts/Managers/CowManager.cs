using UnityEngine;
using System.Collections;

public class CowManager : Manager {
	
	ObjectPool cowPool;

	void Awake () {
		Events.instance.AddListener<CreateCowEvent>(OnCreateCowEvent);
		cowPool = ObjectPool.GetPool ("Cow");
	}

	public override void Init () {
		Vector3 cowPosition = GM.ActiveStep.GetRandomHexagonPosition ();
		cowPosition.y = 5;
		CreateCow (cowPosition);
	}
	
	void OnCreateCowEvent (CreateCowEvent e) {
		CreateCow (e.position);
	}
	
	void CreateCow (Vector3 position) {
		cowPool.GetInstance (position);
	}
}
