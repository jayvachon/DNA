using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	string[] keys = new string[] {
		"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
		"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" 
	};

	void Update () {
		if (Input.anyKeyDown) {
			foreach (string k in keys) {
				if (Input.GetKey (k)) Events.instance.Raise (new KeyPressEvent (k));
			}
		}
	}
}
