#define DEBUG
using UnityEngine;
using System.Collections;

namespace DNA {

	public class SeaManager : MBRefs {

		public float SeaLevel {
			get { return inner.Level; }
		}

		public Levee levee;
		public OuterSea outer;
		public InnerSea inner;

		bool breachedCache = false;

		bool Breached {
			get { return outer.Level > levee.Position.y + levee.Height; }
		}

		#if DEBUG
		void Start () {
			outer.Level = 5;
			inner.Level = 5;
		}
		#endif

		void Update () {
			if (Breached != breachedCache) {
				breachedCache = Breached;
				if (breachedCache) {
					inner.StartRising ();
				} else {
					inner.StopRising ();
				}
			}
			if (breachedCache) {
				inner.OuterLevel = outer.Level;
			}
			#if DEBUG
			if (Input.GetKeyDown (KeyCode.C)) {
				levee.Height += 1f;
			}
			#endif
		}
	}
}