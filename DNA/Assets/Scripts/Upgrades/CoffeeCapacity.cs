using UnityEngine;
using System.Collections;

public class CoffeeCapacity : Upgrade<int> {

	protected override int[] Levels {
		get { return new int[] { 2, 3, 5, 10 }; }
	}
}
