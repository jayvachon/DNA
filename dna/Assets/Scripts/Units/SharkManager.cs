using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class SharkManager : MonoBehaviour {

		GivingTreeUnit givingTree;
		GivingTreeUnit GivingTree {
			get {
				if (givingTree == null) {
					givingTree = GameObject.FindObjectOfType (typeof (GivingTreeUnit)) as GivingTreeUnit;
				}
				return givingTree;
			}
		}

		void OnEnable () {
			LoanManager.onUpdateLoans += OnUpdateLoans;
		}

		void OnUpdateLoans () {

		}

		[DebuggableMethod ()]
		void CreateShark () {
			Shark.Create (new Vector3 (20, 20, 0), GivingTree);
		}
	}
}