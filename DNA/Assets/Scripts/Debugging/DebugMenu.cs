using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;
using DNA.Paths;
using DNA.Tasks;
using DNA.Units;

namespace DNA {

	public class DebugMenu : MonoBehaviour {

		public List<Button> buttons;

		void OnEnable () {
			SelectionHandler.onUpdateSelection += OnUpdateSelection;
			DisableButtons ();
		}

		void OnDisable () {
			SelectionHandler.onUpdateSelection -= OnUpdateSelection;
		}

		void OnUpdateSelection (List<ISelectable> selectables) {

			List<ConstructionSite> constructionSites = selectables
				.FindAll (x => (x is ConstructionSite))
				.ConvertAll (x => x as ConstructionSite);

			if (constructionSites.Count > 0) {
				DisableButtons ();
				EnableButton ("Construct", () => {
					foreach (ConstructionSite c in constructionSites) {
						c.Inventory["Labor"].Clear ();
					}
					constructionSites.Clear ();
				});
			}
		}

		void EnableButton (string text, UnityAction onPress) {
			Button b = buttons.Find (x => !x.gameObject.activeSelf);
			b.gameObject.SetActive (true);
			b.transform.GetChild (0).GetComponent<Text> ().text = text;
			b.onClick.RemoveAllListeners ();
			b.onClick.AddListener (onPress);
			b.onClick.AddListener (() => b.gameObject.SetActive (false));
		}

		void DisableButtons () {
			foreach (Button b in buttons) {
				b.gameObject.SetActive (false);
			}
		}
	}
}