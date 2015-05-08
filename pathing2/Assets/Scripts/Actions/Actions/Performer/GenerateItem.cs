using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class GenerateItem<T> : InventoryAction<T> where T : ItemHolder {

		string name = "";
		public override string Name {
			get { 
				if (name == "") {
					string typeName = typeof (T).Name;
					typeName = typeName.Substring (0, typeName.Length-6);
					name = "Generate " + typeName;
				}
				return name;
			}
		}

		public GenerateItem (PerformCondition performCondition=null) : base (-1f, true, true, performCondition) {}
		
		public override void OnEnd () {
			Holder.Add ();
		}
	}
}