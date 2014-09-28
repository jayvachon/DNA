using UnityEngine;
using System.Collections;

public class StepController : MonoBehaviour {

	public MainCamera cam;
	public RowCreator rowCreator;
	private int activeStepIndex = 0;
	private Step activeStep;
	private Step[] steps;

	private void Awake () {
		cam = Instantiate (cam) as MainCamera;
		rowCreator = Instantiate (rowCreator) as RowCreator;
		rowCreator.Init ();
		GetSteps ();
		SetActiveStep (steps[0]);
	}

	private void GetSteps () {
		Row[] rows = rowCreator.Rows;
		steps = new Step[rows.Length * 12];
		int index = 0;
		for (int i = 0; i < rows.Length; i ++) {
			Step[] s = rows[i].Steps;
			for (int j = 0; j < s.Length; j ++) {
				steps[index] = s[j];
				index ++;
			}
		}
	}

	private bool SetActiveStep (Step step) {
		bool canSetActive = cam.SetActiveStep (step);
		if (canSetActive) {
			step.gameObject.SetActive (true);
		}
		return canSetActive;
	}

	private void IterateActiveStep () {
		Step nextStep = steps[activeStepIndex + 1];
		if (SetActiveStep (nextStep)) {
			activeStepIndex ++;
			activeStep = steps[activeStepIndex];
		}
	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			IterateActiveStep ();
		}
	}
}
