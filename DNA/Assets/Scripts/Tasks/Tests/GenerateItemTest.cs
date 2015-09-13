using UnityEngine;
using System.Collections;
using DNA.InventorySystem;

namespace DNA.Tasks {
	public class GenerateItemTest<T> : GenerateItem<T> where T : ItemHolder {}
}