using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FileDrawer : MonoBehaviour {

	public Canvas canvas;
	public Text h1Obj;
	public Text paragraphObj;

	File renderFile;

	void Start () {
		renderFile = new TestPage ();
		CreateUIElements ();
	}

	void CreateUIElements () {
		PageElement[] elements = renderFile.Elements;
		foreach (PageElement element in elements) {
			CreateHeader (element);
			CreateParagraph (element);
		}
	}

	void CreateHeader (PageElement element) {
		if (element is H1) {
			H1 h1 = element as H1;
			Text newh1 = Instantiate (h1Obj, canvas.transform.position, Quaternion.identity) as Text;
			newh1.transform.SetParent (canvas.transform, true);
			newh1.text = h1.text;
		}
	}

	void CreateParagraph (PageElement element) {
		if (element is Paragraph) {
			Paragraph p = element as Paragraph;
			Text newp = Instantiate (paragraphObj, canvas.transform.position, Quaternion.identity) as Text;
			newp.transform.SetParent (canvas.transform, true);
			newp.text = p.text;
		}
	}

	void CreateUl (PageElement element) {
		if (element is Ul) {
			Ul ul = element as Ul;
			foreach (ListItem li in ul.Items) {
				
			}
		}
	}
}
