using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.Paths;
using DNA.Units;
using DNA.EventSystem;

namespace DNA {

	[RequireComponent (typeof (BoxCollider))]
	public class PointContainer : MBRefs, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

		GridPoint point;
		public GridPoint Point { 
			get { return point; }
			set {
				point = value;
				Position = point.Position;
			}
		}

		public void SetStaticUnit<T> () where T : StaticUnit {
			RemoveStaticUnit ();
			T u = ObjectPool.Instantiate<T> ();
			Point.Unit = u;
			u.transform.SetParent (MyTransform);
			u.transform.localPosition = Vector3.zero;
			u.transform.rotation = MyTransform.rotation;
			LookAtCenter ();
		}

		void RemoveStaticUnit () {
			if (Point.Unit != null) {
				ObjectPool.Destroy (Point.Unit.transform);
				Point.Unit = null;
			}
		}

		bool highlight = false;

		void LookAtCenter () {
			MyTransform.LookAt (new Vector3 (0, -28.7f, 0), Vector3.up);
			MyTransform.SetLocalEulerAnglesX (MyTransform.localEulerAngles.x - 90f);
		}

		public void EnableHighlighting () {
			highlight = true;
		}

		public void DisableHighlighting () {
			highlight = false;
		}

		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new ClickPointEvent (this));
		}
		#endregion

		#region	IPointerEnterHandler implementation
		public void OnPointerEnter (PointerEventData e) {
			if (highlight)
				ObjectPool.Instantiate ("PointHighlight", Point.Position);
		}
		#endregion

		#region	IPointerExitHandler implementation
		public void OnPointerExit (PointerEventData e) {
			if (highlight)
				ObjectPool.Destroy ("PointHighlight");
		}
		#endregion
	}
}