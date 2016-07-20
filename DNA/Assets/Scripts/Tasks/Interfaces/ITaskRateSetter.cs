
namespace DNA.Tasks {

	// TODO: deprecate this (used by MilkshakePool and Laborer)
	public interface ITaskRateSetter {
		float TaskRate { get; } // How quickly a task is performed (%)
	}
}
