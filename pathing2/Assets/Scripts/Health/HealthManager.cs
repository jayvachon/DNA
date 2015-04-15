using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthManager {

	float health = 1f;
	public float Health {
		get { return health; }
		private set { health = value; }
	}
	float healthAtSicknessOnset;

	public bool Sick { get; private set; }

	public void StartSickness () {
		if (Sick) return;
		Debug.Log ("starting Sickness");
		healthAtSicknessOnset = Health;
		Coroutine.Instance.StartCoroutine (30f, OnSickness);
		Sick = true;
	}

	public void StopSickness () {
		Sick = false;
		Coroutine.Instance.StopCoroutine (OnSickness);
	}

	void OnSickness (float progress) {
		Health = Mathf.Abs (SicknessCurve (progress)-1) * healthAtSicknessOnset;
		Debug.Log (Health);
	}

	float SicknessCurve (float p) {
		return Mathf.Pow (p, 3);
	}

	/*readonly float sicknessDuration;
	bool sick = false;
	int sicknessOpportunity = 0;
	int sicknessOpportunitiesCount = 8;
	List<float> sicknessOpportunities = new List<float> ();
	List<float> sicknessLikelihood = new List<float> ();

	public HealthManager () {
		sicknessDuration = TimerValues.Death/2;
		SetSicknessOpportunities ();
	}

	void SetSicknessOpportunities () {
		float separation = 1f/(float)(sicknessOpportunitiesCount+1);
		for (int i = 0; i < sicknessOpportunitiesCount; i ++) {
			float startIndex = (float)(i+1);
			sicknessOpportunities.Add (separation * startIndex);
			sicknessLikelihood.Add (Mathf.Pow (startIndex/(float)(sicknessOpportunitiesCount+1), 1.5f));
		}
	}

	public void OnAge (float progress) {
		if (sicknessOpportunity > sicknessOpportunities.Count-1) return;
		if (progress >= sicknessOpportunities[sicknessOpportunity]) {
			BecomeSick ();
			sicknessOpportunity ++;
		}
	}

	void BecomeSick () {
		if (sick) return;
		float val = Random.value;
		Debug.Log (val + " < " + sicknessLikelihood[sicknessOpportunity]);
		if (val <= sicknessLikelihood[sicknessOpportunity]) {
			sick = true;
			Coroutine.Instance.StartCoroutine (sicknessDuration, ReduceHealth, EndSickness);
		}
	}

	void ReduceHealth (float progress) {
		Health = Mathf.Abs (progress-1);
		Debug.Log (Health);
	}

	void EndSickness () {
		sick = false;
		Debug.Log ("done");
	}*/
}
