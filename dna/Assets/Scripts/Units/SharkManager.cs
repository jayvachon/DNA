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

		float startRadius = 100f;

		void OnEnable () {
			LoanManager.onUpdateLoans += OnUpdateLoans;
		}

		void OnUpdateLoans () {
			// CreateShark ();
		}

		[DebuggableMethod ()]
		void CreateShark () {
			float angle = Random.Range (0, Mathf.PI * 2);
			Shark.Create (
				new Vector3 (
					startRadius * Mathf.Sin (angle), 
					GivingTree.Position.y, 
					startRadius * Mathf.Cos (angle)
				), 
				GivingTree
			);
		}
	}
}