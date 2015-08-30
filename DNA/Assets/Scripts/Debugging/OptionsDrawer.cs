using UnityEngine;
using System.Collections;

public class OptionsDrawer : MonoBehaviour {

	#if UNITY_EDITOR
	void OnGUI () {
		#if VARIABLE_COST
		CostValues.Instance.DrawOptions ();
		#endif
		#if VARIABLE_TIME
		TimerValues.Instance.DrawOptions ();
		#endif
	}
	#endif
}
