using UnityEngine;
using System.Collections;

public class ProductionTier {

	public readonly int Index;
	int[] costs = new [] { 5, 15, 30, 60, 120 };
	int[] returns = new [] { 15, 30, 60, 120, 240 };
	Color[] colors = new [] { Color.grey, Color.white, Color.yellow, Color.green, Color.blue };

	public int Cost { get { return costs[Index]; } }
	public int Return { get { return returns[Index]; } }
	public Color Color { get { return colors[Index]; } }

	public ProductionTier (int index) {
		Index = index;
		if (Index > costs.Length)
			throw new System.Exception ("Index out of range.");
	}
}
