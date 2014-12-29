using UnityEngine;
using System.Collections;

[System.Serializable]
public class ElderHolder : System.Object {

	int count = 0;
	int max = 5;

	public bool HasElders {
		get { return count > 0; }
	}

	public static void Trade (IElderHoldable from, IElderHoldable to) {
		// Sends all the Elders from 'from' to 'to,' and returns them to 'from' if
		// there's not enough room in 'to'
		from.Elders.Add (to.Elders.Add (from.Elders.Clear ()));
	}

	public int Add (int amount) {
		int overflow = amount + count - max;
		count = Mathf.Min (max, amount + count);
		return Mathf.Max (0, overflow);
	}

	public int Subtract (int amount) {
		int excess = amount - count;
		amount = Mathf.Max (0, count - amount);
		return excess;
	}

	public int Clear () {
		int tempCount = count;
		count = 0;
		return tempCount;
	}
}
