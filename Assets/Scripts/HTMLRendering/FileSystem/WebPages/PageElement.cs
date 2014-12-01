using UnityEngine;
using System.Collections;

namespace FauxWeb {
	
	public class PageElement {

		protected float posx = 0f;
		public float PosX {
			get { return posx; }
		}

		protected float posy = 0f;
		public float PosY {
			get { return posy; }
		}

		protected float posz = 0f;
		public float PosZ {
			get { return posz; }
		}

		public Vector3 AnchorPosition {
			get { return new Vector3 (posx, posy, posz); }
		}

		public Vector2 AnchorMax {
			get { return new Vector2 (0, 1); }
		}

		public Vector2 AnchorMin {
			get { return new Vector2 (0, 1); }
		}

		public Vector2 Pivot {
			get { return new Vector2 (0, 1); }
		}
	}
}