using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem {

	/// <summary>
	/// The Inventory contains ItemGroups.
	/// </summary>
	public class Inventory {

		readonly Dictionary<string, ItemGroup> groups = new Dictionary<string, ItemGroup> ();

		/// <summary>
		/// Get the ItemGroups.
		/// </summary>
		public Dictionary<string, ItemGroup> Groups {
			get { return groups; }
		}

		/// <summary>
		/// Get the number of groups.
		/// </summary>
		public int GroupCount {
			get { return groups.Count; }
		}

		/// <summary>
		/// Get an ItemGroup using bracket notation.
		/// </summary>
		public ItemGroup this[string id] {
			get { 
				try {
					return Groups[id];
				}	 
				catch (System.Exception e) {
					throw new System.Exception("Unable to find ItemGroup with the ID '" + id + "'\n" + e);
				} 
			}
		}

		/// <summary>
		/// Get an ItemGroup by type.
		/// </summary>
		public T Get<T> () where T : ItemGroup {
			foreach (var group in Groups) {
				if (group.Key.GetType () == typeof (T))
					return group.Key as T;
			}
			return null;
		}

		/// <summary>
		/// Add an ItemGroup.
		/// </summary>
		/// <param name="group">The ItemGroup to add.</param>
		public void Add (ItemGroup group) {
			group.Initialize (this);
			groups.Add (group.ID, group);
		}
	}
}