using UnityEngine;
using System.Collections;
using DNA.InputSystem;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class CoffeePlant : StaticUnit, ITaskPerformer {

		public override string Name {
			get { return "Coffee Plant"; }
		}

		public override string Description {
			get { return "Deliver coffee to the Giving Tree to create more Laborers."; }
		}
		
		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
					PerformableTasks.Add (new GenerateItem<CoffeeGroup> ());
					PerformableTasks.Add (new GenerateItem<YearGroup> ()).onComplete += (PerformerTask t) => { 
						if (Element != null)
							Element.State = DevelopmentState.Abandoned; 
						PerformableTasks[typeof (GenerateItem<CoffeeGroup>)].Stop ();
					};
				}
				return performableTasks;
			}
		}

		void Awake () {
			unitRenderer.SetColors (new Color (0.204f, 0.612f, 0.325f));
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new CoffeeGroup (0, 20));
			i.Add (new YearGroup (0, 300));
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<CoffeeGroup> ());				
		}

		protected override void OnEnable () {
			base.OnEnable ();
			Inventory["Years"].Clear ();
			Inventory["Coffee"].Set (2);
			PerformableTasks[typeof (GenerateItem<YearGroup>)].Start ();
			PerformableTasks[typeof (GenerateItem<CoffeeGroup>)].Start ();
		}

		protected override void OnDisable () {
			base.OnDisable ();
			PerformableTasks[typeof (GenerateItem<CoffeeGroup>)].Stop ();
			PerformableTasks[typeof (GenerateItem<YearGroup>)].Stop ();
		}

		protected override void OnSetFertility (int tier) {
			Inventory["Coffee"].Capacity = (int)(20 * Fertility.Multipliers[tier]);
			Inventory["Coffee"].Set (2);
		}
	}
}