using System.Collections;

namespace GameActions {
	
	// Can run Actions
	public interface IActionable {

		ActionsList ActionsList { get; set; }
		void OnEndAction ();
	}
}