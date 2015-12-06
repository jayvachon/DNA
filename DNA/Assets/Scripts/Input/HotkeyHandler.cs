using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;
using DNA.Tasks;
using DNA.InputSystem;

public class HotkeyHandler : MonoBehaviour {

	static HotkeyHandler instance = null;
	static public HotkeyHandler Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (HotkeyHandler)) as HotkeyHandler;
				if (instance == null) {
					GameObject go = new GameObject ("HotkeyHandler");
					DontDestroyOnLoad (go);
					instance = go.AddComponent<HotkeyHandler>();
				}
			}
			return instance;
		}
	}

	class Modifiers {
		
		List<KeyCode> modifiers = new List<KeyCode> () {
			KeyCode.LeftShift, KeyCode.RightShift, 
			KeyCode.LeftControl, KeyCode.RightControl, 
			KeyCode.LeftAlt, KeyCode.RightAlt,

			// TODO: will have to handle osx/windows keys the same
			KeyCode.LeftCommand, KeyCode.RightCommand,
			KeyCode.LeftWindows, KeyCode.RightWindows
		};

		List<KeyCode> pressedModifiers = new List<KeyCode> ();
		public List<KeyCode> PressedModifiers {
			get { return pressedModifiers; }
			private set { pressedModifiers = value; }
		}

		public void UpdatePressed () {
			PressedModifiers.Clear ();
			foreach (KeyCode kc in modifiers) {
				if (Input.GetKey (kc))
					PressedModifiers.Add (kc);
			}
		}
	}

	class Hotkey {

		readonly KeyCode key;
		readonly System.Action action;
		readonly List<KeyCode> modifiers;

		public Hotkey (KeyCode key, System.Action action, List<KeyCode> modifiers=null) {
			this.key = key;
			this.action = action;
			this.modifiers = modifiers;
		}

		public void CheckPress (List<KeyCode> pressedModifiers) {

			if (Input.GetKeyDown (key)) {
				if (modifiers != null) {
					foreach (KeyCode kc in modifiers) {
						if (!pressedModifiers.Contains (kc))
							return;	
					}
				}

				action ();
			}
		}
	}

	Modifiers modifiers = new Modifiers ();

	List<Hotkey> hotkeys = new List<Hotkey> () {
		new Hotkey (KeyCode.Period, () => {
			List<Distributor> laborers = ObjectPool.GetActiveObjects<Distributor> ();
			Distributor available = laborers.Find (x => x.Idle);
			if (available != null) {
				SelectionHandler.SelectSingle (available);
			}
		}),
		new Hotkey (KeyCode.L, () => {
			GivingTreeUnit tree = ObjectPool.GetActiveObjects<GivingTreeUnit> ()[0];
			tree.PerformableTasks[typeof (GenerateUnit<Distributor>)].Start ();
		})
	};

	void Update () {
		modifiers.UpdatePressed ();
		foreach (Hotkey k in hotkeys) {
			k.CheckPress (modifiers.PressedModifiers);
		}
	}
}
