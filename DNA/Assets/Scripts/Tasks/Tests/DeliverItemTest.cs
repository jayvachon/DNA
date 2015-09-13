using UnityEngine;
using System.Collections;
using DNA.InventorySystem;

namespace DNA.Tasks {
	public class DeliverItemTest<T> : DeliverItem<T> where T : ItemHolder {}
}