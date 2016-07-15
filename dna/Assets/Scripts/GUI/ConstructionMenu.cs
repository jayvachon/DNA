using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DNA.Tasks;
using DNA.Units;
using DNA.InputSystem;

namespace DNA {

	public class ConstructionMenu : MonoBehaviour {

		public Transform topPanel;
		public Button[] buttons;

		Dictionary<string, PerformerTask[]> categories = new Dictionary<string, PerformerTask[]> ();

		void Awake () {

			SetTopPanelActive (false);

			// Setup road button
			TaskButton b = buttons[0].gameObject.AddComponent<TaskButton> ();
			b.Init (PlayerTask (typeof (ConstructRoad)), false);
			b.AddButtonListener (() => { SetTopPanelActive (false); });

			// Setup categories
			categories.Add ("residential", new PerformerTask[] {
				PlayerTask (typeof (ConstructUnit<House>)),
				PlayerTask (typeof (ConstructUnit<Apartment>))
			});
			categories.Add ("resource", new PerformerTask[] {
				PlayerTask (typeof (ConstructUnit<MilkshakePool>)),
				PlayerTask (typeof (ConstructUnit<CollectionCenter>))
			});
			categories.Add ("civic", new PerformerTask[] {
				PlayerTask (typeof (ConstructUnit<University>)),
				PlayerTask (typeof (ConstructUnit<Turret>))
			});

			buttons[1].onClick.AddListener (() => { SetCategory ("residential"); });
			buttons[2].onClick.AddListener (() => { SetCategory ("resource"); });
			buttons[3].onClick.AddListener (() => { SetCategory ("civic"); });

			SelectionHandler.onUpdateSelection += OnUpdateSelection;
		}

		PerformerTask PlayerTask (System.Type taskType) {
			return Player.Instance.PerformableTasks[taskType];
		}

		void SetCategory (string id) {
			SetTopPanelActive (true);
			ObjectPool.DestroyChildren<TaskButton> (topPanel);
			PerformerTask[] tasks = categories[id];
			foreach (PerformerTask task in tasks) {
				TaskButton2 t = ObjectPool.Instantiate<TaskButton2> ();
				t.transform.SetParent (topPanel);
				t.transform.Reset ();
				t.Init (task);
				t.ButtonText.color = (task.Enabled) ? Palette.Black : Palette.Grey;
				t.Button.interactable = task.Enabled;
			}
		}

		void SetTopPanelActive (bool active) {
			topPanel.gameObject.SetActive (active);
		}

		void OnUpdateSelection (List<ISelectable> selected) {
			SetTopPanelActive (false);
		}
	}
}