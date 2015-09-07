using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Tasks {

	public static class TaskMatcher {

		public static MatchResult GetPerformable (ITaskPerformer performer, ITaskAcceptor acceptor, ITaskAcceptor acceptorPair) {
			
			List<PerformerTask> matches = GetEnabled (performer, acceptor);
			MatchResult match = null;

			// Prioritizes tasks that don't require a pair, or whose pair exists between the acceptors
			foreach (PerformerTask task in matches) {
				
				AcceptorTask acceptorTask = GetAcceptor (task, acceptor);
				if (task.Settings.Pair == null) {
					return new MatchResult (task, false, acceptorTask);
				}

				if (match == null || match.NeedsPair) {
					AcceptorTask pair = GetPair (task, acceptorPair);
					if (pair != null)
						return new MatchResult (task, false, acceptorTask);
					match = new MatchResult (task, true, acceptorTask);
				}
			}

			return match;
		}

		public static List<PerformerTask> GetActive (ITaskPerformer performer, ITaskAcceptor acceptor) {
			return GetMatching (performer.PerformableTasks.ActiveTasks, acceptor.AcceptableTasks.ActiveTasks);
		}

		public static List<PerformerTask> GetEnabled (ITaskPerformer performer, ITaskAcceptor acceptor) {
			return GetMatching (performer.PerformableTasks.EnabledTasks, acceptor.AcceptableTasks.EnabledTasks);
		}

		public static AcceptorTask GetPair (PerformerTask task, ITaskAcceptor acceptor) {
			// TODO: linq
			foreach (var acceptorTask in acceptor.AcceptableTasks.EnabledTasks) {
				AcceptorTask a = acceptorTask.Value;
				if (a.GetType () == task.Settings.Pair)
					return a;
			}
			return null;
		}

		public static AcceptorTask GetAcceptor (PerformerTask task, ITaskAcceptor acceptor) {
			// TODO: linq
			foreach (var acceptorTask in acceptor.AcceptableTasks.EnabledTasks) {
				AcceptorTask a = acceptorTask.Value;
				if (task.GetType () == a.AcceptedTask)
					return a;	
			}
			return null;
		}

		static List<PerformerTask> GetMatching (Dictionary<System.Type, PerformerTask> performerTasks, Dictionary<System.Type, AcceptorTask> acceptorTasks) {
			// TODO: linq
			List<PerformerTask> matching = new List<PerformerTask> ();
			foreach (var acceptorTask in acceptorTasks) {
				PerformerTask p;
				if (performerTasks.TryGetValue (acceptorTask.Value.AcceptedTask, out p)) {
					matching.Add (p);
				}
			}
			return matching;
		}
	}

	public class MatchResult {

		public readonly PerformerTask Match;
		public readonly bool NeedsPair;
		readonly AcceptorTask Acceptor;

		public System.Type PairType {
			get { return Match.Settings.Pair; }
		}

		public MatchResult (PerformerTask match, bool needsPair, AcceptorTask acceptor) {
			Match = match;
			NeedsPair = needsPair;
			Acceptor = acceptor;
		}

		public void Start () {
			if (NeedsPair)
				throw new System.Exception ("The task " + Match + " is unpaired and will not start.");
			if (Acceptor != null) {
				Match.Start (Acceptor);
			} else {
				Match.Start ();
			}
		}

		public void Print () {
			Debug.Log (Match + " Acceptor is: " + Acceptor + " - Needs pair ? " + NeedsPair);
		}
	}
}