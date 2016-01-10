using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.Units;
using DNA.EventSystem;
using DNA.InputSystem;

namespace DNA.Paths {

	[RequireComponent (typeof (BoxCollider))]
	public class PointContainer : PathElementContainer {

		GridPoint point;
		public GridPoint Point { 
			get { return point; }
			set {
				point = value;
				Position = point.Position;
				Element = point;
			}
		}

		Fertility fertility;

		public void SetFertility (float distanceToCenter, float val) {
			fertility = new Fertility (distanceToCenter, val);
		}

		public override T SetObject<T> (bool destroyPrevious=true) {
			T obj = base.SetObject<T> (destroyPrevious);
			try {
				(obj as StaticUnit).FertilityTier = fertility.Value;
			} catch {
				throw new System.Exception ("Fertility has not been set or '" + obj + "' is not a StaticUnit");
			}
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
			float y = -75;
			MyTransform.LookAt (new Vector3 (0, y, 0), Vector3.up);
			MyTransform.SetLocalEulerAnglesX (MyTransform.localEulerAngles.x - 90f);
		}

		protected override void OnEndConstruction (IPathElementObject obj) {
			StaticUnit s = obj as StaticUnit;
			if (s != null)
				s.FertilityTier = fertility.Value;
		}

		#region IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler implementation
		public override void OnPointerDown (PointerEventData e) {
			Events.instance.Raise (new ClickPointEvent (this));
			Events.instance.Raise (new PointerDownEvent (this));
			if (Point.Object is Unit)
				SelectionHandler.ClickSelectable (Point.Unit, e);
			IPointerDownHandler pdh = Point.Object as IPointerDownHandler;
			if (pdh != null) {
				pdh.OnPointerDown (e);
			}
		}

		public override void OnPointerEnter (PointerEventData e) {
			Events.instance.Raise (new MouseEnterPointEvent (this));
		}

		public override void OnPointerExit (PointerEventData e) {
			Events.instance.Raise (new MouseExitPointEvent (this));
		}
		#endregion
	}
}