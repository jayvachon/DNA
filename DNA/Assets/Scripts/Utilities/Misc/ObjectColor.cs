using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Renderer))]
public class ObjectColor : MonoBehaviour {

	new Renderer renderer = null;
	Renderer Renderer {
		get {
			if (renderer == null) {
				renderer = GetComponent<Renderer> ();
			}
			return renderer;
		}
	}

	string primaryColor = "default";
	public string PrimaryColor {
		get { return primaryColor; }
		set { primaryColor = value; }
	}

	public string CurrentColor { get; private set; }

	Dictionary<string, Color> colors = new Dictionary<string, Color> {
		{ "default", Color.white }
	};

	float alpha = 1f;

	protected virtual void OnEnable () {
		if (PrimaryColor != null) {
			SetColor (PrimaryColor);
		}
	}

	public void AddColor (string id, Color c) {
		if (colors.ContainsKey (id)) {
			colors[id] = c;
		} else {
			colors.Add (id, c);
		}
	}

	public void SetColor (string id="", bool setChildren=true) {
		if (id == "") id = PrimaryColor;
		try {
			Renderer.SetColor (colors[id]);
			if (setChildren)
				SetColorInChildren ();
			CurrentColor = id;
		} catch {
			throw new System.Exception ("No color with the id '" + id + "' exists");
		}
	}

	public void SetAlpha (float newAlpha) {
		if (newAlpha != alpha) {
			Color c = GetColor (CurrentColor);
			Color newColor = new Color (c.r, c.g, c.b, newAlpha);
			Renderer.SetColor (newColor);
			SetColorInChildren ();
			alpha = newAlpha;
		}
	}

	public Color GetColor (string id) {
		Color c;
		if (colors.TryGetValue (id, out c)) {
			return c;
		} else {
			throw new System.Exception ("No color with the id '" + id + "' exists");
		}
	}

	public void Hide () {
		Renderer.enabled = false;
	}

	public void Show () {
		Renderer.enabled = true;
	}

	void SetColorInChildren () {
		List<Transform> children = transform.GetAllChildren ();
		foreach (Transform child in children) {
			Renderer childRenderer = child.GetComponent<Renderer> ();
			if (childRenderer != null)
				childRenderer.sharedMaterial = Renderer.sharedMaterial;
		}
	}
}
