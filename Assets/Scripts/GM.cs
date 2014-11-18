using UnityEngine;
using System.Collections;

public class GM : MonoBehaviour {

	// The Game Manager
	public static GM instance;

	// Managers
	public MilkshakeManager mm;
	public PersonManager pm;
	public CowManager cm;
	public BuildingManager bm;

	void Awake () {
		if (instance == null) instance = this;
		mm = Instantiate (mm) as MilkshakeManager;
		pm = Instantiate (pm) as PersonManager;
		cm = Instantiate (cm) as CowManager;
		bm = Instantiate (bm) as BuildingManager;
	}

	void Start () {
		pm.Init ();
		cm.Init ();
		bm.Init ();
	}

	public int GetMshake () {
		return mm.MilkshakeCount;
	}

	public void AddMshake (int amount) {
		mm.AddMilkshakes (amount);
	}

	public bool SubMshake (int amount) {
		return mm.SubtractMilkshakes (amount);
	}
}
