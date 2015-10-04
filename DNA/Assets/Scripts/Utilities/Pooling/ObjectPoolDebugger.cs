using UnityEngine;
using System.Collections;
using DNA.Paths;
using DNA.Units;

namespace Temp {

	public class ObjectPoolDebugger : MonoBehaviour {

		public void Update () {
			if (Input.GetKeyDown (KeyCode.Space)) {
				//Road r = ObjectPool.Instantiate<Road> ();
				//ObjectPool.Destroy<Road> (r);
				//ObjectPool.GetPool<UnitsList> ("UnitsList");
				//ObjectPool.GetPool<PlotsCreator> ("PlotsCreator");
			}
		}
	}
}