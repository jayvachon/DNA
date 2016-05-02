using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DNA {

	public class LeveeInfo : MonoBehaviour {

		public SeaManager sea;
		public RectTransform leveeLevel;
		public RectTransform seaLevel;
		public Image seaMeter;

		float min, max;

		void Awake () {
			min = sea.outer.MinLevel;
			max = sea.outer.MaxLevel;
		}

		void Update () {
			// float s = Mathf.InverseLerp (min, max, sea.outer.Level);
			float l = Mathf.InverseLerp (min, max, sea.LeveeTop);
			float s = sea.outer.Fill;
			// seaLevel.transform.SetLocalScaleY (s);
			seaLevel.transform.SetLocalScaleY (sea.outer.Fill);
			leveeLevel.transform.SetLocalScaleY (l);

			if (s > l) {
				seaMeter.color = Palette.Red;
			} else {
				seaMeter.color = Palette.Sea;
			}
		}
	}
}