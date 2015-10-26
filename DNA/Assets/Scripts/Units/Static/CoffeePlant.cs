#define SHORTLIFE
using UnityEngine;
using System.Collections;
//using DNA.InventorySystem;
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
			//Inventory.Add (new CoffeeHolder (20, 0));
			Inventory.Add (new CoffeeGroup (0, 20));
			//Inventory.Add (new YearHolder (350, 0));
			Inventory.Add (new YearGroup (0, 350));
			//Inventory.Get<CoffeeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);

			//AcceptableTasks.Add (new AcceptCollectItem<CoffeeHolder> ());
			AcceptableTasks.Add (new AcceptCollectItem<CoffeeGroup> ());

			//PerformableTasks.Add (new GenerateItem<CoffeeHolder> ());
			PerformableTasks.Add (new GenerateItem<CoffeeGroup> ());
			//PerformableTasks.Add (new ConsumeItem<YearHolder> ()).onComplete += (PerformerTask t) => { 
			PerformableTasks.Add (new ConsumeItem<YearGroup> ()).onComplete += (PerformerTask t) => { 
				if (Element != null)
					Element.State = DevelopmentState.Abandoned; 
				//PerformableTasks[typeof (GenerateItem<CoffeeHolder>)].Stop ();
				PerformableTasks[typeof (GenerateItem<CoffeeGroup>)].Stop ();
			};

		}

		protected override void OnEnable () {
			base.OnEnable ();
			//Inventory.Get<YearHolder> ().Initialize ();
			//Inventory.Get<CoffeeHolder> ().Initialize (2);
			Inventory["Years"].Clear ();
			Inventory["Coffee"].Set (2);
			#if SHORTLIFE
			if (!shortLife) {
				Inventory.Get<YearGroup> ().Remove (25);
				//Inventory.Get<YearHolder> ().Remove (25);
				shortLife = true;
			}
			#endif
			PerformableTasks[typeof (ConsumeItem<YearGroup>)].Start ();
			PerformableTasks[typeof (GenerateItem<CoffeeGroup>)].Start ();
		}

		protected override void OnDisable () {
			base.OnDisable ();
			PerformableTasks[typeof (GenerateItem<CoffeeGroup>)].Stop ();
			PerformableTasks[typeof (ConsumeItem<YearGroup>)].Stop ();
		}

		protected override void OnSetFertility (int tier) {
			Inventory["Coffee"].Capacity = (int)(20 * Fertility.Multipliers[tier]);
			//Inventory["Coffee"].Initialize (2);
			Inventory["Coffee"].Set (2);
		}
	}
}