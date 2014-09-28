using UnityEngine;
using System.Collections;

public class RowCreator : MonoBehaviour {
	
	public GameObject row;
	private Row[] rows;
	public Row[] Rows {
		get { return rows; }
	}

	public void Init () {
		int rowCount = 10;
		rows = new Row[rowCount];
		for (int i = 0; i < rowCount; i ++) {
			rows[i] = CreateRow (i);
		}
	}

	private Row CreateRow (int index) {
		GameObject go = Instantiate (row) as GameObject;
		Row r = go.GetComponent<Row>();
		r.Init (index);
		return r;
	}
}