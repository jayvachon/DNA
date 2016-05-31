using UnityEngine;
using System.Collections;
using InventorySystem;
using DNA.Units;

namespace DNA.Tasks {

	public class WorkplaceDeliverItem<T> : CollectItem<T> where T : ItemGroup {

		public override bool Enabled {
			get { return Workplace.Accessible && !Group.Full; }
		}

		IWorkplace Workplace {
			get {
				try {
					return (IWorkplace)Performer;
				} catch (System.InvalidCastException e) {
					throw new System.Exception ("Only TaskPerformers that implement the IWorkplace interface can use the WorkplaceCollectItem task\n" + e);
				}
			}
		}

		public WorkplaceDeliverItem (string symbolOverride="") : base (symbolOverride) {}
	}
}