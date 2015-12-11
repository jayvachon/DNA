using UnityEngine;
using System.Linq;
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

		public static string GetUnitSymbol (System.Type unitType) {
			return GetUnitSettings (unitType).Symbol;
		}

		public static int GetConstructionCost (string unitSymbol) {
			try {
				return (Data.TasksSettings.Tasks
					.Values.Where (x => x.Symbol == "construct_" + unitSymbol)
					.ToList ()[0] as CostTaskSettings)
					.Costs[0].Sum (x => x.Value);
			} catch {
				throw new System.Exception ("Could not find a cost for the unit '" + unitSymbol + "'");
			}
		}
	}
}