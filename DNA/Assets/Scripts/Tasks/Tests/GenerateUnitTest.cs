using UnityEngine;
using System.Collections;
using DNA.Units;

namespace DNA.Tasks {
	public class GenerateUnitTest<T> : GenerateUnit<T> where T : Unit {}
}