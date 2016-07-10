using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.FlowerDesigner {

	public class Pistil : Part<Cube> {

		public Cube ovary;

		[Range (0.1f, 3f)]
		public float _size = 1f;

		[Range (0.2f, 1f)] // percent
		public float _styleSize = 0.67f;

		protected override void OnUpdatePartCount (List<Cube> style) {
			
			ovary.Scale = _size;

			if (style.Count == 0)
				return;

			style[0].LocalPosition = new Vector3 (0, ovary.Scale * 0.5f, 0);

			for (int i = 1; i < style.Count; i ++) {
				Cube c = style[i];
				Cube parent = style[i-1];
				c.LocalPosition = parent.LocalPosition + new Vector3 (0, parent.Scale*0.5f + c.Scale*0.5f, 0f);
			}

			foreach (Cube s in style) {
				s.Scale = _size * _styleSize;
			}
		}
	}
}