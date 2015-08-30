using UnityEngine;
using System.Collections;

public class BuildingManager : Manager {

	ObjectPool hospitalPool;
	ObjectPool housePool;

	void Awake () {
		Events.instance.AddListener<CreateBuildingEvent>(OnCreateBuildingEvent);
		hospitalPool = ObjectPool.GetPool ("Hospital");
		housePool = ObjectPool.GetPool ("House");
	}

	public override void Init () {
		/*Vector3 hospitalPosition = GM.ActiveStep.GetRandomHexagonPosition ();
		hospitalPosition.y = 5;
		CreateHospital (hospitalPosition);
		Vector3 housePosition = GM.ActiveStep.GetRandomHexagonPosition ();
		housePosition.y = 2.5f;
		CreateHouse (housePosition);*/
	}

	void OnCreateBuildingEvent (CreateBuildingEvent e) {
		// do something
	}

	void CreateBuilding (Vector3 position, string type) {
		switch (type) {
			case "hospital": CreateHospital (position); break;
			case "house": CreateHouse (position); break;
		}
	}

	void CreateHospital (Vector3 position) {
		hospitalPool.GetInstance (position);
	}

	void CreateHouse (Vector3 position) {
		housePool.GetInstance (position);
	}
}
