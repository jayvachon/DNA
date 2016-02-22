using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class TaskIndicator : MBRefs {

		Dictionary<System.Type, string> renderers = new Dictionary<System.Type, string> () {
			{ typeof (CollectItem<CoffeeGroup>), "CoffeePlantRenderer" },
			{ typeof (DeliverItem<CoffeeGroup>), "CoffeePlantRenderer" },
			{ typeof (CollectItem<MilkshakeGroup>), "DerrickRenderer" },
			{ typeof (DeliverItem<MilkshakeGroup>), "DerrickRenderer" },
			{ typeof (CollectItem<LaborGroup>), "ConstructionSiteRenderer" },
			{ typeof (CollectItem<HappinessGroup>), "FlowerRenderer" }
		};

		UnitRenderer currentTask;

		public void SetTask (PerformerTask task) {
			RemoveTask ();
			currentTask = ObjectPool.Instantiate (renderers[task.GetType ()]).GetComponent<UnitRenderer> ();
			currentTask.Parent = MyTransform;
			currentTask.LocalPosition = Vector3.zero;

			Vector3 originalScale = currentTask.LocalScale;
			currentTask.LocalScale = new Vector3 (
				originalScale.x * 0.3f, 
				originalScale.y * 0.3f, 
				originalScale.z * 0.3f);
		}

		public void RemoveTask () {
			if (currentTask != null) {
				ObjectPool.Destroy<UnitRenderer> (currentTask);
			}
		}
	}
}