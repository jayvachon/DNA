using UnityEngine;
using System.Collections;

public class HouseActionsList : ActionsList {

	DefaultAction defaultAction = new DefaultAction ();
	CollectEldersAction collectEldersAction = new CollectEldersAction ();

	public HouseActionsList () {
		Add (defaultAction);
		Add (collectEldersAction);
	}	
}
