using System.Collections;
using FauxWeb;

public class RootDirectory : Directory {

	public RootDirectory () : base () {
		SetFiles (new File[] {
			new BuildDirectory (),
			new TestPage ()
		});
	}
}