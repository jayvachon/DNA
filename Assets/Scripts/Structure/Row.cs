using UnityEngine;
using System.Collections;

public class Row : MonoBehaviour {

	public GameObject step;
	private int stepCount = 12;
	private int index;

	private Step[] steps;
	public Step[] Steps {
		get { return steps; }
	}

	public void Init (int _index) {
		index = _index;
		float ySep = 1f * Structure.scale;
		float rowHeight = (float)stepCount * ySep;
		float yStart = index * rowHeight;
		float slope = 0f;
		float deg = 360f / (float)stepCount;
		steps = new Step[stepCount];
		for (int i = 0; i < stepCount; i ++) {
			float degrees = (float)i * deg;
			steps[i] = CreateStep (yStart + i * ySep, degrees, slope);
		}
	}

	private Step CreateStep (float yPos, float rotation, float slope) {
		GameObject go = Instantiate(
			step, 
			new Vector3(0, yPos, 0), 
			Quaternion.Euler(-slope, rotation, slope)
		) as GameObject;
		Step s = go.GetComponent<Step>();
		s.Init (0.54f); // TODO: set width based on stepCount instead of hardcoding this value
		go.transform.parent = transform;
		return s;
	}
}
