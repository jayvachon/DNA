using UnityEngine;
using System.Collections;

namespace GameInventory {

	public class ElderItem : Item {

		float health = 1f;
		public float Health {
			get { return health; }
		}

		public ElderItem () {
			health = Random.Range (0f, 1f);
			Debug.Log (health);
		}

		public override void Print () {
			Debug.Log ("Elder Health: " + health);
		}
	}
}