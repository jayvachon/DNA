using UnityEngine;
using System.Collections;

namespace DNA {

	public class Sea : MBRefs {

		public virtual float Fill {
			get { return average.Fill; }
		}

		public virtual float Level {
			get { return average.Level; }
		}

		public float RiseRate { get; set; }
		public float MinLevel { get { return average.Min; } }
		public float MaxLevel { get { return average.Max; } }

		protected SeaLevel average = new SeaLevel (-10f, 10f);

		void Update () {

			// raise/lower the sea level
			average.Fill += RiseRate;
		}
	}
}