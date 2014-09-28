using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	private Vector3 center = new Vector3 (0, 0, 0);
	private Transform myTransform;
	private Vector3 destPosition;
	private Vector3 destRotation;
	private bool moving = false;

	private void Awake () {
		myTransform = transform;
	}

	public bool SetActiveStep (Step activeStep) {
		if (moving) return false;
		SetTransform (activeStep);
		StartCoroutine (MoveToTransform (0.5f));
		return true;
	}

	private void SetTransform (Step activeStep) {
		SetRotation (activeStep);
		SetPosition (activeStep);
	}

	private void SetRotation (Step step) {
		float stepYRotation = step.transform.eulerAngles.y;
		destRotation = new Vector3 (
			40f,
			stepYRotation + 180f,
			myTransform.rotation.z
		);
	}

	private void SetPosition (Step step) {
		float yRot = Mathf.Deg2Rad * destRotation.y;
		destPosition = new Vector3 (
			Mathf.Sin (yRot) * -1.5f * Structure.scale,
			step.transform.position.y + 1f * Structure.scale,
			Mathf.Cos (yRot) * -1.5f * Structure.scale
		);
	}

	private IEnumerator MoveToTransform (float time) {

		moving = true;

		float eTime = 0f;
		Vector3 startPosition = myTransform.position;
		Quaternion startRotation = myTransform.rotation;
		Quaternion destRot = Quaternion.Euler (destRotation);

		while (eTime < time) {
			float progress = eTime / time;
			eTime += Time.deltaTime;
			myTransform.position = Vector3.Lerp (startPosition, destPosition, Mathf.SmoothStep (0f, 1f, progress));
			myTransform.rotation = Quaternion.Lerp (startRotation, destRot, Mathf.SmoothStep (0f, 1f, progress));
			yield return null;
		}

		moving = false;
	}
}
