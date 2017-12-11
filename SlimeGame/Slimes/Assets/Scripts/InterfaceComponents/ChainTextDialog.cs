using System.Collections.Generic;

public class ChainTextDialog : TipDialog
{

	private List<string> texts;
	private int currentTextIndex;
	protected OnClickOkDialog g;

	public ChainTextDialog(){
		texts = new List<string> ();
		texts.Add ("Texto 1");
		texts.Add ("Texto 2");
		texts.Add ("Texto 3");
		currentTextIndex = 0;
		SetInfoTextText(texts[currentTextIndex]);
		this.f = () => {
			this.Show();
			currentTextIndex++;
			if(currentTextIndex>=texts.Count){
				this.Hide();
				currentTextIndex = 0;
				g();
			}
			this.SetInfoTextText(texts[currentTextIndex]);
		};
		g = () => {
		};
	}

	public void SetOnClickFunction(OnClickOkDialog f){
		this.g = f;
	}

	public void SetTextList(List<string> texts){
		this.texts = texts;
		currentTextIndex = 0;
		this.SetInfoTextText (this.texts [currentTextIndex]);
	}
}

