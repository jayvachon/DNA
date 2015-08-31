#undef DEBUG_MSG
using UnityEngine;
using System.Collections;
using DNA.Models;

namespace DNA.Tasks {

	public delegate void OnStart ();
	public delegate void OnEnd ();

	public abstract class PerformerTask : Task {

		public float Progress { get; private set; }
		public ITaskPerformer Performer { get; set; }

		// For testing
		public TaskSettings Settings {
			get { return settings; }
		}

		public bool Performing {
			get { return performing; }
		}
		// End testing

		public OnStart onStart;
		public OnEnd onEnd;

		TaskSettings settings;
		bool perform = false;
		bool performing = false;

		public PerformerTask () {
			settings = DataManager.GetTaskSettings (this.GetType ());
			if (settings.Repeat && settings.Duration == 0)
				throw new System.Exception (this.GetType () + " is marked as repeating with a duration of 0. This will cause the game to hang.");
			if (settings.AutoStart) Start ();
		}

		public void Start () {

			// Don't allow the action to overlap itself
			if (!Enabled || performing) return;
			performing = true;
			perform = true;

			Log ("Start", true);
			SendOnStartMessage ();
			Coroutine.Start (settings.Duration, SetProgress, End);
		}

		public void Stop () {
			performing = false;
			perform = false;
			Coroutine.Stop (SetProgress);
		}

		void End () {
			Log ("End", true);
			performing = false;
			OnEnd ();
			if (settings.Repeat && perform) {
				Start ();
			} else {
				perform = false;
			}
		}

		void SetProgress (float progress) {
			Progress = progress;
		}

		void SendOnStartMessage () {
			if (onStart != null) onStart ();
		}

		protected virtual void OnEnd () {
			if (onEnd != null) onEnd ();
		}

		void Log (string message, bool printType) {
			#if DEBUG_MSG
			if (printType) {
				Debug.Log (message + ": " + this.GetType ());
			} else {
				Debug.Log (message);
			}
			#endif
		}
	}
}