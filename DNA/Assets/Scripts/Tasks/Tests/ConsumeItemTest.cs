using UnityEngine;
using System.Collections;
using DNA.InventorySystem;

namespace DNA.Tasks {
	public class ConsumeItemTest<T> : ConsumeItem<T> where T : ItemHolder {}
}