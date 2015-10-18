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
				Element.State = DevelopmentState.Abandoned; 
				PerformableTasks[typeof (GenerateItem<CoffeeHolder>)].Stop ();
			};

		}

		protected override void OnEnable () {
			base.OnEnable ();
			Inventory.Get<YearHolder> ().Initialize ();
			#if SHORTLIFE
			if (!shortLife) {
				Inventory.Get<YearHolder> ().Remove (25);
				shortLife = true;
			}
			#endif
			PerformableTasks[typeof (DNA.Tasks.ConsumeItem<YearHolder>)].Start ();
		}
	}
}