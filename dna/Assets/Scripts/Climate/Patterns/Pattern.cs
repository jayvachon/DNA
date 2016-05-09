using UnityEngine;
using System.Collections;

namespace DNA.Climate {

	[System.Serializable]
	public abstract class Pattern : System.Object {

		public float Cursor { get; set; }

		string name = "";
		public virtual string Name {
			get { return name; } 
			set { name = value; }
		}

		public abstract float ValueAt (float position);

		public float ValueAtCursor {
			get { return ValueAt (Cursor); }
		}

		public virtual void Update () {
			Cursor += Time.deltaTime;
		}

		// Gets the upcoming values from the cursor to cursor+time. Resolution is time in seconds.
		public float[] LookAheadValues (float time, float resolution=0.1f) {

			float[] vals = new float[Mathf.CeilToInt (time / resolution)];

			for (int i = 0; i < vals.Length; i ++)
				vals[i] = ValueAt (Cursor + (float)i * resolution);

			return vals;
		}

		public override string ToString () {
			return "Cursor: " + Cursor + "\nValue: " + ValueAtCursor;
		}
	}
}