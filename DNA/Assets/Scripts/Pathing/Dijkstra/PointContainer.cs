using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.Paths;
using DNA.Units;
using DNA.EventSystem;
using DNA.InputSystem;

namespace DNA {

	[RequireComponent (typeof (BoxCollider))]
	public class PointContainer : PathElementContainer, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

		GridPoint point;
		public GridPoint Point { 
			get { return point; }
			set {
				point = value;
				Position = point.Position;
				Element = point;
			}
		}

		public override T SetObject<T> () {
			T obj = base.SetObject<T> ();
			LookAtCenter ();
			return obj;
		}

		void RemoveStaticUnit () {
			if (Point.Object != null) {
				ObjectPool.Destroy (((StaticUnit)Point.Object).transform);
				Point.Object = null;
			}
		}

		void LookAtCenter () {
			MyTransform.LookAt (new Vector3 (0, -28.7f, 0), Vector3.up);
			MyTransform.SetLocalEulerAnglesX (MyTransform.localEulerAngles.x - 90f);
		}

		#region IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler implementation
		public void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new ClickPointEvent (this));
			if (Point.Object is Unit)
				SelectionHandler.ClickSelectable (Point.Unit, e);
		}

		public void OnPointerEnter (PointerEventData e) {
			Events.instance.Raise (new MouseEnterPointEvent (this));
		}

		public void OnPointerExit (PointerEventData e) {
			Events.instance.Raise (new MouseExitPointEvent (this));
		}
		#endregion
	}
}