using UnityEngine;
using System.Collections;
using Units;

namespace DNA.Tasks {
	public class GenerateUnitTest<T> : GenerateUnit<T> where T : Unit {}
}