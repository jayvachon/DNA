using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {

	public class CollectItemTest<T> : CollectItem<T> where T : ItemGroup {}
}