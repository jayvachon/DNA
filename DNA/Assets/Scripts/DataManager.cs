using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
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
				return Data.TasksSettings.Tasks.Values.Where (x => x.Type == taskType).ToList ()[0];
			} catch {
				throw new System.Exception ("No model exists for the task '" + taskType + "'");
			}
		}

		public static TaskSettings GetTaskSettings (string symbol) {
			try {
				return Data.TasksSettings[symbol];
			} catch {
				throw new System.Exception ("No task exists with the symbol '" + symbol + "'");
			}
		}

		public static TaskSettings GetPerformerPairSettings (System.Type taskType) {
			try {
				return Data.TasksSettings.Tasks.FirstOrDefault (x => x.Value.Pair == taskType).Value;
			} catch {
				throw new System.Exception ("No model could be found for the pair of '" + taskType + "'");
			}
		}

		public static UnitSettings GetUnitSettings (System.Type unitType) {
			try {
				return Data.UnitsSettings[unitType];
			} catch {
				throw new System.Exception ("No model exists for the unit '" + unitType + "'");
			}
		}

		public static LoanSettings GetLoanSettings (System.Type loanType) {
			try {
				return Data.LoansSettings[loanType];
			} catch {
				throw new System.Exception ("No model exists for the loan '" + loanType + "'");
			}
		}

		public static string GetUnitSymbol (System.Type unitType) {
			return GetUnitSettings (unitType).Symbol;
		}

		public static int GetConstructionCost (string unitSymbol) {
			try {
				return ((CostTaskSettings)Data.TasksSettings.Tasks["construct_" + unitSymbol]).Costs[0].Sum (x => x.Value);
			} catch {
				throw new System.Exception ("Could not find a cost for the unit '" + unitSymbol + "'");
			}
		}

		public static Dictionary<string, int> GetConstructionCosts (string unitSymbol) {
			try {
				return ((CostTaskSettings)Data.TasksSettings.Tasks["construct_" + unitSymbol]).Costs[0];
			} catch {
				throw new System.Exception ("Could not find a cost for the unit '" + unitSymbol + "'");
			}
		}
	}
}