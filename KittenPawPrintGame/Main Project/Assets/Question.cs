using System;
using System.Collections;

public class Question
{
	public string text;
	public Answer[] answers;
	
	Random random = new Random();
	
	public Question (string iText, Answer[] iAnswers)
	{
		text = iText;
		answers = iAnswers;
	}
	
	public Answer GetRandomAnswer()
	{
		return answers[random.Next (0, answers.Length)];
	}
}
