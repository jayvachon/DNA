using UnityEngine;
using System.Collections;
using DNA.Tasks;
using DNA.InputSystem;
using InventorySystem;

namespace DNA {

	public class Levee : MBRefs, ISelectable, IInventoryHolder {

		const float WallHeight = 1f;
		const float WallWidth = 2f;

		public Inventory Inventory {
			get { return Player.Instance.Inventory; }
		}
		
		public float Height {
			get { return MyTransform.localScale.y; }
			set { MyTransform.SetLocalScaleY (value); }
		}

		public readonly RegularPolygon edges = new RegularPolygon (5, 75f);
		readonly float startHeight = 1f;

		void Awake () {
			Upgrades.Instance.AddListener<LeveeHeight> (
				(LeveeHeight u) => { Height = u.CurrentValue; }
			);
			Init ();
		}

		void Init () {

			for (int i = 0; i < edges.SideCount; i ++) {
				LeveeWall w = ObjectPool.Instantiate<LeveeWall> ();
				w.Levee = this;
				Transform t = w.transform;
				t.position = edges.Positions[i];
				edges.ApplyAngleY (t, i);
				t.SetParent (transform);
				t.SetLocalPositionY (0.5f);
				t.localScale = new Vector3 (
					edges.SideLength + 1.25f,
					WallHeight,
					WallWidth
				);
			}
		}

		#region ISelectable implementation
		SelectSettings selectSettings;
		public SelectSettings SelectSettings {
			get { 
				if (selectSettings == null) {
					selectSettings = new SelectSettings ();
				}
				return selectSettings;
			}
		}
		public virtual void OnSelect () {}
		public virtual void OnUnselect () {}
		#endregion
	}
}