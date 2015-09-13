using UnityEngine;
using System.Collections;
using DNA.InventorySystem;

namespace DNA.Tasks {

	public class CollectItemTest<T> : CollectItem<T> where T : ItemHolder {}
}