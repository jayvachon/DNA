using System.Collections;

public class RootDirectory : Directory {

	public RootDirectory () : base () {
		SetFiles (new File[] {
			new BuildDirectory (),
			new TestPage ()
		});
	}
}