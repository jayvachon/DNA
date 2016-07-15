#undef QUARTER_TIME
#undef DEBUG_MSG
using UnityEngine;
using System.Collections;
using DNA.Models;

namespace DNA.Tasks {

	public enum TaskState { Idle, Performing, Queued }

	public delegate void OnStart (PerformerTask task);
	public delegate void OnEnd (PerformerTask task);
	public delegate void OnComplete (PerformerTask task);
	public delegate void OnEndWait (PerformerTask task, bool performing);

	public delegate void OnChangeTaskState (TaskState state);

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

		TaskState state = TaskState.Idle;
		public TaskState State {
			get { return state; }
			private set {
				state = value;
				if (onChangeState != null)
					onChangeState (state);
			}
		}

		public virtual float Duration {
			get {
				float d = settings.Duration;
				if (Performer is ITaskRateSetter) {
					float rate = ((ITaskRateSetter)Performer).TaskRate;
					#if UNITY_EDITOR
					if (Mathf.Approximately (rate, 0f))
						throw new System.Exception ("Task rate must be larger than 0");
					#endif
					d *= rate;
				}
				return d;
			}
		}

		public bool Performing {
			get { return performing; }
		}

		public OnStart onStart;
		public OnEnd onEnd;
		public OnComplete onComplete;
		public OnEndWait onEndWait;
		public OnChangeTaskState onChangeState;

		protected TaskSettings settings;
		protected AcceptorTask acceptTask;

		bool perform = false;	 // should the task be performing?
		bool performing = false; // is the task being performed?

		public PerformerTask (string symbolOverride="") {

			// Load the settings
			if (symbolOverride == "")
				settings = DataManager.GetTaskSettings (this.GetType ());
			else
				settings = DataManager.GetTaskSettings (symbolOverride);

			// Validate settings
			if (settings.Repeat && Duration == 0)
				throw new System.Exception (this.GetType () + " is marked as repeating with a duration of 0. This will cause the game to hang.");

			if (settings.Type != GetType ())
				throw new System.Exception ("The type defined in the task model " + settings + " does not match the type " + GetType ());
		}

		public void OnBind () {
			if (Start ()) {
				State = TaskState.Performing;
			} else {
				State = TaskState.Idle;
			}
		}

		public void OnQueue () {
			State = TaskState.Queued;
		}

		public bool Start (AcceptorTask acceptTask) {
			
			if (!Enabled || !acceptTask.Enabled) {
				return false;
			}

			this.acceptTask = acceptTask;
			acceptTask.Binder.Add (this, settings.BindCapacity);
			return true;
		}

		public bool Start () {

			// Don't allow the action to overlap itself
			if (!Enabled || performing) return false;
			performing = true;
			perform = true;

			Log ("Start", true);
			OnStart ();
			Co2.StartCoroutine (Duration
			#if QUARTER_TIME
			*0.25f
			#else
			, SetProgress, End);
			#endif

			return true;
		}

		public void Stop () {
			State = TaskState.Idle;
			performing = false;
			perform = false;
			Co2.StopCoroutine (SetProgress);
			if (acceptTask != null)
				acceptTask.Binder.Remove (this, settings.BindCapacity);
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
						if (!Start ()) {
							SendOnCompleteMessage ();
						}
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
				float time = Duration;
				#if QUARTER_TIME
				time *= 0.25f;
				#endif
				Co2.WaitForSeconds (time, () => {
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
			State = TaskState.Idle;
			if (acceptTask != null)
				acceptTask.Binder.Remove (this, settings.BindCapacity);
			if (onComplete != null) onComplete (this);
		}

		void SendEndWaitMessage (bool performing) {
			if (onEndWait != null)
				onEndWait (this, performing);
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