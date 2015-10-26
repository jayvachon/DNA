using UnityEngine;
using System.Collections;
using InventorySystem;

namespace DNA.Tasks {
	public class GenerateItemTest<T> : GenerateItem<T> where T : ItemGroup {}
}