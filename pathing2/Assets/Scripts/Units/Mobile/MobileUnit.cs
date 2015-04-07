using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathing;
using GameActions;
using GameInput;

namespace Units {

	public class MobileUnit : Unit, IActionPerformer, IBinder {

		MobileUnitTransform mobileTransform;
		public MobileUnitTransform MobileTransform {
			get {
				if (mobileTransform == null) {
					mobileTransform = unitTransform as MobileUnitTransform;
				}
				return mobileTransform;
			}
		}

		Path path = null;
		public Path Path {
			get { 
				if (path == null) {
					IPathable pathable = MobileTransform as IPathable; 
					path = pathable.Path;
				}
				return path;
			}
		}

		public PerformableActions PerformableActions { get; protected set; }
		public IActionAcceptor BoundAcceptor { get; private set; }

		public virtual void OnRelease () {
			StartMovingOnPath ();
		}

		public virtual void OnBindActionable (IActionAcceptor acceptor) {
			BoundAcceptor = acceptor;
			ActionHandler.instance.Bind (this);
		}

		public virtual void OnEndActions () {
			StartMovingOnPath ();
		}

		void StartMovingOnPath () {
			MobileTransform.StartMovingOnPath ();
			PerformableActions.DisableAll ();
			EnableAcceptedActions (GetAcceptedActionsOnPath ());
			//PerformableActions.Print ();
		}

		// TODO: Have this return Dictionary<string, AcceptorAction> so that EnableAcceptedActions can check
		// to make sure that it isn't pairing actions with other actions on the same unit
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

		public virtual void OnDragRelease (Unit unit) {}
	}
}