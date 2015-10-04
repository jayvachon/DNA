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
			
			Inventory = new Inventory (this);
			Inventory.Add (new CoffeeHolder (20, 0));
			Inventory.Add (new YearHolder (150, 0));
			Inventory.Get<CoffeeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);

			AcceptableTasks.Add (new AcceptCollectItem<CoffeeHolder> ());

			PerformableTasks.Add (new GenerateItem<CoffeeHolder> ());
			PerformableTasks.Add (new ConsumeItem<YearHolder> ()).onComplete += OnDie;

		}

		//public override void OnPoolCreate () {
		protected override void OnEnable () {
			//base.OnPoolCreate ();
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

		void OnDie (PerformerTask task) {
			Destroy<CoffeePlant> ();
		}
	}
}