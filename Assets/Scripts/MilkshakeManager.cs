using UnityEngine;
using System.Collections;

public class MilkshakeManager : MonoBehaviour {

	int milkshakeCount = 100;
	public int MilkshakeCount {
		get { return milkshakeCount; }
	}

	void OnAddMilkshakesEvent (AddMilkshakesEvent e) {
		AddMilkshakes (e.amount);
	}

	public void AddMilkshakes (int amount) {
		milkshakeCount += amount;
		Events.instance.Raise (new AddMilkshakesEvent (amount));
	}

	public bool SubtractMilkshakes (int amount) {
		if (milkshakeCount > amount) {
			milkshakeCount -= amount;
			Events.instance.Raise (new SubtractMilkshakesEvent (amount));
			return true;
		}
		return false;
	}
}
