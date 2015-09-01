using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {
	public class DeliverItemTest<T> : DeliverItem<T> where T : ItemHolder {}
}