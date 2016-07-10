using UnityEngine;
using System.Collections;

namespace DNA.FlowerDesigner {

	public class Cube : Part {

		public float Scale {
			get { return LocalScale.y; }
			set { LocalScale = new Vector3 (value, value, value); }
		}
	}
}