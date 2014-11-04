using UnityEngine;
using System.Collections;

public class Unit : Selectable {

	Vector3 targetPos = Vector3.zero;	// desired position
	float maxForce = 100f;				// max force available
	float pGain = 20f; 					// proportional gain
	float iGain = 0.05f;					// integral gain
	float dGain = 10f;					// differential gain
	Vector3 integrator = Vector3.zero; 	
	Vector3 lastError = Vector3.zero;	// error accumulator
	Vector3 curPos = Vector3.zero;		// actual position
	Vector3 force = Vector3.zero;		// current force

	public override void OnStart () {
		targetPos = MyTransform.position;
	}

	void FixedUpdate () {
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
		rigidbody.AddForce (force);
	}

	public override void ClickNothing (MouseClickEvent e) {
		if (Selected) {
			targetPos = e.point;
		}
	}

	public virtual void OnEndMove () {}
	
	/*bool moving = false;
	
	public override void ClickNothing (MouseClickEvent e) {
		if (Selected) {
			StartMove (e.point);
		}
	}

	public void StartMove (Vector3 destination) {
		StartCoroutine (Move (destination));
	}

	IEnumerator Move (Vector3 destPosition) {

		if (moving) yield return null;
		moving = true;

		Vector3 startPosition = MyTransform.position;
		float distance = Vector3.Distance (startPosition, destPosition);
		float speed = 15f;
		float time = distance / speed;
		float eTime = 0f;

		Vector3 targetDir = Vector3.Normalize(destPosition - startPosition);
		rigidbody.rotation = 
		while (eTime < time) {
			eTime += Time.deltaTime;
			rigidbody.velocity = targetDir * speed;
			yield return null;
		}

		moving = false;
		OnEndMove ();
	}

	public virtual void OnEndMove () {}*/
}
