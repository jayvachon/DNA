using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MaterialsExtended {

	private static List<Material> materials = new List<Material>();

	public static void SetColor ( this Renderer renderer, Color color ) {
		renderer.sharedMaterial = ColoredMaterial ( color );
	}

	public static void SetColors ( this Renderer renderer, Color[] colors ) {
		renderer.sharedMaterials = ColoredMaterials ( colors );
	}

	// Get a material with the given color
	public static Material ColoredMaterial ( Color color ) {

		if ( materials.Count == 0 ) 
			return AddNewMaterial ( color );

		foreach ( Material m in materials ) {
			if ( m.color == color ) return m;
		}

		return AddNewMaterial ( color );

	}

	// Get an array of materials with the given colors
	public static Material[] ColoredMaterials ( Color[] colors ) {

		Material[] m = new Material[ colors.Length ];
		for ( int i = 0; i < m.Length; i ++ ) {
			m[i] = ColoredMaterial ( colors [i] );
		}
		return m;

	}

	private static Material AddNewMaterial ( Color color ) {

		materials.Add ( CreateMaterial ( color ));
		return materials[ materials.Count - 1];

	}

	private static Material CreateMaterial ( Color color ) {

		Material m;
		if ( color.a > 0.99 ) {
			m = new Material (Shader.Find ("Standard"));
			m.SetFloat("_Smoothness", 0);
		} else {
			m = new Material (Shader.Find ("Standard"));
			m.SetFloat("_Mode", 2);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = 3000;
		}
		m.SetColor ("_Color", color);
		
		return m;
	}
}
