using UnityEngine;
using System.Collections;

public class LeveeHeight : Upgrade<int> {

	protected override int[] Levels {
		get { return new int[] { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21 }; }
	}
}
