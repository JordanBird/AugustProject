using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Game
{
	public string name;
	public string author;
	public string type;
	
	public Question[] questions;
	
	Random random = new Random();
	
	public Game(string iName, string iAuthor, string iType, Question[] iQuestions)
	{
		name = iName;
		author = iAuthor;
		type = iType;
		questions = iQuestions;
	}
	
	public Game(string XMLLocation, bool resource)
	{
		XMLToGame (XMLLocation, resource);
	}
	
	public void GameToXML()
	{
		XmlDocument document = new XmlDocument();
		
		//Creates main body.
		XmlElement eGame = document.CreateElement( "", "game", "" );
        document.AppendChild( eGame );
		
		//Adds the games name to the file.
		XmlElement eName = document.CreateElement( "", "name", "" );
		XmlText tName = document.CreateTextNode( name );
		eName.AppendChild( tName );
        eGame.AppendChild( eName );
		
		//Adds author.
		XmlElement eAuthor = document.CreateElement( "", "author", "" );
		XmlText tAuthor = document.CreateTextNode( author );
		eAuthor.AppendChild( tAuthor );
        eGame.AppendChild( eAuthor );
		
		//Add type.
		XmlElement eType = document.CreateElement( "", "type", "" );
		XmlText tType = document.CreateTextNode( type );
		eType.AppendChild( tType );
        eGame.AppendChild( eType );
		
		//Creates question body.
		XmlElement eQuestions = document.CreateElement( "", "questions", "" );
        eGame.AppendChild( eQuestions );
		
		for (int i = 0; i < questions.Length; i++)
		{
			//Creates specific question body.
			XmlElement eQuestion = document.CreateElement( "", "question", "" );
        	eQuestions.AppendChild( eQuestion );
			
			//Adds the actual question.
			XmlElement eQText = document.CreateElement( "", "text", "" );
			XmlText tQuestion = document.CreateTextNode( questions[i].text );
			eQText.AppendChild( tQuestion );
			eQuestion.AppendChild( eQText );
			
			//Adds answers body.
			XmlElement eAnswers = document.CreateElement( "", "answers", "" );
        	eQuestion.AppendChild( eAnswers );
			
			for (int a = 0; a < questions[i].answers.Length; a++)
			{
				//Adds specific answer body.
				XmlElement eAnswer = document.CreateElement( "", "answer", "" );
        		eAnswers.AppendChild( eAnswer );
				
				//Adds answer text - Will be image later in development.
				XmlElement eText = document.CreateElement( "", "aText", "" );
				XmlText tAnswer = document.CreateTextNode( questions[i].answers[a].text );
				eText.AppendChild( tAnswer );
        		eAnswer.AppendChild( eText );
				
				//Adds if correct or not.
				XmlElement eCorrect = document.CreateElement( "", "correct", "" );
				XmlText tCorrect = document.CreateTextNode( questions[i].answers[a].correct.ToString () );
				eCorrect.AppendChild( tCorrect );
        		eAnswer.AppendChild( eCorrect );
			}
		}
		
		document.Save (cscript_master.dataPath + @"\Game Files\" + name + ".xml");
	}
	
	public void XMLToGame(string location, bool resource)
	{
		XmlDocument document = new XmlDocument();
		
		if (resource == true)
		{
			document.LoadXml (location);
		}
		else
		{
			document.Load (location);
		}

		//Reads in name, author and type.
		name = document.SelectSingleNode ("//name").InnerText;
		author = document.SelectSingleNode ("//author").InnerText;
		type = document.SelectSingleNode ("//type").InnerText;
		
		List<Question> questionList = new List<Question>();
		
		foreach (XmlNode x in document.GetElementsByTagName("question"))
		{
			//Gets actual question.
			string xName = x.SelectSingleNode ("//text").InnerText;
			List<Answer> answers = new List<Answer>();
			
			//Gets answers.
			foreach (XmlNode y in x.SelectNodes ("//answer"))
			{
				if (y["correct"].InnerText == "True")
					answers.Add (new Answer(true, y["aText"].InnerText));
				else
					answers.Add (new Answer(false, y["aText"].InnerText));
			}
			
			questionList.Add (new Question(xName, answers.ToArray ()));
		}
		
		questions = questionList.ToArray ();
	}
	
	public Question GetRandomQuestion()
	{
		return questions[random.Next(0, questions.Length)];
	}
}
