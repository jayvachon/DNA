#define QUARTER_TIME
#undef DEBUG_MSG
using UnityEngine;
using System.Collections;
using DNA.Models;

namespace DNA.Tasks {

	public delegate void OnStart (PerformerTask task);
	public delegate void OnEnd (PerformerTask task);
	public delegate void OnComplete (PerformerTask task);

	public abstract class PerformerTask : Task {

		public float Progress { get; private set; }
		
		ITaskPerformer performer;
		public ITaskPerformer Performer { 
			get { return performer; }
			set {
				performer = value;
				if (settings.AutoStart) Start ();
			}
		}

		public TaskSettings Settings {
			get { return settings; }
		}

		// For testing (?)
		public bool Performing {
			get { return performing; }
		}
		// End testing

		public OnStart onStart;
		public OnEnd onEnd;
		public OnComplete onComplete;
		protected AcceptorTask acceptTask;
		protected TaskSettings settings;

		bool perform = false;	 // should the task be performing?
		bool performing = false; // is the task performing now?

		public PerformerTask () {
			settings = DataManager.GetTaskSettings (this.GetType ());
			if (settings.Repeat && settings.Duration == 0)
				throw new System.Exception (this.GetType () + " is marked as repeating with a duration of 0. This will cause the game to hang.");
		}

		public void Start (AcceptorTask acceptTask) {
			if (acceptTask.Enabled) {
				if (Start ()) this.acceptTask = acceptTask;
			}
		}

		public bool Start () {

			// Don't allow the action to overlap itself
			if (!Enabled || performing) return false;
			performing = true;
			perform = true;

			Log ("Start", true);
			OnStart ();
			#if QUARTER_TIME
			Coroutine.Start (settings.Duration*0.25f, SetProgress, End);
			#else
			Coroutine.Start (settings.Duration, SetProgress, End);
			#endif

			return true;
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
				if (acceptTask == null) {
					if (!Start ()) {
						SendOnCompleteMessage ();
						if (settings.Repeat && settings.AutoStart) {
							StartOnEnable ();
						}
					}
				} else {
					if (acceptTask.Enabled) {
						if (!Start ()) SendOnCompleteMessage ();
					} else {
						SendOnCompleteMessage ();
					}
				}
			} else {
				perform = false;
				SendOnCompleteMessage ();
			}
		}

		void SetProgress (float progress) {
			Progress = progress;
		}

		void StartOnEnable () {
			if (!Enabled) {
				float time = settings.Duration;
				#if QUARTER_TIME
				time *= 0.25f;
				#endif
				Coroutine.WaitForSeconds (time, () => {
					if (!perform)
						return;
					if (!Start ()) {
						StartOnEnable ();
					}
				});
			}
		}

		protected virtual void OnStart () {
			if (onStart != null) onStart (this);
		}

		protected virtual void OnEnd () {
			if (onEnd != null) onEnd (this);
		}

		void SendOnCompleteMessage () {
			if (onComplete != null) onComplete (this);
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