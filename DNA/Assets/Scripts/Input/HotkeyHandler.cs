using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Units;
using DNA.Tasks;
using DNA.InputSystem;

namespace DNA {

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
				List<Laborer> laborers = ObjectPool.GetActiveInstances<Laborer> ();
				Laborer available = laborers.Find (x => x.Idle);
				if (available != null) {
					SelectionHandler.SelectSingle (available);
				}
			}),
			new Hotkey (KeyCode.L, () => {
				GivingTreeUnit tree = ObjectPool.GetActiveInstances<GivingTreeUnit> ()[0];
				tree.PerformableTasks[typeof (GenerateLaborer)].Start ();
			}),
			new Hotkey (KeyCode.Escape, () => {
				SelectionHandler.Clear ();
				TaskPen.Remove ();
			}),
			new Hotkey (KeyCode.R, () => {
				TaskPen.Remove ();
				TaskPen.Set (Player.Instance.PerformableTasks[typeof (ConstructRoad)]);
			}),
			new Hotkey (KeyCode.U, () => {
				TaskPen.Remove ();
				TaskPen.Set (Player.Instance.PerformableTasks[typeof (ConstructUnit<University>)]);
			}),
			new Hotkey (KeyCode.M, () => {
				TaskPen.Remove ();
				TaskPen.Set (Player.Instance.PerformableTasks[typeof (ConstructUnit<MilkshakePool>)]);
			}),
			new Hotkey (KeyCode.C, () => {
				TaskPen.Remove ();
				TaskPen.Set (Player.Instance.PerformableTasks[typeof (ConstructUnit<CollectionCenter>)]);
			}),
			new Hotkey (KeyCode.H, () => {
				TaskPen.Remove ();
				TaskPen.Set (Player.Instance.PerformableTasks[typeof (ConstructUnit<House>)]);
			}),
			new Hotkey (KeyCode.P, () => {
				TaskPen.Remove ();
				TaskPen.Set (Player.Instance.PerformableTasks[typeof (ConstructUnit<Apartment>)]);
			})
			#if UNITY_EDITOR
			,
			new Hotkey (KeyCode.Equals, () => {
				Player.Instance.Inventory["Coffee"].Add (200);
			}),
			new Hotkey (KeyCode.Minus, () => {
				Player.Instance.Inventory["Milkshakes"].Add (200);
			})
			#endif
		};

		void Update () {
			modifiers.UpdatePressed ();
			foreach (Hotkey k in hotkeys) {
				k.CheckPress (modifiers.PressedModifiers);
			}
		}
	}
}