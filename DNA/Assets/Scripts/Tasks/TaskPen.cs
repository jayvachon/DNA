using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using DNA.EventSystem;
using DNA.Units;
using DNA.Paths;

namespace DNA.Tasks {

	public static class TaskPen {

		static PerformerTask task;

		public static void Set (PerformerTask newTask) {

			task = newTask;
			System.Type[] taskTypes = task.GetType ().GetGenericArguments ();

			if (taskTypes.Length > 0) {
				string renderer = UnitRenderer.GetRenderer (DataManager.GetUnitSymbol (taskTypes[0]));
				GameCursor.SetVisual (task, ObjectPool.Instantiate (renderer) as UnitRenderer);
				EmptyClickHandler.Instance.onClick += OnEmptyClick;
			}

			Events.instance.AddListener<ClickPointEvent> (OnClickPointEvent);
			Events.instance.AddListener<MouseEnterPointEvent> (OnMouseEnterPointEvent);
			Events.instance.AddListener<MouseExitPointEvent> (OnMouseExitPointEvent);
		}

		static void OnEmptyClick (System.Type type) {
			Remove ();
		}

		static void Remove () {
			task = null;
			GameCursor.RemoveVisual ();
			EmptyClickHandler.Instance.onClick -= OnEmptyClick;
			Events.instance.RemoveListener<ClickPointEvent> (OnClickPointEvent);
			Events.instance.RemoveListener<MouseEnterPointEvent> (OnMouseEnterPointEvent);
			Events.instance.RemoveListener<MouseExitPointEvent> (OnMouseExitPointEvent);
		}

		static bool CanConstructOnPoint (GridPoint point) {
			return (task as IConstructable).CanConstruct (point);
		}

		static void Construct (GridPoint point, PointContainer container) {

			if (task is ConstructRoad) {

				RoadConstructor.Instance.AddPoint (point);
				if (RoadConstructor.Instance.PointCount < 2)
					return;

				CostTask t = task as CostTask;
				string text = "Purchase: ";
				foreach (var cost in t.Costs) {
					text += cost.Value.ToString () + cost.Key.Substring (0, 1);
				}
				UI.Instance.ConstructPrompt.Open (text,
					() => {
						task.Start ();
						Remove ();
					},
					() => {
						RoadConstructor.Instance.Clear ();
						Remove ();
					}
				);
			}

			ConstructUnit c = task as ConstructUnit;
			if (c != null) {
				c.ElementContainer = container;
				c.Start ();
			}
		}

		static void OnClickPointEvent (ClickPointEvent e) {
			Construct (e.Point, e.Container);
			GameCursor.Target = null;
		}

		static void OnMouseEnterPointEvent (MouseEnterPointEvent e) {
			if (CanConstructOnPoint (e.Point)) {
				GameCursor.Target = e.Point.Position;
			}
		}

		static void OnMouseExitPointEvent (MouseExitPointEvent e) {
			GameCursor.Target = null;
		}
	}
}