using System.Collections;

namespace GameInput {
					 
	public interface IReleasable {
		void OnRelease (ReleaseSettings releaseSettings);
	}
}