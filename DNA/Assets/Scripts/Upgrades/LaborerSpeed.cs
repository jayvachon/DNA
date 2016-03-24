using UnityEngine;
using System.Collections;

public class LaborerSpeed : Upgrade<float> {

	protected override float[] Levels {
		get { return new float[] { 1f, 1.2f, 1.33f, 1.5f, 1.75f, 2f }; }
	}
}
