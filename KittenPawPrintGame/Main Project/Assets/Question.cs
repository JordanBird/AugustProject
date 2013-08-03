using System.Collections;

public class Question
{
	public string text;
	
	public Answer[] answers;
	
	public Question (string iText, Answer[] iAnswers)
	{
		text = iText;
		answers = iAnswers;
	}
}
