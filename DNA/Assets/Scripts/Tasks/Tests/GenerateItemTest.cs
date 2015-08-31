using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {
	public class GenerateItemTest<T> : GenerateItem<T> where T : ItemHolder {}
}