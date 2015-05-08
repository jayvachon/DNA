using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SeaLevelMeter : MonoBehaviour {

	public Sea sea;
	public Slider slider;

	void Update () {
		slider.value = sea.LevelPercent;
	}
}
