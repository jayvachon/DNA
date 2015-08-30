using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void OnDie ();

public class HealthManager {

	public OnDie onDie;

	float health = 1f;
	public float Health {
		get { return health; }
		private set { health = value; }
	}
	float healthAtSicknessOnset;

	bool canBecomeSick = true;
	public bool CanBecomeSick { 
		get { return canBecomeSick; }
		set { canBecomeSick = value; }
	}

	public bool Sick { get; private set; }

	public void StartSickness () {
		if (Sick || !CanBecomeSick) return;
		Debug.Log ("starting sickness");
		healthAtSicknessOnset = Health;
		Sick = true;
		//Coroutine.Instance.StartCoroutine (30f, OnSickness, OnEndSickness);
	}

	public void StopSickness () {
		Sick = false;
		//Coroutine.Instance.StopCoroutine (OnSickness);
	}

	void OnSickness (float progress) {
		Health = Mathf.Abs (SicknessCurve (progress)-1) * healthAtSicknessOnset;
	}

	void OnEndSickness () {
		Debug.Log (Health);
		if (Health <= 0.001f) {
			if (onDie != null) onDie ();
		}
	}

	float SicknessCurve (float p) {
		//return Mathf.Pow (p, 3);
		return p;
	}
}
