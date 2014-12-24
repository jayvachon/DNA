using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DevelopableUnit : StaticUnit {

	public override void OnAwake () {
		base.OnAwake ();
		MyActionsList.Add (new BuildUnitAction (3f, "Hospital"));
		MyActionsList.Add (new BuildUnitAction (2f, "House"));
		MyActionsList.Add (new BuildUnitAction (1f, "Cow"));
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