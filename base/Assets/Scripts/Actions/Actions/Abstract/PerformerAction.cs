using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public abstract class PerformerAction : Action {

		float duration;
		public float Duration {
			get { return duration; }
		}

		bool autoRepeat = false;

		public IActionPerformer Performer { get; set; }
		
		Inventory inventory = null;
		protected Inventory Inventory {
			get {
				if (inventory == null && Performer is IInventoryHolder) {
					IInventoryHolder holder = Performer as IInventoryHolder;
					inventory = holder.Inventory;
				}
				if (inventory == null) {
					Debug.LogError ("ActionPerformer does not implement IInventoryHolder");
				}
				return inventory;
			}
		}

		public PerformerAction (float duration, bool autoStart=false, bool autoRepeat=false) {
			this.duration = duration;
			this.autoRepeat = autoRepeat;
			if (autoStart) Start ();
		}

		public virtual void Start () {
			ActionHandler.instance.StartAction (this);
		}

		public virtual void Perform (float progress) {}

		public virtual void End () {
			if (autoRepeat) Start ();
		}
	}
}