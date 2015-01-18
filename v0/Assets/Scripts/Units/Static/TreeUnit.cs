using UnityEngine;
using System.Collections;
using GameActions;
using GameInput;

public class TreeUnit : StaticUnit, IActionable {

	public ActionsList ActionsList { get; set; }
	
	protected override void Awake () {
		base.Awake ();
		ActionsList = new TreeActionsList (this);
		ActionsDrawer.Create (MyTransform, ActionsList);
	}

	void Update () {
		if (!SelectionManager.IsSelected (this))
			return;
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			ActionsList.Start<CreateUnit<ProducerUnit>> ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			ActionsList.Start<CreateUnit<DistributorUnit>> ();
		}
	}

	public void OnEndAction () {}
}

public class TreeActionsList : ActionsList {

	public TreeActionsList (IActionable actionable) : base (actionable) {
		Add (new CreateUnit<ProducerUnit> (5));
		Add (new CreateUnit<DistributorUnit> (5));
	}
}