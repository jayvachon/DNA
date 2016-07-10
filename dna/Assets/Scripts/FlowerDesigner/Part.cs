using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.FlowerDesigner {

	public class Part : MBRefs {}

	public class Part<T> : Part where T : Part {

		List<T> parts = new List<T> ();

		public int _partCount = 0;

		#if UNITY_EDITOR
		void Update () {
			SetPartCount (_partCount);
			OnUpdatePartCount (parts);
		}
		#endif

		public void SetPartCount (int count) {

			#if UNITY_EDITOR
			count = Mathf.Max (0, count);
			#endif

			int adjustment = count - parts.Count;

			if (adjustment == 0)
				return;

			while (adjustment > 0) {
				T t = ObjectPool.Instantiate<T> ();
				t.Parent = MyTransform;
				t.MyTransform.Reset ();
				parts.Add (t);
				adjustment --;
			}

			while (adjustment < 0) {
				T t = parts[parts.Count-1];
				parts.Remove (t);
				ObjectPool.Destroy<T> (t);
				adjustment ++;
			}

			OnUpdatePartCount (parts);
		}

		protected virtual void OnUpdatePartCount (List<T> _parts) {}
	}
}