using UnityEngine;
using System.Collections;

public class MyGUI : MonoBehaviour {

	public class Command : System.Object {
		string key;
		string description;
		public string Key { 
			get { return key; }
		}
		public string Description {
			get { return description; }
		}

		public Command (string key, string description) {
			this.key = key;
			this.description = description;
		}
	}

	Command[] commands;
	int commandCount = 0;
	int milkshakesCount = 100;

	MyGUI instance;

	void Awake () {
		if (instance == null) instance = this;
	}

	void Start () {
		Events.instance.AddListener<SetCommandsEvent>(OnSetCommands);
		Events.instance.AddListener<ResetCommandsEvent>(OnResetCommands);
		Events.instance.AddListener<AddMilkshakesEvent>(OnAddMilkshakesEvent);
		Events.instance.AddListener<SubtractMilkshakesEvent>(OnSubtractMilkshakesEvent);
	}

	void OnGUI () {
		GUI.Label (new Rect (10, 10, 300, 20), new GUIContent ("Milkshakes: " + milkshakesCount));
		if (commandCount == 0) return;
		for (int i = 0; i < commandCount; i ++) {
			float left = 10;
			float top = 30;
			float space = 20;
			Rect r = new Rect (
				left,
				top + (i * space),
				300,
				20
			);
			string key = "[" + commands[i].Key + "] ";
			string description = " " + commands[i].Description;
			GUI.Label (r, new GUIContent (key + description));
		}
	}

	void UpdateMilkshakeCount () {
		milkshakesCount = GM.instance.GetMshake ();
	}

	void OnSetCommands (SetCommandsEvent e) {
		string[] keys = e.keys;
		string[] descriptions = e.descriptions;
		if (keys.Length != descriptions.Length) {
			Debug.LogError ("keys and descriptions length must match");
		}
		commandCount = keys.Length;
		commands = new Command[commandCount];
		for (int i = 0; i < commandCount; i ++) {
			commands[i] = new Command (keys[i], descriptions[i]);
		}
	}

	void OnResetCommands (ResetCommandsEvent e) {
		commandCount = 0; 
		commands = new Command[0];
	}

	void OnAddMilkshakesEvent (AddMilkshakesEvent e) {
		UpdateMilkshakeCount ();
	}

	void OnSubtractMilkshakesEvent (SubtractMilkshakesEvent e) {
		UpdateMilkshakeCount ();
	}
}
