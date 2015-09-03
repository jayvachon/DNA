using UnityEngine;
using System.Collections;
using DNA.Models;

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
}
