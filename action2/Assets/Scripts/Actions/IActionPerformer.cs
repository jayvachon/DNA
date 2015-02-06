using System.Collections;

namespace GameActions {

	public interface IActionPerformer {
		PerformableActions PerformableActions { get; }
		void OnEndAction ();
	}
}
