using UnityEngine;

public static class Palette {

	public static Color Magenta {
		get { return new Color (0.694f, 0.235f, 0.443f); }
	}

	public static Color Green {
		get { return new Color (0.569f, 0.792f, 0.251f); }
	}

	public static Color Tan {
		get { return new Color (0.941f, 0.796f, 0.431f); }
	}

	public static Color Blue {
		get { return new Color (0.25f, 0, 0.847f); }
	}

	public static Color ApplyAlpha (Color c, float alpha) {
		return new Color (c.r, c.g, c.b, alpha);
	}
}
