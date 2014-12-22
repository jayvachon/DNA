using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DevelopableUnit : StaticUnit {

	public override void OnAwake () {
		base.OnAwake ();
		MyActionsList.Add (new BuildUnitAction (this, "Hospital", 3f));
		MyActionsList.Add (new BuildUnitAction (this, "House", 1f));
	}

	public override void OnDepart () {
		if (MyActionsList.ActiveAction is BuildUnitAction) {
			BuildUnitAction bua = MyActionsList.ActiveAction as BuildUnitAction;
			string developInto = bua.name;
			Transform t = ObjectPool.Instantiate (developInto, StartPosition);
			ChangePoint (t.GetScript<IPathPoint>());
			ObjectPool.Destroy ("Developable", MyTransform);
		}
	}
}