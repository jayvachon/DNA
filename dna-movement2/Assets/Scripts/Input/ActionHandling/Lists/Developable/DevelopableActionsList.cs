using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DevelopableActionsList : ActionsList {

	DefaultAction defaultAction 			= new DefaultAction ();
	BuildHospitalAction buildHospitalAction = new BuildHospitalAction ();
	BuildHouseAction buildHouseAction 		= new BuildHouseAction ();
	BuildCowAction buildCowAction 			= new BuildCowAction ();

	public DevelopableActionsList () {
		Add (defaultAction);
		Add (buildHospitalAction);
		Add (buildHouseAction);
		Add (buildCowAction);
	}
}
