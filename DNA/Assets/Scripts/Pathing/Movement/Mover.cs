using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mover : MonoBehaviour {

	struct Settings {
		
		public readonly float radius;
		public readonly float speed;

		public Settings (float radius, float speed) {
			this.radius = radius;
			this.speed = speed;
		}
	}

	struct Trajectory {

		public readonly Vector3 origin;
		public readonly Vector3 target;
		public readonly Quaternion rotationOrigin;
		public readonly Quaternion rotationTarget;
		public readonly Vector3 translateOrigin;
		public readonly Vector3 translateTarget;

		public bool NeedsRotation {
			get { return Mathf.Abs (rotationOrigin.eulerAngles.y - rotationTarget.eulerAngles.y) > 0.1f; }
		}

		public Trajectory (Vector3 moverPosition, Vector3 origin, Vector3 target, float radius) {

			this.origin = origin;
			this.target = target;

			if (Vector3.Distance (moverPosition, origin) > radius) {
				
				// If not inside the radius of the origin, move straight to the target
				this.rotationOrigin = Quaternion.identity;
				this.rotationTarget = Quaternion.identity;
				this.translateOrigin = moverPosition;
				this.translateTarget = target.GetPointAroundAxis (Quaternion.LookRotation (translateOrigin - target), radius);
			} else {

				// If inside the radius of the origin, rotate before moving
				this.rotationOrigin = Quaternion.LookRotation (moverPosition - origin);
				this.rotationTarget = (Vector3.Distance (origin, target) > 0.1f) 
					? Quaternion.LookRotation (target - origin) 
					: rotationOrigin;
				this.translateOrigin = origin.GetPointAroundAxis (rotationTarget, radius);
				this.translateTarget = (Vector3.Distance (origin, target) > 0.1f) 
					? target.GetPointAroundAxis (Quaternion.LookRotation (origin - target), radius) 
					: translateOrigin;
			}
		}
	}

	class MovementBehaviour {

		Transform mover;
		Settings settings;
		Trajectory trajectory;

		Co rotateCo;
		Co translateCo;

		Quaternion rotation;
		Vector3 position;

		public delegate void OnEndMove ();
		public delegate void OnEndRotation ();

		public OnEndMove onEndMove;
		public OnEndRotation onEndRotation;

		float RotationTime {
			get { return Mathf.Abs (trajectory.rotationOrigin.ArcLengthClockwise (trajectory.rotationTarget , settings.radius)) / settings.speed; }
		}

		float TranslateTime {
			get { return Vector3.Distance (trajectory.translateOrigin, trajectory.translateTarget) / settings.speed; }
		}

		// <temp>
		public Quaternion Rotation {
			get { return rotation; }
		}
		// </temp>

		public MovementBehaviour (Transform mover, Settings settings) {
			this.mover = mover;
			this.settings = settings;
		}

		public void SetTrajectory (Trajectory t) {

			this.trajectory = t;

			if (trajectory.NeedsRotation) {
				Rotate (RotationTime, Translate);
			} else {
				Translate ();
			}
		}

		public void FullRotation (float duration, Vector3 point) {
			trajectory = new Trajectory (mover.position, point, point, settings.radius);
			Rotate (duration, EndRotation);
		}

		void Rotate (float duration, System.Action onEnd) {
			// if (rotateCo != null && rotateCo.gameObject.activeSelf)
				// rotateCo.Stop (false);
			rotateCo = Co.Start (duration, UpdateRotation, onEnd);
		}

		void UpdateRotation (float p) {
			rotation = trajectory.rotationOrigin.SlerpClockwise (trajectory.rotationTarget, p);
			mover.position = trajectory.origin.GetPointAroundAxis (rotation, settings.radius);
		}

		void Translate () {
			// if (translateCo != null && translateCo.gameObject.activeSelf)
				// translateCo.Stop (false);
			translateCo = Co.Start (TranslateTime, UpdateTranslation, EndMove);
		}

		void UpdateTranslation (float p) {
			mover.position = Vector3.Lerp (trajectory.translateOrigin, trajectory.translateTarget, p);
		}

		void EndMove () {
			if (onEndMove != null)
				onEndMove ();
		}

		void EndRotation () {
			if (onEndRotation != null)
				onEndRotation ();
		}
	}

	MovementBehaviour movement;
	Settings settings = new Settings (1.5f, 2f);
	Trajectory trajectory;

	// <temp vars>
	public Transform weirdTarget;
	public Transform[] targets;
	int pathPosition = 0;
	// </temp vars>

	void OnEnable () {
		movement = new MovementBehaviour (transform, settings);
		// movement.onEndMove += OnEndMove;
		movement.onEndMove += OnArrive;
	}

	void OnDisable () {
		movement.onEndMove -= OnArrive;
		// movement.onEndMove -= OnEndMove;
	}

	void StartMove () {
		trajectory = new Trajectory (transform.position, targets[pathPosition].position, targets[pathPosition+1].position, settings.radius);
		movement.SetTrajectory (trajectory);
	}

	void MoveToMiddleTarget () {
		// movement.onEndMove -= OnEndMove;
		StartMove ();
	}

	void OnArrive () {
		if (pathPosition < targets.Length-2) {
			pathPosition ++;
		} else {
			pathPosition = 0;
		}
	}

	void OnEndMove () {
		if (pathPosition < targets.Length-2) {
			pathPosition ++;
			StartMove ();
		}
	}

	void Update () {
		
		GizmosDrawer.Instance.Clear ();
		DrawClock (trajectory.origin, trajectory.rotationOrigin, trajectory.rotationTarget);

		if (Input.GetKeyDown (KeyCode.Q)) {
			pathPosition = 0;
			StartMove ();
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			MoveToMiddleTarget ();
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			movement.FullRotation (3f, targets[pathPosition].position);
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			trajectory = new Trajectory (transform.position, targets[pathPosition].position, weirdTarget.position, settings.radius);
			movement.SetTrajectory (trajectory);
		}
	}

	void DrawClock (Vector3 origin, Quaternion from, Quaternion to) {
		float clockRadius = 2f;
		float angA = movement.Rotation.eulerAngles.y * Mathf.Deg2Rad;
		float angB = from.eulerAngles.y * Mathf.Deg2Rad;
		float angC = to.eulerAngles.y * Mathf.Deg2Rad;
		Vector3 dirA = new Vector3 (
			origin.x + clockRadius * Mathf.Sin (angA),
			origin.y,
			origin.z + clockRadius * Mathf.Cos (angA)
		);
		Vector3 dirB = new Vector3 (
			origin.x + clockRadius * Mathf.Sin (angB),
			origin.y,
			origin.z + clockRadius * Mathf.Cos (angB)
		);
		Vector3 dirC = new Vector3 (
			origin.x + clockRadius * Mathf.Sin (angC),
			origin.y,
			origin.z + clockRadius * Mathf.Cos (angC)
		);
		GizmosDrawer.Instance.Add (new GizmoLine (origin, dirA));
		GizmosDrawer.Instance.Add (new GizmoLine (origin, dirB));
		GizmosDrawer.Instance.Add (new GizmoLine (origin, dirC));
	}
}
