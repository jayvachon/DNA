using UnityEngine;
using System.Collections;
using DNA.Paths;

// ugh all the nulls -__-

namespace DNA.Tasks {

	public class TaskFinder {

		ITaskPerformer performer;

		public TaskFinder (ITaskPerformer performer) {
			this.performer = performer;
		}

		// Finds a match on the given path element
		// (optional) if mustBeEnabled is false, also searches for disabled tasks
		public bool TaskFromPathElement (PathElement elem, out MatchResult result, bool mustBeEnabled=true) {
			ITaskAcceptor acceptor = elem.Object as ITaskAcceptor;
			result = (acceptor == null)
				? null
				: TaskMatcher.GetPerformable (performer, acceptor, mustBeEnabled);
			return result != null;
		}

		// Checks if the match exists on the given path element
		// Finding none, looks for any other matches
		public bool TaskFromMatch (PathElement elem, MatchResult match, out MatchResult result) {
			ITaskAcceptor acceptor = elem.Object as ITaskAcceptor;
			result = (match == null || acceptor == null)
				? null
				: TaskMatcher.GetPerformable (match, performer, acceptor);
			return result != null;
		}

		// Checks for matches on the path element's connections
		public bool TaskFromConnections (PathElement elem, out MatchResult result) {

			GridPoint point = ConnectionToPoint (elem);
			if (point == null) {
				result = null;
				return false;
			}

			Connection connection = point.Connections.Find (x => x.State == DevelopmentState.UnderConstruction);
			if (connection == null) {
				result = null;
				return false;
			}

			return TaskFromPathElement (connection, out result);
		}

		// Finds a pair for the given task
		public bool PairFromTask (PerformerTask task, PathElement currentElement, PathElement previousElement, out PathElement destination) {

			// Early out if task does not require a pair
			if (task.Settings.Pair == null) {
				destination = null;
				return false;
			}

			bool gotoPrevious = !task.Settings.AlwaysPairNearest 
				&& previousElement != null 
				&& TaskMatcher.GetPair (task, previousElement) != null;

			if (gotoPrevious) {
				destination = previousElement;
				return true;
			} 

			return NearestPathElementWithPair (currentElement, task, out destination);
		}

		// Finds the nearest path element with the given task
		public bool NearestPathElementWithTask (PathElement origin, PerformerTask task, out PathElement destination) {

			System.Type taskType = task.GetType ();
			GridPoint point = ConnectionToPoint (origin);
			destination = Pathfinder.FindNearestPoint (
				point,
				(GridPoint p) => { 
					
					if (TaskMatcher
						.GetEnabled (performer, p.Object as ITaskAcceptor)
						.Find (x => x.GetType () == taskType) != null) {
						return true;
					}
					
					foreach (Connection c in p.Connections) {

						ITaskAcceptor acceptor = c.Object as ITaskAcceptor;
						if (acceptor == null) 
							continue;

						if (TaskMatcher
							.GetEnabled (performer, acceptor)
							.Find (x => x.GetType () == taskType) != null) {
							return true;
						}
					}

					return false;
				}
			);
			return destination != null;
		}

		// Finds tasks on the given path element, and if a disabled one is found, searches for the pair
		public bool NearestPairFromDisabledTask (PathElement origin, out PathElement destination) {
			MatchResult result;
			if (TaskFromPathElement (origin, out result, false)) {
				return NearestPathElementWithPair (origin, result.Match, out destination);
			}
			destination = null;
			return false;
		}

		// Finds the nearest path element that pairs with the given task
		public bool NearestPathElementWithPair (PathElement origin, PerformerTask task, out PathElement destination) {
			GridPoint point = ConnectionToPoint (origin);
			destination = Pathfinder.FindNearestPoint (
				point,
				(GridPoint p) => { return TaskMatcher.GetPair (task, p) != null; }
			);
			return destination != null;
		}

		// Finds a relevent GridPoint associated with the Connnection, if the PathElement is a Connection
		GridPoint ConnectionToPoint (PathElement elem) {
			
			Connection c = elem as Connection;
			
			if (c == null) {
				return (GridPoint)elem;
			}

			return System.Array.Find (c.Points, x =>
				x.State == DevelopmentState.UnderConstruction
				|| x.State == DevelopmentState.Developed 
				|| x.State == DevelopmentState.UnderRepair);
		}
	}
}