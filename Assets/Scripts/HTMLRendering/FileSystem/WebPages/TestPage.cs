using System.Collections;

public class TestPage : WebPage {

	public TestPage () {
		H1 h1 = new H1 ("Headeraaaaggghhh");
		Paragraph p1 = new Paragraph ("here's some text");
		Paragraph p2 = new Paragraph ("here's some more text eh");
		Ul ul = new Ul (new Li[] {
			new Li (p1),
			new Li (p2)
		});
		Elements = new PageElement[] {
			h1, p1, p2
		};
	}
}
