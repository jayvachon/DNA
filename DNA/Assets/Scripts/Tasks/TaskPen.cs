using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using DNA.EventSystem;
using DNA.Units;
using DNA.Paths;

namespace DNA.Tasks {

	public static class TaskPen {

		public delegate void OnRemove ();

		public static OnRemove onRemove;
		static PerformerTask task;
		static UnitRenderer visual;
		static bool roadTask;

		public static void Set (PerformerTask newTask) {

			if (!newTask.Enabled)
				return;

			task = newTask;
			roadTask = task is ConstructRoad;
			bool seedTask = task is PlantSeed;

			System.Type[] taskTypes;
			if (roadTask) {
				taskTypes = new [] { typeof (Road) };
			} else if (seedTask) {
				taskTypes = new [] { typeof (Seed) };
			} else {
				taskTypes = task.GetType ().GetGenericArguments ();
			}

			if (taskTypes.Length > 0) {
				string renderer = UnitRenderer.GetRenderer (DataManager.GetUnitSymbol (taskTypes[0]));
				visual = ObjectPool.Instantiate (renderer) as UnitRenderer;
				visual.SetAlpha (0.33f);
				GameCursor.Instance.SetVisual (task, visual);
			}

			GameCursor.Instance.onClick += OnClick;

			if (roadTask) {
				Events.instance.AddListener<ClickConnectionEvent> (OnClickConnectionEvent);
				Events.instance.AddListener<MouseEnterConnectionEvent> (OnMouseEnterConnectionEvent);
				Events.instance.AddListener<MouseExitConnectionEvent> (OnMouseExitConnectionEvent);
			} else {
				Events.instance.AddListener<ClickPointEvent> (OnClickPointEvent);
				Events.instance.AddListener<MouseEnterPointEvent> (OnMouseEnterPointEvent);
				Events.instance.AddListener<MouseExitPointEvent> (OnMouseExitPointEvent);
			}
		}

		static void OnClick (bool overTarget) {
			if (!overTarget) Remove ();
		}

		public static void Remove () {

			if (task != null) {
				task = null;

				GameCursor.Instance.Target = null;
				GameCursor.Instance.onClick -= OnClick;
				GameCursor.Instance.RemoveVisual ();

				Events.instance.RemoveListener<ClickConnectionEvent> (OnClickConnectionEvent);
				Events.instance.RemoveListener<MouseEnterConnectionEvent> (OnMouseEnterConnectionEvent);
				Events.instance.RemoveListener<MouseExitConnectionEvent> (OnMouseExitConnectionEvent);
				
				Events.instance.RemoveListener<ClickPointEvent> (OnClickPointEvent);
				Events.instance.RemoveListener<MouseEnterPointEvent> (OnMouseEnterPointEvent);
				Events.instance.RemoveListener<MouseExitPointEvent> (OnMouseExitPointEvent);
			}

			if (onRemove != null)
				onRemove ();
		}

		static bool CanConstructOnElement (PathElement element) {
			return (task as IConstructable).CanConstruct (element);
		}

		static void Construct (GridPoint point, PointContainer container) {
			IConstructable c = task as IConstructable;
			if (c != null) {
				c.ElementContainer = container;
				c.Start ();
				Remove ();
			}
		}

		static void ConstructRoad (Connection connection, ConnectionContainer container) {
			ConstructRoad c = task as ConstructRoad;
			c.ElementContainer = container;
			c.Start ();
			Remove ();
		}

		static void OnClickPointEvent (ClickPointEvent e) {
			if (CanConstructOnElement (e.Point)) {
				Construct (e.Point, e.Container);
			}
		}

		static void OnMouseEnterPointEvent (MouseEnterPointEvent e) {
			if (CanConstructOnElement (e.Point)) {
				GameCursor.Instance.Target = e.Point.Position;
				visual.SetAlpha (1f);
			}
		}

		static void OnMouseExitPointEvent (MouseExitPointEvent e) {
			GameCursor.Instance.Target = null;
			visual.SetAlpha (0.33f);
		}

		static void OnClickConnectionEvent (ClickConnectionEvent e) {
			if (CanConstructOnElement (e.Connection)) {
				ConstructRoad (e.Connection, e.Container);
			}
		}

		static void OnMouseEnterConnectionEvent (MouseEnterConnectionEvent e) {
			if (CanConstructOnElement (e.Connection)) {
				GameCursor.Instance.Target = e.Connection.Center;
				GameCursor.Instance.Rotation = e.Connection.Rotation;
				visual.SetAlpha (1f);
			}
		}

		static void OnMouseExitConnectionEvent (MouseExitConnectionEvent e) {
			GameCursor.Instance.Target = null;
			visual.SetAlpha (0.33f);
		}
	}
}