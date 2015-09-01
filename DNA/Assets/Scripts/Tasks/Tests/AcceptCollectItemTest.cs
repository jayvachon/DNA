using UnityEngine;
using System.Collections;
using GameInventory;

namespace DNA.Tasks {
	public class AcceptCollectItemTest<T> : AcceptCollectItem<T> where T : ItemHolder {}
}