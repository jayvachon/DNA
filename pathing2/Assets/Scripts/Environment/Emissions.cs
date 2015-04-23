using UnityEngine;
using System.Collections;

public class Emissions : MonoBehaviour {

	static int emitters = 0;
	public static int Emitters {
		get { return emitters; }
		set { emitters = value; }
	}
}
