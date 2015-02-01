using System.Collections;

namespace GameActions {
	
	// Runs Actions
	public interface IActionable {

		ActionsList ActionsList { get; set; }
		void OnEndAction ();
	}
}