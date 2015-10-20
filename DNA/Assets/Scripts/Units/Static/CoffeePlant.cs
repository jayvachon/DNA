#define SHORTLIFE
using UnityEngine;
using System.Collections;
using DNA.InventorySystem;
using DNA.InputSystem;
using DNA.Tasks;

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
				}
				return performableTasks;
			}
		}

		#if SHORTLIFE
		static bool shortLife = false;
		#endif

		void Awake () {
			
			unitRenderer.SetColors (new Color (0.204f, 0.612f, 0.325f));

			Inventory = new Inventory (this);
			Inventory.Add (new CoffeeHolder (20, 0));
			Inventory.Add (new YearHolder (250, 0));
			Inventory.Get<CoffeeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);

			AcceptableTasks.Add (new AcceptCollectItem<CoffeeHolder> ());

			PerformableTasks.Add (new GenerateItem<CoffeeHolder> ());
			PerformableTasks.Add (new ConsumeItem<YearHolder> ()).onComplete += (PerformerTask t) => { 
				if (Element != null)
					Element.State = DevelopmentState.Abandoned; 
				PerformableTasks[typeof (GenerateItem<CoffeeHolder>)].Stop ();
			};

		}

		protected override void OnEnable () {
			base.OnEnable ();
			Inventory.Get<YearHolder> ().Initialize ();
			Inventory.Get<CoffeeHolder> ().Initialize (2);
			#if SHORTLIFE
			if (!shortLife) {
				Inventory.Get<YearHolder> ().Remove (25);
				shortLife = true;
			}
			#endif
			PerformableTasks[typeof (DNA.Tasks.ConsumeItem<YearHolder>)].Start ();
			PerformableTasks[typeof (DNA.Tasks.GenerateItem<CoffeeHolder>)].Start ();
		}

		protected override void OnDisable () {
			base.OnDisable ();
			PerformableTasks[typeof (GenerateItem<CoffeeHolder>)].Stop ();
			PerformableTasks[typeof (ConsumeItem<YearHolder>)].Stop ();
		}

		protected override void OnSetFertility (int tier) {
			Inventory["Milkshakes"].Capacity = (int)(20 * Fertility.Multipliers[tier]);
			Inventory["Milkshakes"].Initialize (2);
		}
	}
}