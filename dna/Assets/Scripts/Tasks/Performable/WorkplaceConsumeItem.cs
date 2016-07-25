using UnityEngine;
using System.Collections;
using InventorySystem;
using DNA.Units;

namespace DNA.Tasks {

	public class WorkplaceConsumeItem<T> : ConsumeItem<T> where T : ItemGroup {

		public override bool Enabled {
			get { return Workplace.Accessible && Workplace.Efficiency > 0f && !Group.Empty; }
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

		public override float Duration {
			get { return EfficiencyManager.Instance.GetRate (settings.Duration); }
		}
	}
}