using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {
	public class DeliverItemTest<T> : DeliverItem<T> where T : ItemGroup {}
}