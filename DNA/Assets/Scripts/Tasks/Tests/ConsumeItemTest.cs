using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {
	public class ConsumeItemTest<T> : ConsumeItem<T> where T : ItemGroup {}
}