using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using DNA.Units;

namespace DNA.Tasks {

	public static class TaskPen {

		public static void Set (PerformerTask task) {
			GameCursor.SetVisual (task,
				ObjectPool.Instantiate (
					UnitRenderer.Renderers [DataManager.GetUnitSymbol (task.GetType ().GetGenericArguments ()[0])]
				) as UnitRenderer
			);
		}
	}
}