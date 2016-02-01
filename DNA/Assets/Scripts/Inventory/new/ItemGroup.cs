using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem {

	public delegate void OnUpdate ();
	public delegate void OnEmpty ();
	public delegate void OnFill ();
	public delegate void OnRemove ();
	public delegate void OnAdd<T> (List<T> items) where T : Item;
	public delegate void OnUpdateCapacity ();

	/// <summary>
	/// An ItemGroup contains InventoryItems. This abstract class is useful for grouping ItemGroups
	/// of different types together, but only ItemGroup<T> should ever inherit from it.
	/// </summary>
	public abstract class ItemGroup {

		/// <summary>
		/// Get the ID. This is useful when finding an ItemGroup in the Inventory.
		/// </summary>
		public abstract string ID { get; }

		protected List<Item> items = new List<Item> ();
		public readonly Inventory Subgroups = new Inventory ();

		/// <summary>
		/// Get the InventoryItems.
		/// </summary>
		public List<Item> Items {
			get { return items; }
		}

		protected Inventory inventory;

		/// <summary>
		/// Get the Inventory that this ItemGroup is contained in.
		/// </summary>
		public Inventory Inventory {
			get { return inventory; }
		}

		int capacity = -1;

		/// <summary>
		/// Get the capacity of the inventory (-1 means no capacity)
		/// </summary>
		public int Capacity {
			get { return capacity; }
			set { 
				capacity = value; 
				if (onUpdateCapacity != null)
					onUpdateCapacity ();
			}
		}

		/// <summary>
		/// Get whether or not a capacity has been set.
		/// </summary>
		public bool HasCapacity {
			get { return Capacity != -1; }
		}

		/// <summary>
		/// Get the total number of items.
		/// </summary>
		public int Count { 
			get { return items.Count; }
		}

		/// <summary>
		/// Returns true if there are no items.
		/// </summary>
		public bool Empty { 
			get { return Count == 0; }
		}

		/// <summary>
		/// Returns true if the group is at capacity.
		/// </summary>
		public bool Full {
			get { return Count == Capacity; }
		}

		/// <summary>
		/// Returns the percentage of the group that has been filled with items.
		/// </summary>
		public float PercentFilled {
			get {
				if (!HasCapacity) {
					Debug.LogWarning ("The ItemGroup " + this + " does not have a capacity and will always return 100% filled.");
					return 1f;
				}
				float capacity = (float)Capacity;
				float count = (float)Count;
				return count / capacity;
			}
		}

		/// <summary>
		/// Called any time an item is added or removed.
		/// </summary>
		public OnUpdate onUpdate;

		/// <summary>
		/// Called any time an item is added or removed.
		/// </summary>
		public OnRemove onRemove;

		/// <summary>
		/// Called when the last item is removed.
		/// </summary>
		public OnEmpty onEmpty;

		/// <summary>
		/// Called when the group reaches capacity.
		/// </summary>
		public OnFill onFill;

		/// <summary>
		/// Called when the capacity is changed.
		/// </summary>
		public OnUpdateCapacity onUpdateCapacity;

		public abstract void Initialize (Inventory inventory);
		public abstract void Set (int count);
		public abstract void Add (int count);
		public abstract Item Add (Item item=null);
		public abstract void Add (List<Item> newItems);
		public abstract void Remove (int count);
		public abstract Item Remove (Item item=null, bool sendUpdate=true);
		public abstract void Fill ();
		public abstract void Clear ();
		public abstract void Transfer (ItemGroup toGroup, Item item=null);
		public abstract bool Contains (Item item);
		protected abstract void SendUpdateMessage ();
		protected abstract void SendEmptyMessage ();
		protected abstract void SendRemoveMessage ();
		protected abstract void SendFillMessage ();
		public abstract void Print ();
	}

	/// <summary>
	/// Contains InventoryItems of type T. Every type of Item should have a corresponding ItemGroup
	/// and all new ItemGroups should inherit from this class.
	/// </summary>
	public class ItemGroup<T> : ItemGroup where T : Item, new () {
		
		public override string ID { get { return ""; } }

		public List<T> MyItems {
			get { return Items.ConvertAll (x => (T)x); }
		}

		/// <summary>
		/// Called any time items are added.
		/// </summary>
		public OnAdd<T> onAdd;

		public ItemGroup (int startCount=0, int capacity=-1) {
			Capacity = capacity;
			Set (startCount);
		}

		/// <summary>
		/// Initialize by setting the Inventory that contains this ItemGroup. There is generally no need
		/// to explicitly use this function because Inventory already calls it whenever an ItemGroup is added.
		/// </summary>
		/// <param name="inventory">The Inventory that contains this ItemGroup.</param>
		public override void Initialize (Inventory inventory) {
			this.inventory = inventory;
		}

		/// <summary>
		/// Set the number of items.
		/// </summary>
		public override void Set (int newCount) {
			if (newCount == Count) return;
			if (Count < newCount) {
				Add (newCount - Count);
			} else {
				Remove (Count - newCount);
			}
		}

		/// <summary>
		/// Add a number of items.
		/// </summary>
		/// <param name="count">The number of items to add.</param>
		public override void Add (int count) {
			List<Item> items = new List<Item> ();
			for (int i = 0; i < count; i ++) {
				items.Add (new T ());
			}
			Add (items);
		}

		/// <summary>
		/// Add an Item
		/// </summary>
		/// <param name="item">The Item to add.</param>
		public override Item Add (Item item=null) {
			if (item == null) item = new T ();
			Add (new List<Item> { item });
			return item;
		}

		/// <summary>
		/// Add a list of InventoryItems.
		/// </summary>
		/// <param name="newItems">A list of InventoryItems to be added.</param>
		public override void Add (List<Item> newItems) {
			
			List<Item> addedItems = new List<Item> ();
			while (newItems.Count > 0 && (!HasCapacity || Count < Capacity)) {
				Item newItem = newItems[0];
				if (newItem != null) {
					newItem.Initialize (Inventory, this);
					addedItems.Add (newItem);
				}
				newItems.RemoveAt (0);
			}
			items.AddRange (addedItems);

			SendAddMessage (addedItems.ConvertAll (x => (T)x));
			SendUpdateMessage ();
			if (Count == Capacity)
				SendFillMessage ();
		}

		/// <summary>
		/// Removes a number of items.
		/// </summary>
		/// <param name="count">The number of items to remove</param>
		public override void Remove (int count) {
			if (count <= 0) return;
			for (int i = 0; i < count-1; i ++) {
				Remove (null, false);
			}
			Remove (null, true);
		}

		/// <summary>
		/// Removes the given Item.
		/// </summary>
		/// <param name="item">The Item to remove.</param>
		/// <returns>The removed Item (null if the item was not in the group)</returns>
		public override Item Remove (Item item=null, bool sendUpdate=true) {
			
			if (Empty) return null;
			Item removedItem = items[0] ?? item;
			if (item == null) {
				items.RemoveAt (0);
			} else {
				items.Remove (item);
			}

			SendRemoveMessage ();
			if (sendUpdate) SendUpdateMessage ();
			if (Empty) SendEmptyMessage ();
			
			return removedItem;
		}

		/// <summary>
		/// Fills group to capacity, if a capacity has been set.
		/// </summary>
		public override void Fill () {
			if (!HasCapacity) {
				Debug.LogWarning ("The ItemGroup " + this + " cannot be filled because a capacity has not been set.");
				return;
			}
			Set (Capacity);
		}

		/// <summary>
		/// Removes all items.
		/// </summary>
		public override void Clear () {
			items.Clear ();
			SendUpdateMessage ();
			SendEmptyMessage ();
		}

		/// <summary>
		/// Transfers an item from this ItemGroup to another ItemGroup.
		/// </summary>
		/// <param name="toGroup">ItemGroup to send the item to.</param>
		/// <param name="item">The Item to transfer.</param>
		public override void Transfer (ItemGroup toGroup, Item item=null) {
			Item i = Remove (item);
			toGroup.Add (i);
		}

		/// <summary>
		/// Checks if this ItemGroup contains a specfic Item.
		/// </summary>
		/// <param name="item">The Item to search for.</param>
		/// <returns>True if the item was found.</returns>
		public override bool Contains (Item item) {
			return Items.Contains (item);
		}

		protected override void SendRemoveMessage () {
			if (onRemove != null) onRemove ();
		}

		protected override void SendUpdateMessage () {
			if (onUpdate != null) onUpdate ();
		}

		protected override void SendEmptyMessage () {
			if (onEmpty != null) onEmpty ();
		}

		protected void SendAddMessage (List<T> items) {
			if (onAdd != null) onAdd (items);
		}

		protected override void SendFillMessage () {
			if (onFill != null) onFill ();
		}

		/// <summary>
		/// Prints every Item to the console.
		/// </summary>
		public override void Print () {
			foreach (Item item in Items) {
				Debug.Log (item.Name);
			}
		}
	}
}