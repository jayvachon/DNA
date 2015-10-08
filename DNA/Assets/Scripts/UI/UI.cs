using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	static UI instance = null;
	static public UI Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (UI)) as UI;
			}
			return instance;
		}
	}

	public ConstructPrompt ConstructPrompt;

}
