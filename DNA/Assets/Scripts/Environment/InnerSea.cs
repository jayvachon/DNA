using UnityEngine;
using System.Collections;

namespace DNA {

	public class InnerSea : Sea2 {

		bool rising = false;
		public float OuterLevel { get; set; }

		protected override void Awake () {
			base.Awake ();
			riseRate = 0.01f;
		}

		public void StartRising () {
			rising = true;
		}

		public void StopRising () {
			rising = false;
		}

		void Update () {
			if (rising && Level < OuterLevel)
				Level += riseRate;
			if (!rising)
				Level -= riseRate;
		}
	}
}