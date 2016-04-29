using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DNA {

	public class MonthTimer : UIElement {

		public bool roundTimer = true;

		void Update () {
			if (roundTimer)
				Image.fillAmount = LoanManager.Time;
			else
				transform.SetLocalScaleY (LoanManager.Time);
		}
	}
}