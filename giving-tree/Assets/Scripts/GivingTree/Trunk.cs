using UnityEngine;
using System.Collections;

public class Trunk : MonoBehaviour {

	public float radius;
	public float height;

	Helix helix = null;
	public Helix Helix {
		get { 
			if (helix == null) {
				helix = new Helix (radius, height);
			}
			return helix; 
		}
	}
}
