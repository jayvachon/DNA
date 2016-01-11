using UnityEngine;
using System.Collections;

public class MilkshakeCapacity : Upgrade<int> {

	protected override int[] Levels {
		get { return new int[] { 3, 5, 8, 15 }; }
	}
}
