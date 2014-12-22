using UnityEngine;
using System.Collections;

public class ActionsListManager : MonoBehaviour {

	public static ActionsListManager instance = null;
	ActionsList actionsList = null;
	public ActionsList Actions {
		get { return actionsList; }
	}

	void Awake () {
		if (instance == null)
			instance = this;
		Events.instance.AddListener<SelectUnitEvent> (OnSelectUnitEvent);
		Events.instance.AddListener<UnselectUnitEvent> (OnUnselectUnitEvent);
	}

	void OnSelectUnitEvent (SelectUnitEvent e) {
		if (e.unit is StaticUnit) {
			StaticUnit su = e.unit as StaticUnit;
			actionsList = su.MyActionsList;
		}
	}

	void OnUnselectUnitEvent (UnselectUnitEvent e) {
		actionsList = null;
	}

	//public void OnPerformAction (Action action) {}
}
