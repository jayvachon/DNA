using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DNA {

	public class MonthTimer : UIElement {

		void Update () {
			Image.fillAmount = LoanManager.Time;
		}
	}
}