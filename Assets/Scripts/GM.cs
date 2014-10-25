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

	// Static properties
	static Step activeStep;
	public static Step ActiveStep {
		get { return GM.activeStep; }
	}

	void Awake () {
		if (instance == null) instance = this;
		mm = Instantiate (mm) as MilkshakeManager;
		pm = Instantiate (pm) as PersonManager;
		cm = Instantiate (cm) as CowManager;
		bm = Instantiate (bm) as BuildingManager;
		Events.instance.AddListener<ChangeActiveStepEvent>(OnChangeActiveStepEvent);
	}

	void Start () {
		StartCoroutine (InitSelectableManagers ());
	}

	IEnumerator InitSelectableManagers () {
		while (activeStep == null || !activeStep.Ready) {
			yield return null;
		}
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

	public void OnChangeActiveStepEvent (ChangeActiveStepEvent e) {
		activeStep = e.step;
	}
}
