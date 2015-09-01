using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {

	public class CollectItemTest<T> : CollectItem<T> where T : ItemHolder {}
}