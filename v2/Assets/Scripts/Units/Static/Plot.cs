﻿using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {
	
	public class Plot : StaticUnit, IActionAcceptor {

		public override string Name {
			get { return "Plot"; }
		}
		
		public AcceptableActions AcceptableActions { get; private set; }

		void Awake () {

			Inventory = new Inventory ();
			Inventory.Add (new MilkshakeHolder (0, 0, CreateBuilding));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("DeliverMilkshake", new AcceptDeliverItem<MilkshakeHolder> ());

			InventoryDrawer.Create (StaticTransform.transform, Inventory);
		}

		void CreateBuilding () {
			Debug.Log ("heard");
		}
	}
}