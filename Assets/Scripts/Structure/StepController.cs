using UnityEngine;
using System.Collections;

public class StepController : MonoBehaviour {

	public RowCreator rowCreator;
	private int activeStepIndex = 0;
	private Step activeStep;
	private Step[] steps;

	private void Awake () {
		rowCreator = Instantiate (rowCreator) as RowCreator;
		rowCreator.Init ();
		GetSteps ();
	}

	private void Start () {
		SetActiveStep (0);
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

	private void SetActiveStep (int s) {
		activeStep = steps[s];
		Events.instance.Raise (new ChangeActiveStepEvent (activeStep));
	}

	private void IterateActiveStep () {
		activeStepIndex ++;
		SetActiveStep (activeStepIndex);
	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			IterateActiveStep ();
		}
	}
}
