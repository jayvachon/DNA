
namespace GameInput {
	
	public interface IClickable {

		// If an "IgnoreLayer" is hit in the same click,then OnClick will not be called.
		// This is useful for e.g. when the UI is clicked and you
		// don't want the thing underneath it to register a click
		InputLayer[] IgnoreLayers { get; }
		void OnClick (ClickSettings clickSettings);
	}
}