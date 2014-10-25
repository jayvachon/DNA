using UnityEngine;
using System.Collections;

public class StepController : MonoBehaviour {

	public RowCreator rowCreator;
	private int activeStepIndex = 0;
	private Step activeStep;
	private Step[] steps;

	void Awake () {
		rowCreator = Instantiate (rowCreator) as RowCreator;
		rowCreator.Init ();
		GetSteps ();
	}

	void Start () {
		SetActiveStep (0);
	}

	void GetSteps () {
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

	void SetActiveStep (int s) {
		activeStep = steps[s];
		Events.instance.Raise (new ChangeActiveStepEvent (activeStep));
	}

	void IterateActiveStep () {
		activeStepIndex ++;
		SetActiveStep (activeStepIndex);
	}

	/*private void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			IterateActiveStep ();
		}
	}*/
}
