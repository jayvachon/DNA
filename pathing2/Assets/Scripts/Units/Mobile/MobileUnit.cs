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
		}

		List<KeyValuePair<string, IActionAcceptor>> GetAcceptedActionsOnPath () {
			
			List<KeyValuePair<string, IActionAcceptor>> acceptedActions = 
				new List<KeyValuePair<string, IActionAcceptor>> ();

			foreach (PathPoint point in Path.Points.Points) {
				
				IActionAcceptor acceptor 	= point.StaticUnit as IActionAcceptor;
				List<string> actions 		= PerformableActions.GetAcceptedActions (acceptor);
				
				foreach (string action in actions) {
					acceptedActions.Add (
						new KeyValuePair<string, IActionAcceptor> (action, acceptor)
					);
				}
			}
			return acceptedActions;
		}

		void EnableAcceptedActions (List<KeyValuePair<string, IActionAcceptor>> acceptedActions) {

			Dictionary<KeyValuePair<string, IActionAcceptor>, System.Type> unpairedActions = 
				new Dictionary<KeyValuePair<string, IActionAcceptor>, System.Type> ();

			foreach (var acceptedAction in acceptedActions) {

				string actionName 										= acceptedAction.Key;
				PerformerAction action 									= PerformableActions.Get (actionName);
				System.Type requiredPair								= action.RequiredPair;
				KeyValuePair<string, IActionAcceptor> acceptorAction	= new KeyValuePair<string, IActionAcceptor> (actionName, acceptedAction.Value);

				// Enable the action if it doesn't require a pair
				if (requiredPair == null) {
					PerformableActions.Enable (actionName);
					continue;
				}

				// If unpairedActions is empty, add the action
				if (unpairedActions.Count == 0) {
					unpairedActions.Add (acceptorAction, action.GetType ());
					continue;
				}

				// If unpairedActions isn't empty, try to find the pair in unpairedActions
				// Both actions are enabled if they are pairs & not from the same IActionAcceptor
				string pair = "";
				foreach (var unpairedAction in unpairedActions) {
					var keyVal = unpairedAction.Key;
					if (unpairedAction.Value == requiredPair && keyVal.Value != acceptorAction.Value) {
						pair = keyVal.Key;
						PerformableActions.Enable (pair);
						PerformableActions.Enable (actionName);
						break;
					}
				}

				// If no pair was found, add the action to the dictionary
				if (pair == "") {
					unpairedActions.Add (acceptorAction, action.GetType ());
				} else {

					// But if a pair WAS found, remove it from the dictionary
					unpairedActions.Remove (acceptorAction);
				}
			}
		}

		public virtual void OnDragRelease (Unit unit) {}
	}
}