using System.Collections;

public class Game
{
	public string name;
	public string author;
	public string type;
	public string question;
	
	public Answer[] answers;
	
	public Game(string iName, string iAuthor, string iType, string iQuestion, Answer[] iAnswers)
	{
		name = iName;
		author = iAuthor;
		type = iType;
		question = iQuestion;
		answers = iAnswers;
	}
}
