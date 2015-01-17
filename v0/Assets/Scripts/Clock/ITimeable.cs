using UnityEngine;
using System.Collections;

namespace GameClock {
	
	public interface ITimeable {
		void OnBeatsElapsed ();
	}
}