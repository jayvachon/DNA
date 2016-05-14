using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public interface IDamageable {
		Vector3 Position { get; }
		void TakeDamage ();
	}
}