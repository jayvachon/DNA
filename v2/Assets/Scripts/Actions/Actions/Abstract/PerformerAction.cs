using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public abstract class PerformerAction : Action {

		bool autoRepeat = false;
		float duration;
		public float Duration {
			get { return duration; }
		}

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

		public IActionPerformer Performer { get; set; }
		protected AcceptCondition AcceptCondition { get; private set; }
		protected PerformCondition PerformCondition { get; private set; }

		public PerformerAction (float duration, bool autoStart=false, bool autoRepeat=false, PerformCondition performCondition=null) {
			this.duration = duration;
			this.autoRepeat = autoRepeat;
			this.PerformCondition = performCondition;
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
			if (PerformCondition == null || PerformCondition.Performable) {
				OnEnd ();
			}
			if (autoRepeat) {
				Start ();
			}
		}

		public virtual void OnEnd () {}
	}
}