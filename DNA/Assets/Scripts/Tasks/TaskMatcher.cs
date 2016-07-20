using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Tasks {

	public static class TaskMatcher {

		// Returns a task for the performer to perform		
		public static MatchResult GetPerformable (ITaskPerformer performer, ITaskAcceptor acceptor, bool mustBeEnabled=true) {

			List<PerformerTask> matches = mustBeEnabled
				? GetEnabled (performer, acceptor)
				: GetActive (performer, acceptor);

			if (matches.Count == 0)
				return null;

			// Prioritizes tasks that don't require a pair, or whose pair exists between the acceptors
			foreach (PerformerTask task in matches) {

				AcceptorTask acceptorTask = GetAcceptor (task, acceptor);
				if (task.Settings.Pair == null) {
					return new MatchResult (task, false, acceptorTask);
				}
			}

			return new MatchResult (matches[0], true, GetAcceptor (matches[0], acceptor));
		}

		// Tries to find a pair for the provided match		
		public static MatchResult GetPerformable (MatchResult match, ITaskPerformer performer, ITaskAcceptor acceptor) {

			if (match == null)
				return null;

			AcceptorTask pair = GetPair (match.Match, acceptor);
			if (pair == null)
				return null;

			foreach (PerformerTask performerTask in GetEnabled (performer, acceptor)) {
				if (pair.AcceptedTask == performerTask.GetType ()) {
					return new MatchResult (performerTask, true, pair);
				}
			}
			return null;
		}

		public static List<PerformerTask> GetActive (ITaskPerformer performer, ITaskAcceptor acceptor) {
			return GetMatching (performer.PerformableTasks.ActiveTasks, acceptor.AcceptableTasks.ActiveTasks);
		}

		public static List<PerformerTask> GetEnabled (ITaskPerformer performer, ITaskAcceptor acceptor) {
			try {
				return GetMatching (performer.PerformableTasks.EnabledTasks, acceptor.AcceptableTasks.EnabledTasks);
			} catch {
				throw new System.Exception ("The ITaskPerformer " + performer + " or ITaskAcceptor " + acceptor + " is null");
			}
		}

		public static AcceptorTask GetPair (PerformerTask task, DNA.Paths.PathElement acceptor, bool mustBeEnabled=true) {
			try {
				return GetPair (task, ((ITaskAcceptor)acceptor.Object), mustBeEnabled);
			} catch {
				throw new System.Exception (acceptor.Object + " does not implement the ITaskAcceptor interface");
			}
		}

		public static AcceptorTask GetPair (PerformerTask task, ITaskAcceptor acceptor, bool mustBeEnabled=true) {
			// TODO: linq
			Dictionary<System.Type, AcceptorTask> tasks = mustBeEnabled
				? acceptor.AcceptableTasks.EnabledTasks
				: acceptor.AcceptableTasks.ActiveTasks;
			foreach (var acceptorTask in tasks) {
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

		public static MatchResult StartMatch (ITaskPerformer performer, ITaskAcceptor acceptor, bool mustBeEnabled=false) {
			MatchResult match = GetPerformable (performer, acceptor, mustBeEnabled);
			match.Start (true);
			return match;
		}

		/// <summary>
		/// Gets the Performer Tasks that the performers share in common
		/// </summary>
		public static List<PerformerTask> GetTasksInCommon (List<ITaskPerformer> performers, bool getActive=false) {

			if (performers.Count == 0)
				return new List<PerformerTask> ();

			Dictionary<System.Type, PerformerTask> firstTasks = getActive 
				? performers[0].PerformableTasks.ActiveTasks
				: performers[0].PerformableTasks.EnabledTasks;

			if (performers.Count < 2)
				return firstTasks.Values.ToList ().ConvertAll (x => x as PerformerTask);

			// foreach (ITaskPerformer p in performers) {

				Dictionary<System.Type, PerformerTask> commonTasks = new Dictionary<System.Type, PerformerTask> ();
				foreach (var t in commonTasks) {
					if (!firstTasks.ContainsKey (t.Key))
						firstTasks.Remove (t.Key);
				}

				// if (firstTasks.Count == 0)
					// break;
			// }
			return firstTasks.Values.ToList ().ConvertAll (x => x as PerformerTask);
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

		public float Duration {
			get { return Match.Duration * GetPerformCount (); }
		}

		public MatchResult (PerformerTask match, bool needsPair, AcceptorTask acceptor) {
			Match = match;
			NeedsPair = needsPair;
			Acceptor = acceptor;
		}

		public bool Start (bool ignorePair=false) {
			if (!ignorePair && NeedsPair) // TODO: this check might be excessive
				throw new System.Exception ("The task " + Match + " is unpaired and will not start.");
			if (Acceptor != null) {
				return Match.Start (Acceptor);
			} else {
				return Match.Start ();
			}
		}

		public void Stop () {
			Match.Stop ();
		}

		public int GetPerformCount () {
			AcceptInventoryTask a = Acceptor as AcceptInventoryTask;
			if (a == null)
				return 0;

			return a.GetPerformCount (Match);
		}

		public void Print () {
			Debug.Log (Match + " Acceptor is: " + Acceptor + " - Needs pair ? " + NeedsPair);
		}
	}
}