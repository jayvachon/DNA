using UnityEngine;
using System.Collections;
using DNA.Models;

namespace DNA {

	public static class DataManager {

		static GameData gameData;
		public static GameData Data {
			get {
				if (gameData == null) {
					gameData = new GameData ();
				}
				return gameData;
			}
		}

		public static TaskSettings GetTaskSettings (System.Type taskType) {
			try {
				return Data.TasksSettings[taskType];
			} catch {
				throw new System.Exception ("No model exists for the task '" + taskType + "'");
			}
		}

		public static UnitSettings GetUnitSettings (System.Type unitType) {
			try {
				return Data.UnitsSettings[unitType];
			} catch {
				throw new System.Exception ("No model exists for the task '" + unitType + "'");
			}
		}

		public static int GetConstructionCost<T> () where T : Units.StaticUnit, Paths.IPathElementObject {
			try {
				return new Tasks.GenerateUnit<T> ().TotalCost;
			} catch {
				throw new System.Exception ("Could not find the cost of GenerateUnit<" + typeof (T) + ">");
			}
		}
	}
}