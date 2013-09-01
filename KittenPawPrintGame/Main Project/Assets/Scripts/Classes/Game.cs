using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;


public class Game
{
	public string name;
	public string author;
	public string type;
	
	public Texture2D background;
	
	public Question[] questions;
	
	public string location = "";

	string dataPath;
	
	public Game(string iName, string iAuthor, string iType, Question[] iQuestions, Texture2D iBackground)
	{
		background = iBackground;
		dataPath = GameObject.FindGameObjectWithTag ("Master").GetComponent<cscript_master>().dataPath;
		
		name = iName;
		author = iAuthor;
		type = iType;
		questions = iQuestions;
	}
	
	public Game(string XMLLocation, bool resource)
	{
		location = XMLLocation;
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

		//Add background.
		XmlElement eBackground = document.CreateElement( "", "background", "" );
		XmlText tBackground = document.CreateTextNode(System.Text.Encoding.GetEncoding (1252).GetString(background.EncodeToPNG () ));
		eBackground.AppendChild( tBackground );
        eGame.AppendChild( eBackground );

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
				
				//Adds the type.
				XmlElement eAType = document.CreateElement( "", "aType", "" );
				XmlText tAType = document.CreateTextNode( questions[i].answers[a].type.ToString () );
				eAType.AppendChild( tAType );
        		eAnswer.AppendChild( eAType );
				
				//Adds text.
				XmlElement eText = document.CreateElement( "", "aText", "" );
				XmlText tAnswer = document.CreateTextNode( questions[i].answers[a].text );
				eText.AppendChild( tAnswer );
        		eAnswer.AppendChild( eText );
				
				//Adds texture.
				XmlElement eTexture = document.CreateElement( "", "aTexture", "" );
				XmlText tTexture;
				
				if (questions[i].answers[a].texture != null)
					tTexture = document.CreateTextNode( System.Text.Encoding.GetEncoding (1252).GetString(questions[i].answers[a].texture.EncodeToPNG ()) );
				else
					tTexture = document.CreateTextNode("");
				
				eTexture.AppendChild( tTexture );
        		eAnswer.AppendChild( eTexture );
				
				//Adds if correct or not.
				XmlElement eCorrect = document.CreateElement( "", "correct", "" );
				XmlText tCorrect = document.CreateTextNode( questions[i].answers[a].correct.ToString () );
				eCorrect.AppendChild( tCorrect );
        		eAnswer.AppendChild( eCorrect );
			}
		}
		
		document.Save (dataPath + @"/Game Files/" + name + ".xml");
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
		
		if (document.SelectSingleNode ("//background").InnerText != "")
		{
			background = new Texture2D(1, 1);
			background.LoadImage (System.Text.Encoding.GetEncoding (1252).GetBytes (document.SelectSingleNode ("//background").InnerText));
			Debug.Log ("Success");
		}
		else
		{
			background = GameObject.FindGameObjectWithTag ("GUI Master").GetComponent<cscript_GUI_master>().blankBlackTexture;
		}
		
		List<Question> questionList = new List<Question>();
		
		foreach (XmlNode x in document.GetElementsByTagName("question"))
		{
			//Gets actual question.
			string xName = x["text"].InnerText;
			List<Answer> answers = new List<Answer>();
			
			//Gets answers.
			//foreach (XmlNode y in  x.SelectNodes ("//answers//answer"))
			foreach (XmlNode y in  x["answers"])
			{
				bool correct = false;
				
				if (y["correct"].InnerText == "True")
					correct = true;
				else
					correct = false;
				try
				{
					if (y["aType"].InnerText == "0")
					{
						//Text
						answers.Add (new Answer(correct, y["aText"].InnerText));
					}
					else
					{
						//Image
						Texture2D texture = new Texture2D(1, 1);
						texture.LoadImage (System.Text.Encoding.GetEncoding (1252).GetBytes (y["aTexture"].InnerText));
						
						answers.Add (new Answer(correct, texture));
					}
				}
				catch
				{
					answers.Add (new Answer(correct, y["aText"].InnerText));
				}
			}
			
			questionList.Add (new Question(xName, answers.ToArray ()));
		}
		
		questions = questionList.ToArray ();
	}
	
	public Question GetRandomQuestion()
	{
		return questions[UnityEngine.Random.Range (0, questions.Length)];
	}
}