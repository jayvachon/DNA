using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public interface IDamageable {
		void StartTakeDamage ();
		void StopTakeDamage ();
	}
}