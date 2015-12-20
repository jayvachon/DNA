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
			get { return Mathf.Abs (rotationOrigin.eulerAngles.y - rotationTarget.eulerAngles.y) > 0f; }
		}

		public Trajectory (Vector3 moverPosition, Vector3 origin, Vector3 target, float radius) {
			this.origin = origin;
			this.target = target;
			this.rotationOrigin = Quaternion.LookRotation (moverPosition - origin);
			this.rotationTarget = Quaternion.LookRotation (target - origin);
			this.translateOrigin = origin.GetPointAroundAxis (rotationTarget, radius);
			this.translateTarget = target.GetPointAroundAxis (Quaternion.LookRotation (origin - target), radius);
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
				Rotate ();
			} else {
				Translate ();
			}
		}

		void Rotate () {

			// Rotate to face the target
			
			if (rotateCo != null)
				rotateCo.Stop (false);
			rotateCo = Co.Start (RotationTime, UpdateRotation, Translate);
		}

		void UpdateRotation (float p) {
			rotation = trajectory.rotationOrigin.SlerpClockwise (trajectory.rotationTarget , p);
			mover.position = trajectory.origin.GetPointAroundAxis (rotation, settings.radius);
		}

		void Translate () {

			// Move from the origin to the target

			if (translateCo != null)
				translateCo.Stop (false);
			translateCo = Co.Start (TranslateTime, UpdateTranslation);
		}

		void UpdateTranslation (float p) {
			mover.position = Vector3.Lerp (trajectory.translateOrigin, trajectory.translateTarget, p);
		}
	}

	MovementBehaviour movement;
	Settings settings = new Settings (1.5f, 2f);
	Trajectory trajectory;

	// <temp vars>
	public Transform origin;
	public Transform[] targets;
	// </temp vars>

	void OnEnable () {
		movement = new MovementBehaviour (transform, settings);
	}

	void Update () {
		trajectory = new Trajectory (transform.position, origin.position, targets[0].position, settings.radius);
		GizmosDrawer.Instance.Clear ();
		DrawClock (trajectory.origin, trajectory.rotationOrigin, trajectory.rotationTarget);

		if (Input.GetKeyDown (KeyCode.Q)) {
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
