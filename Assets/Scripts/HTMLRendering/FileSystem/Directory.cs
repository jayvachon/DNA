using System.Collections;
using System.Collections.Generic;

public class Directory : File {

	File[] files;

	public Directory () {

	}

	public void SetFiles (File[] files) {
		this.files = files;
	}
}