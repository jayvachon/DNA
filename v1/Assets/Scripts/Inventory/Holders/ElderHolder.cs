using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory {

	public class ElderHolder : ItemHolder<ElderItem> {

		public override string Name {
			get { return "Elders"; }
		}

		// TODO: create a class that handles this + the sickness enum in ElderCondition
		float sickThreshold = 0.5f;
		
		public ElderHolder (int capacity, int startCount) : base (capacity) {
			AddNew (startCount);
		}

		void AddNew (int count) {
			if (count == 0)
				return;
			for (int i = 0; i < count; i ++) {
				Add (new ElderItem ());
			}
		}

		public bool HasSick () {
			foreach (ElderItem elder in Items) {
				if (elder.Health < sickThreshold) {
					return true;
				}
			}
			return false;
		}

		public bool HasHealthy () {
			foreach (ElderItem elder in Items) {
				if (elder.Health >= sickThreshold) {
					return true;
				}
			}
			return false;
		}

		// TODO: use a lambda
		public List<Item> RemoveSick (int amount) {
			List<Item> temp = new List<Item> (0);
			while (Count > 0 && amount > 0) {
				if (items[0].Health < sickThreshold) {
					temp.Add (items[0]);
				}
				items.RemoveAt (0);
				amount --;
			}
			
			// return items that were removed
			return temp; 
		}

		// TODO: use a lambda
		public List<Item> RemoveHealthy (int amount) {
			List<Item> temp = new List<Item> (0);
			while (Count > 0 && amount > 0) {
				if (items[0].Health >= sickThreshold) {
					temp.Add (items[0]);
				}
				items.RemoveAt (0);
				amount --;
			}
			
			// return items that were removed
			return temp; 
		}

		// TODO: use a lambda
		public void TransferSick (ElderHolder senderHolder, int amount=-1) {
			if (senderHolder is ElderHolder) {
				if (amount == -1) amount = Capacity;
				ElderHolder sender = senderHolder as ElderHolder;
				List<Item> items = sender.RemoveSick (amount);
				List<Item> overflow = Add (items);
				sender.Add (overflow);
			}
		}

		// TODO: use a lambda
		public void TransferHealthy (ElderHolder senderHolder, int amount=-1) {
			if (senderHolder is ElderHolder) {
				if (amount == -1) amount = Capacity;
				ElderHolder sender = senderHolder as ElderHolder;
				List<Item> items = sender.RemoveHealthy (amount);
				List<Item> overflow = Add (items);
				sender.Add (overflow);
			}
		}

		public override void Print () {
			foreach (ElderItem item in Items) {
				ElderItem elder = item as ElderItem;
				elder.Print ();
			}
		}
	}
}