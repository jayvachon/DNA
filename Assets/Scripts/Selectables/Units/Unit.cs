using UnityEngine;
using System.Collections;

public class Unit : Selectable {

	StraightMovementPath movementPath;
	bool moving = false;

	Vector3 targetPos = Vector3.zero;	// desired position
	float maxForce = 10f;				// max force available
	float pGain = 1f;					// proportional gain
	float iGain = 0f;					// integral gain
	float dGain = 0.25f;				// differential gain
	Vector3 integrator = Vector3.zero; 	
	Vector3 lastError = Vector3.zero;	// error accumulator
	Vector3 curPos = Vector3.zero;		// actual position
	Vector3 force = Vector3.zero;		// current force
	float massForce = 0f;

	public override void OnStart () {
		targetPos = MyTransform.position;
		massForce = rigidbody.mass * 10f;
		maxForce = massForce * 0.15f;
		OnStartChild ();
	}

	void FixedUpdate () {
		return;
		curPos = MyTransform.position;
		Vector3 error = targetPos - curPos; 						// generate the error signal
		integrator += error * Time.deltaTime;						// integrate error
		Vector3 diff = (error - lastError) / Time.deltaTime;		// differentiate error
		lastError = error;

		// calculate the force summing the 3 errors with respective gains
		force = error * pGain + integrator * iGain + diff * dGain; 

		// clamp the force to the max value available
		force = Vector3.ClampMagnitude (force, maxForce);

		// apply the force to accelerate the rigidbody
		rigidbody.AddForce (force * massForce);

		/*if (Input.GetKeyDown (KeyCode.Alpha1)) {
			pGain += 0.1f;
			Debug.Log ("p: " + pGain);
			targetPos = curPos;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			pGain -= 0.1f;
			Debug.Log ("p: " + pGain);
			targetPos = curPos;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			iGain += 0.1f;
			Debug.Log ("i: " + iGain);
			targetPos = curPos;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			iGain -= 0.1f;
			Debug.Log ("i: " + iGain);
			targetPos = curPos;
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			dGain += 0.1f;
			Debug.Log ("d: " + dGain);
			targetPos = curPos;
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			dGain -= 0.1f;
			Debug.Log ("d: " + dGain);
			targetPos = curPos;
		}*/
	}

	public override void ClickNothing (MouseClickEvent e) {
		if (Selected) {
			//targetPos = e.point;
			StartMove2 (e.point);
		}
	}

	public void StartMove (Vector3 pos) {
		targetPos = pos;
	}

	public void StartMove2 (Vector3 pos) {
		if (moving) return;
		movementPath = new StraightMovementPath (MyTransform.position, pos);
		StartCoroutine (Move (movementPath.Path));
	}

	IEnumerator Move (Vector3[] path) {

		moving = true;

		int p = 0;
		int pathLength = path.Length;

		while (p < pathLength-1) {

			yield return StartCoroutine (MoveStep (path[p], path[p+1]));

			p ++;
			yield return null;
		}

		moving = false;
	}

	IEnumerator MoveStep (Vector3 start, Vector3 end) {
		
		float time = 0.5f;
		float eTime = 0f;

		while (eTime < time && moving) {
			eTime += Time.deltaTime;
			MyTransform.position = Vector3.Lerp (start, end, eTime / time);
			yield return null;
		}
	}

	IEnumerator Stationary (Vector3 centerPoint) {
		yield return null;
	}

	public virtual void OnEndMove () {}
	public virtual void OnStartChild () {}
}
