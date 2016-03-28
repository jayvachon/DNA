using UnityEngine;

public static class Palette {

	public static Color Pink {
		get { return new Color (1f, 0.5f, 1f); }
	}

	public static Color Magenta {
		get { return new Color (0.694f, 0.235f, 0.443f); }
	}

	public static Color YellowGreen {
		get { return new Color (0.635f, 0.96f, 0.373f); }
	}

	public static Color Green {
		get { return new Color (0.569f, 0.792f, 0.251f); }
	}

	public static Color DarkGreen {
		get { return  new Color (0.137f, 0.294f, 0.235f); }
	}

	public static Color Tan {
		get { return new Color (0.941f, 0.796f, 0.431f); }
	}

	public static Color Sea {
		get { return new Color (0.126f, 0.227f, 0.847f); }
	}

	public static Color DarkBlue {
		get { return new Color (0.133f, 0.161f, 0.413f); }
	}

	public static Color Blue {
		get { return new Color (0.25f, 0, 0.847f); }
	}

	public static Color White {
		get { return new Color (1f, 1f, 1f); }
	}

	public static Color Yellow {
		get { return new Color (1f, 0.898f, 0.231f); }
	}

	public static Color Black {
		get { return new Color (0.2f, 0.2f, 0.2f); }
	}

	public static Color Red {
		get { return new Color (1f, 0f, 0f); }
	}

	public static Color Grey {
		get { return new Color (0.67f, 0.67f, 0.67f); }
	}

	public static Color ApplyAlpha (Color c, float alpha) {
		return new Color (c.r, c.g, c.b, alpha);
	}
}
