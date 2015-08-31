using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {
	public class ConsumeItemTest<T> : ConsumeItem<T> where T : ItemHolder {}
}