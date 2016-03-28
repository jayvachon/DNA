using UnityEngine;
using System.Collections;

public class Eyesight : Upgrade<int> {

	protected override int[] Levels {
		get { return new int[] { 3, 4, 5, 6, 7 }; }
	}
}
