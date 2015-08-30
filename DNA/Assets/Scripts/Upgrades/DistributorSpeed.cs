using UnityEngine;
using System.Collections;

public class DistributorSpeed : Upgrade<float> {

	protected override float[] Levels {
		get { return new float[] { 0.5f, 0.6f, 0.75f, 0.9f, 1f }; }
	}
}
