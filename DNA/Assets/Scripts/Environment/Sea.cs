using UnityEngine;
using System.Collections;

namespace DNA {

	public class Sea : MBRefs {

		public readonly float MinLevel = -10f;
		public readonly float MaxLevel = 10f;

		public float Level {
			get { return MyTransform.localPosition.y; }
			set {
				MyTransform.SetLocalPositionY (Mathf.Clamp (value, MinLevel, MaxLevel));
			}
		}

		protected virtual void Awake () {
			Level = MinLevel;
		}
	}
}