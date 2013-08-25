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
	
	public void RandomizeAnswers()
	{
		for (int i = 1; i < answers.Length; i++)
        {
            int pos = random.Next(i + 1);
            Answer tempAnswer = answers[i];
            answers[i] = answers[pos];
            answers[pos] = tempAnswer;
        }
	}
}