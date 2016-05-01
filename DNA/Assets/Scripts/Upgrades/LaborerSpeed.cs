using UnityEngine;
using System.Collections;

public class LaborerSpeed : Upgrade<float> {

	protected override float[] Levels {
		get { return new float[] { 1.33f, 1.5f, 1.67f, 2f, 2.33f, 2.5f }; }
	}
}
