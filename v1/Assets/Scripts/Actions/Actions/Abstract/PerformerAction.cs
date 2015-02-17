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

		protected AcceptCondition AcceptCondition { get; private set; }

		public PerformerAction (float duration, bool autoStart=false, bool autoRepeat=false) {
			this.duration = duration;
			this.autoRepeat = autoRepeat;
			if (autoStart) Start ();
		}

		public void Bind (AcceptCondition acceptCondition) {
			AcceptCondition = acceptCondition;
		}

		public virtual void Start () {
			ActionHandler.instance.StartAction (this);
		}

		public virtual void Perform (float progress) {}

		public void End () {
			OnEnd ();
			if (autoRepeat) Start ();
		}

		public virtual void OnEnd () {}
	}
}