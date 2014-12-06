using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FauxWeb;

public class FileDrawer : MonoBehaviour {

	public Canvas canvas;
	public Text textObj;

	float yPos = 0f;
	float width = 500f;
	Vector2 lastPosition = Vector2.zero;

	File renderFile;
	Text textSizer;

	void Start () {
		renderFile = new TestPage ();
		textSizer = Instantiate (textObj) as Text;
		CreateUIElements ();
	}

	void CreateUIElements () {
		PageElement[] elements = renderFile.Elements;
		foreach (PageElement element in elements) {
			CreateUIElement (element);
		}
	}

	void CreateUIElement (PageElement element) {
		if (element is Header1) {
			CreateTextElement (element as Header1);
		} else if (element is Paragraph) {
			CreateTextElement (element as Paragraph);
		} else if (element is UnorderedList) {
			CreateUnorderedList (element as UnorderedList);
		}
	}

	// This works, but is super ugly
	void CreateTextElement (TextElement te) {
		
		float lineHeight = 0;
		float lastLineHeight = 0;

		for (int i = 0; i < te.Contents.Length; i ++) {
			
			float lineWidth = width - lastPosition.x;

			TextType tc = te.Contents[i];
			Text t = CreateText (tc, lastPosition, lineWidth);
			lineHeight = t.preferredHeight;
			TextGenerationSettings tgs = t.GetGenerationSettings (new Vector2 (lineWidth, lineHeight));

			var tgen = t.cachedTextGenerator;
			tgen.Populate (t.text, tgs);

			if (tgen.lineCount > 1 && lineWidth < width) {
				
				int breakIndex = tgen.lines[1].startCharIdx;
				int lastIndex = t.text.Length;
				string newText = t.text.Substring (breakIndex, lastIndex-breakIndex);

				// check if we split the string on a word
				if (!t.text.Substring(0, breakIndex).Contains (" ")) {
					newText = t.text;
					t.text = "";
				} else {
					t.text = t.text.Substring (0, breakIndex);
				}
				
				TextType tc2 = tc;
				tc2.Text = newText;

				lastPosition.x = 0;
				lastPosition.y -= t.preferredHeight;
				t = CreateText (tc2, lastPosition, width);
				lineHeight = t.preferredHeight;

				tgs = t.GetGenerationSettings (new Vector2 (width, lineHeight));
				tgen = t.cachedTextGenerator;
				tgen.Populate (tc2.Text, tgs);

				lastLineHeight = tc2.LineHeight;
			} else {
				lastLineHeight = tc.LineHeight;
			}

			Vector2 newLastPosition = tgen.verts[(tgen.characterCount-1)*4].position;
			lastPosition.x += newLastPosition.x;
			lastPosition.y += newLastPosition.y;
		}

		lastPosition.x = 0;
		lastPosition.y -= lastLineHeight;
	}

	Text CreateText (TextType tc, Vector2 pos, float lineWidth) {
		
		Text t = Instantiate (textObj, canvas.transform.position, Quaternion.identity) as Text;
		t.transform.SetParent (canvas.transform, true);

		// Text and Font
		t.text = tc.Text;
		t.fontStyle = tc.Style;
		t.fontSize = tc.FontSize;
		t.color = tc.FontColor;

		// Size
		Vector2 sizeDelta = new Vector2 (lineWidth, t.preferredHeight);
		t.rectTransform.sizeDelta = sizeDelta;

		// Position
		Vector3 position = tc.AnchorPosition;
		position.x += pos.x;
		position.y += pos.y;
		t.rectTransform.pivot = tc.Pivot;
		t.rectTransform.anchorMax = tc.AnchorMax;
		t.rectTransform.anchorMin = tc.AnchorMin;
		t.rectTransform.anchoredPosition = position;
		
		return t;
	}

	void CreateUnorderedList (UnorderedList ul) {
		for (int i = 0; i < ul.Items.Length; i ++) {
			CreateUIElement (ul.Items[i].Element);
		}
	}
}
