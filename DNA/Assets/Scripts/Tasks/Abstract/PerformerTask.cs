#undef QUARTER_TIME
#undef DEBUG_MSG
using UnityEngine;
using System.Collections;
using DNA.Models;

namespace DNA.Tasks {

	public enum TaskStartResult { Success, Wait, Disabled, Null }
	public delegate void OnStart (PerformerTask task);
	public delegate void OnEnd (PerformerTask task);
	public delegate void OnComplete (PerformerTask task);
	public delegate void OnEndWait (PerformerTask task, bool performing);

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
		public OnEndWait onEndWait;

		protected TaskSettings settings;
		protected AcceptorTask acceptTask;

		bool perform = false;	 // should the task be performing?
		bool performing = false; // is the task being performed?

		public PerformerTask () {
			settings = DataManager.GetTaskSettings (this.GetType ());
			if (settings.Repeat && settings.Duration == 0)
				throw new System.Exception (this.GetType () + " is marked as repeating with a duration of 0. This will cause the game to hang.");
		}

		public TaskStartResult Start (AcceptorTask acceptTask) {
			/*if (acceptTask.Enabled && (Settings.BindCapacity == 0 || acceptTask.BoundCount < Settings.BindCapacity)) {
				if (Start ()) {
					this.acceptTask = acceptTask;
					acceptTask.Bind (this);
					return true;
				}
			}
			return false;*/
			if (!acceptTask.Enabled) {
				return TaskStartResult.Disabled;
			}

			this.acceptTask = acceptTask;

			if (Settings.BindCapacity > 0 && acceptTask.BoundCount >= Settings.BindCapacity) {
				acceptTask.onChangeBoundCount += OnChangeBoundCount;
				return TaskStartResult.Wait;
			}

			TaskStartResult result = Start ();
			if (result == TaskStartResult.Success) {
				acceptTask.Bind (this);
			}
			return result;
		}

		public TaskStartResult Start () {

			// Don't allow the action to overlap itself
			if (!Enabled || performing) return TaskStartResult.Disabled;
			performing = true;
			perform = true;

			Log ("Start", true);
			OnStart ();
			#if QUARTER_TIME
			Coroutine.Start (settings.Duration*0.25f, SetProgress, End);
			#else
			Coroutine.Start (settings.Duration, SetProgress, End);
			#endif

			return TaskStartResult.Success;
		}

		public void Stop () {
			performing = false;
			perform = false;
			Coroutine.Stop (SetProgress);
			if (acceptTask != null)
				acceptTask.Unbind (this);
		}

		public void CancelWait () {
			onEndWait = null;
			acceptTask.onChangeBoundCount -= OnChangeBoundCount;
		}

		void End () {
			Log ("End", true);
			performing = false;
			OnEnd ();
			if (settings.Repeat && perform) {
				if (acceptTask == null) {
					if (Start () != TaskStartResult.Success) {
						SendOnCompleteMessage ();
						if (settings.Repeat && settings.AutoStart) {
							StartOnEnable ();
						}
					}
				} else {
					if (acceptTask.Enabled) {
						if (Start () != TaskStartResult.Success) {
							SendOnCompleteMessage ();
							acceptTask.Unbind (this);
						}
					} else {
						SendOnCompleteMessage ();
						acceptTask.Unbind (this);
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
					if (Start () != TaskStartResult.Success) {
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

		void OnChangeBoundCount (int boundCount) {
			TaskStartResult result = Start (acceptTask);
			if (result != TaskStartResult.Wait) {
				acceptTask.onChangeBoundCount -= OnChangeBoundCount;
				SendEndWaitMessage (result == TaskStartResult.Success);
			}
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