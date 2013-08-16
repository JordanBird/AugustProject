using System.Collections;

public class Answer
{
	public bool correct;
	public string text;
	
	//Type, 0 = Text, 1 = Image.
	public int type = 0;
	
	public Answer(bool iCorrect, string iText)
	{
		text = iText;
		correct = iCorrect;
	}
}