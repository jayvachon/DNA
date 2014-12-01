using System.Collections;
using FauxWeb;

public class TestPage : WebPage {

	public TestPage () {
		
		Header1 h1 = new Header1 ("A Header For Your Troubles");
		Paragraph p1 = new Paragraph (
			new TextType[] {
				new DefaultContent ("Here's some test text for testing purposes and if i have a lot of it then it maybeaerwe kind of works turnkey "),
				new Hyperlink ("whoa there"),
				new DefaultContent (" now there's text after the link even tho it goes on for way too long "),
				new Hyperlink ("this is another link for all you bald people out there"),
				new DefaultContent (" i really want toual to have a goreat day")
			}
		);
		Paragraph p2 = new Paragraph ("paragraph2 begins NO!O!W");
		UnorderedList ul = new UnorderedList (new ListItem[] {
			new ListItem (p1)//,
			//new ListItem (p2)
		});
		Elements = new PageElement[] {
			h1, p1, p2
			//ul
		};
	}
}
