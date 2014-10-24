using UnityEngine;
using System.Collections;

public class CowManager : MonoBehaviour {
	
	ObjectPool cowPool;
	
	void Start () {
		cowPool = ObjectPool.GetPool ("Cow");
		Events.instance.AddListener<CreateCowEvent>(OnCreateCowEvent);
	}
	
	void OnCreateCowEvent (CreateCowEvent e) {
		CreateCow (e.position);
	}
	
	void CreateCow (Vector3 position) {
		cowPool.GetInstance (position);
	}
}
