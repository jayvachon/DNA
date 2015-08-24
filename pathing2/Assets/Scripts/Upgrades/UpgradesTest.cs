using UnityEngine;
using System.Collections;

public class UpgradesTest : MonoBehaviour {

	int level = 0;

	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			level ++;
			//Upgrades.Instance.SetLevel<DistributorSpeed> (level);
		}
	}	
}
