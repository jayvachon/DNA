using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.Paths;
using DNA.Units;
using DNA.EventSystem;

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

		public void SetStaticUnit<T> () where T : StaticUnit {
			SetObject (ObjectPool.Instantiate<T> ());
			LookAtCenter ();
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

			// temp -- just for testing
			if (Element.State == DevelopmentState.UnderConstruction)
				EndConstruction ();
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