using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;
using Pathing;

namespace Units {

	public class Distributor : MobileUnit, IActionPerformer {

		new string name = "Distributor";
		public override string Name {
			get { return name; }
		}

		public PerformableActions PerformableActions { get; private set; }

		AgeManager ageManager = new AgeManager ();

		void Awake () {

			Inventory = new Inventory ();
			Inventory.Add (new MilkHolder (5, 0));
			Inventory.Add (new IceCreamHolder (3, 0));
			Inventory.Add (new MilkshakeHolder (3, 0));
			Inventory.Add (new ElderHolder (2, 0));

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("CollectMilk", new CollectItem<MilkHolder> (0.5f));
			PerformableActions.Add ("DeliverMilk", new DeliverItem<MilkHolder> (0.5f));
			PerformableActions.Add ("CollectIceCream", new CollectItem<IceCreamHolder> (1));
			PerformableActions.Add ("DeliverIceCream", new DeliverItem<IceCreamHolder> (1));
			PerformableActions.Add ("CollectMilkshake", new CollectItem<MilkshakeHolder> (2));
			PerformableActions.Add ("DeliverMilkshake", new DeliverItem<MilkshakeHolder> (2));
			PerformableActions.Add ("CollectElder", new CollectItem<ElderHolder> (2));
			PerformableActions.Add ("DeliverElder", new DeliverItem<ElderHolder> (2));
			PerformableActions.DisableAll ();

			//Debug.Log (PerformableActions.Get ("CollectMilk").RequiredPair);
		}

		void Start () {
			ageManager.BeginAging (OnAge, OnRetirement);
		}

		void OnAge (float progress) {
			float p = Mathf.Clamp01 (Mathf.Abs (progress - 1));
			Path.Speed = Path.PathSettings.maxSpeed * Mathf.Sqrt(-(p - 2) * p);
		}

		void OnRetirement () {
			Path.Speed = 0;
			MobileTransform.StopMovingOnPath ();
			name = "Elder";
			unitInfoContent.Refresh ();
			Path.Enabled = false;
		}

		// having this happen on release means that the distributor won't know if an action acceptor's accepted
		// actions have changed (e.g. if the distributor is on a path w/ a plot, and the player decides to have the plot birth a unit)
		public override void OnRelease () {
			MobileTransform.StartMovingOnPath ();
			PerformableActions.DisableAll ();
			EnableAcceptedActions (GetAcceptedActionsOnPath ());
		}

		List<string> GetAcceptedActionsOnPath () {
			List<string> acceptedActions = new List<string> ();
			foreach (PathPoint point in Path.Points.Points) {
				acceptedActions.AddRange (
					PerformableActions.GetAcceptedActions (point.StaticUnit as IActionAcceptor)
				);
			}
			return acceptedActions;
		}

		/**
		 *	1. when a path is formed, get each path point and cast to IActionAcceptor
		 *	2. create a list of actions that the distributor can perform with the acceptor
		 *	3. check for paired actions (collect/deliver) and if a pair exists in the path,
		 *		enable the performer action
		 */

		void EnableAcceptedActions (List<string> acceptedActions) {
			
			Dictionary<string, System.Type> unpairedActions = new Dictionary<string, System.Type> ();
			
			for (int i = 0; i < acceptedActions.Count; i ++) {
				
				string actionName 			= acceptedActions[i];
				PerformerAction action 		= PerformableActions.Get (actionName);
				System.Type requiredPair 	= action.RequiredPair;
				
				// Enable the action if it doesn't require a pair
				if (requiredPair == null) {
					PerformableActions.Enable (actionName);
					continue;
				} 

				// Add the action to the dictionary if the dictionary is empty
				if (unpairedActions.Count == 0) {
					unpairedActions.Add (actionName, action.GetType ());
					continue;
				} 

				// If the dictionary isn't empty, check if the pair exists in the dictionary
				// and enable both actions if it does
				string pair = "";
				foreach (var unpairedAction in unpairedActions) {
					if (unpairedAction.Value == requiredPair) {
						pair = unpairedAction.Key;
						PerformableActions.Enable (pair);
						PerformableActions.Enable (actionName);
						break;
					}
				}

				// If no pair was found, at the action to the dictionary
				if (pair == "") {
					System.Type t;
					if (!unpairedActions.TryGetValue (actionName, out t)) {
						unpairedActions.Add (actionName, action.GetType ());
					}
				} else {
					
					// But if a pair WAS found, remove it from the dictionary
					unpairedActions.Remove (pair);
				}
			}
		}

		public override void OnDragRelease (Unit unit) {
			if (ageManager.Retired) {
				House house = unit as House;
				if (house != null) {
					house.Inventory.AddItem<ElderHolder> (new ElderItem ());
					ObjectCreator.Instance.Destroy<Distributor> (transform);
				}
			}
		}

		public override void OnCreate () {
			Path.Enabled = true;
		}
	}
}