using UnityEngine;
using System.Collections;

namespace DNA {

	public class Sea2 : MBRefs {

		protected readonly float MinLevel = -10f;
		protected readonly float MaxLevel = 10f;

		public float Level {
			get { return MyTransform.localPosition.y; }
			set {
				MyTransform.SetLocalPositionY (Mathf.Clamp (value, MinLevel, MaxLevel));
			}
		}

		protected virtual void Awake () {
			Level = MinLevel;
			GetComponent<Renderer> ().SetColor (Palette.ApplyAlpha (Palette.Blue, 0.75f));
		}
	}
}