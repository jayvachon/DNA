using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowPassFilter {

	Queue<float> buffer = new Queue<float> ();

	int bufferSize = 10;
	public int BufferSize {
		get { return bufferSize; }
		set { bufferSize = value; }
	}

	public float Output {
		get {
			float total = 0f;
			foreach (float val in buffer) 
				total += val;
			return total / (float)buffer.Count;
		}
	}

	public float InputSignal (float val) {
		buffer.Enqueue (val);
		if (buffer.Count > bufferSize) {
			buffer.Dequeue ();
		}
		return Output;
	}

	public void Reset () {
		buffer.Clear ();
	}
}