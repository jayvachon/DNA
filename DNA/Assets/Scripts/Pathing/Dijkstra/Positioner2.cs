﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Paths {

	public class Positioner2 {

		struct Settings {
			
			public readonly float radius;
			public readonly float maxSpeed;

			public Settings (float radius, float maxSpeed) {
				this.radius = radius;
				this.maxSpeed = maxSpeed;
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

				float originDistance = Vector3.Distance (moverPosition, origin);

				if (!Mathf.Approximately (originDistance, radius) && originDistance > radius) {
					
					// If not inside the radius of the origin, move straight to the target
					// (ignores the origin)
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

			public delegate void OnEndMove (Vector3 target);
			public delegate void OnEndRotation ();
			public delegate void OnStartRotation ();

			public OnEndMove onEndMove;
			public OnEndRotation onEndRotation;
			public OnStartRotation onStartRotation;

			public bool Moving {
				get { return (rotateCo != null && rotateCo.Running) || (translateCo != null && translateCo.Running); }
			}

			float RotationTime {
				get { return Mathf.Abs (trajectory.rotationOrigin.ArcLengthClockwise (trajectory.rotationTarget , settings.radius)) / settings.maxSpeed; }
			}

			float TranslateTime {
				get { return Vector3.Distance (trajectory.translateOrigin, trajectory.translateTarget) / settings.maxSpeed; }
			}

			public MovementBehaviour (Transform mover, Settings settings) {
				this.mover = mover;
				this.settings = settings;
			}

			public void SetTrajectory (Trajectory t, float speed) {

				this.trajectory = t;

				if (trajectory.NeedsRotation) {
					StartRotation ();
					Rotate (RotationTime / speed, () => { 
						EndRotation ();
						Translate (speed);
					});
				} else {
					Translate (speed);
				}
			}

			public void FullRotation (float duration, Vector3 point) {
				trajectory = new Trajectory (mover.position, point, point, settings.radius);
				Rotate (duration, EndRotation);
			}

			public void Stop () {
				if (rotateCo != null)
					rotateCo.Stop (false);
				if (translateCo != null)
					translateCo.Stop (false);
			}

			void Rotate (float duration, System.Action onEnd) {
				if (rotateCo != null)
					rotateCo.Stop (false);
				rotateCo = Co.Start (duration, UpdateRotation, onEnd);
			}

			void UpdateRotation (float p) {
				Quaternion rotation = trajectory.rotationOrigin.SlerpClockwise (trajectory.rotationTarget, p);
				mover.position = trajectory.origin.GetPointAroundAxis (rotation, settings.radius);
			}

			void Translate (float speed) {
				if (translateCo != null)
					translateCo.Stop (false);
				translateCo = Co.Start (TranslateTime / speed, UpdateTranslation, () => { EndMove (trajectory.target); });
			}

			void UpdateTranslation (float p) {
				mover.position = Vector3.Lerp (trajectory.translateOrigin, trajectory.translateTarget, p);
			}

			void EndMove (Vector3 target) {
				if (onEndMove != null)
					onEndMove (target);
			}

			void StartRotation () {
				if (onStartRotation != null)
					onStartRotation ();
			}

			void EndRotation () {
				if (onEndRotation != null)
					onEndRotation ();
			}
		}

		class PathBehaviour {

			Transform mover;
			Settings settings;
			GridPoint startPoint;
			GridPoint endPoint;
			int pathPosition = 0;
			List<GridPoint> path;

			public GridPoint PreviousPoint {
				get { return path[pathPosition-1]; }
			}

			public GridPoint CurrentPoint {
				get { 
					if (path == null || path.Count == 0)
						return endPoint;
					return path[pathPosition]; 
				}
			}

			public void SetDestination (GridPoint point) {
				endPoint = point;
				CalculatePath ();
			}

			public Vector3[] NextTrajectory () {

				if (path == null || path.Count == 0 || pathPosition == path.Count-1)
					return null;

				Vector3 origin = path[pathPosition].Position;
				Vector3 target = path[pathPosition+1].Position;
				pathPosition ++;

				return new Vector3[] { origin, target };
			}

			void CalculatePath () {

				// Check if mover is already moving along a path
				if (path != null && path.Count > 0) {
					GridPoint nearest = NearestPoint ();
					if (nearest == null) {
						
						// Mover is between points and gets redirected (this is buggy)
						startPoint = path[pathPosition];
					} else {

						// Mover is at a point and gets redirected
						startPoint = nearest;
					}
				}

				// Ignore if the mover is already at the destination
				if (startPoint == endPoint) {
					path.Clear ();
					return;
				}
				
				List<GridPoint> newPath = Pathfinder.GetFreePath (startPoint, endPoint);
				if (newPath.Count > 0) {
					path = newPath;
					pathPosition = 0;
				}
			}

			// Returns a point if the mover is within the radius of the point
			// finding none, returns null
			GridPoint NearestPoint () {
				Vector3 moverPosition = mover.position;
				GridPoint origin = path[pathPosition-1];
				GridPoint target = path[pathPosition];
				if (Vector3.Distance (moverPosition, origin.Position) <= settings.radius)
					return origin;
				if (Vector3.Distance (moverPosition, target.Position) <= settings.radius)
					return target;
				return null;
			}

			public PathBehaviour (Transform mover, Settings settings, GridPoint startPoint) {
				this.mover = mover;
				this.settings = settings;
				this.startPoint = startPoint;
			}
		}

		public delegate void OnArriveAtDestination (GridPoint point);
		public delegate void OnEnterPoint (GridPoint point);
		public delegate void OnExitPoint (GridPoint point);

		public OnArriveAtDestination onArriveAtDestination;
		public OnEnterPoint onEnterPoint;
		public OnExitPoint onExitPoint;

		GridPoint destination;
		public GridPoint Destination {
			get { return destination; }
			set {
				destination = value;
				path.SetDestination (destination);
				StartMove ();
			}
		}

		public bool Moving {
			get { return movement.Moving; }
		}

		float speed = 1f; // %
		public float Speed {
			get { return speed; }
			set { speed = value; }
		}

		float speedMultiplier = 1f; // upgradable value

		Transform mover;
		Settings settings = new Settings (1.25f, 2f);
		MovementBehaviour movement;
		PathBehaviour path;

		public Positioner2 (Transform mover, GridPoint startPoint) {
			this.mover = mover;
			movement = new MovementBehaviour (mover, settings);
			movement.onEndMove += OnEndMove;
			movement.onStartRotation += OnStartRotation;
			movement.onEndRotation += OnEndRotation;
			path = new PathBehaviour (mover, settings, startPoint);

			Upgrades.Instance.AddListener<LaborerSpeed> (
				(LaborerSpeed u) => speedMultiplier = u.CurrentValue
			);
		}

		public void RotateAroundPoint (float time) {
			movement.FullRotation (time, (path.CurrentPoint == null)
				? Destination.Position
				: path.CurrentPoint.Position);
		}

		bool StartMove () {
			Vector3[] trajectory = path.NextTrajectory ();
			if (trajectory == null) {
				movement.Stop ();
			} else {
				movement.SetTrajectory (new Trajectory (mover.position, trajectory[0], trajectory[1], settings.radius), Speed * speedMultiplier);
				return true;
			}
			return false;
		}

		void OnEndMove (Vector3 target) {
			if (!StartMove () && onArriveAtDestination != null) {
				onArriveAtDestination (path.CurrentPoint);
			}
		}

		void OnStartRotation () {
			if (onEnterPoint != null) {
				onEnterPoint (path.PreviousPoint);
			}
		}

		void OnEndRotation () {
			if (onExitPoint != null) {
				onExitPoint (path.CurrentPoint);
			}
		}
	}
}