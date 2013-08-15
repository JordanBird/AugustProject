using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class cscript_navigation : MonoBehaviour 
{
	cscript_master master;
	
	Texture2D blankWhiteTexture = new Texture2D(1, 1);
	Texture2D blankBlackTexture = new Texture2D(1, 1);
	
	//Main Menu Variables
	public GUISkin game;
	
	Game[] games;
	
	int position = 0;
	
	//Create Game Variables
	public GUISkin correctAnswer;
	public GUISkin incorrectAnswer;
	
	string gameName = "Add Game Name Here";
	string authorName = "Add Author Here";
	string multipleChoiceQuestion = "Add Multiple Choice Question Here";
	string answer = "Add Answer Text Here";
	
	int selectedCreateGame = 0;
	
	List<Answer> answers = new List<Answer>();
	List<Question> questions = new List<Question>();
	
	bool addQuestion = false;
	
	int questionPosition = 0;
	
	FancyButton[] buttons = new FancyButton[12];
	
	public void Init(cscript_master m)
	{
		master = m;
	}
	
	// Use this for initialization
	void Start ()
	{
		blankWhiteTexture.SetPixel (0, 0, Color.white);
		blankWhiteTexture.Apply();
			
		blankBlackTexture.SetPixel (0, 0, Color.black);
		blankBlackTexture.Apply();
		
		correctAnswer = Resources.Load ("GUI Skins/GUISkin_Correct_Answer") as GUISkin;
		incorrectAnswer = Resources.Load ("GUI Skins/GUISkin_Incorrect_Answer") as GUISkin;
		game = Resources.Load ("GUI Skins/GUISkin_Main_Menu_Games") as GUISkin;
		
		List<Game> tempGames = new List<Game>();
		
		tempGames.Add (new Game((Resources.Load ("Game Files/Multiplication Football") as TextAsset).ToString (), true));
		tempGames.Add (new Game((Resources.Load ("Game Files/Fly to the Noun") as TextAsset).ToString (), true));
		tempGames.Add (new Game((Resources.Load ("Game Files/Word Train") as TextAsset).ToString (), true));
		
		Directory.CreateDirectory (cscript_master.dataPath + @"/Game Files");
		
		foreach (string s in Directory.GetFiles (cscript_master.dataPath + @"/Game Files"))
		{
			try
			{
				tempGames.Add (new Game(s, false));
			}
			catch {}
		}
		
		games = tempGames.ToArray ();
		
		buttons[0] = new FancyButton("About", 10, 10, 100, 30, 0.8f, 0);
		buttons[1] = new FancyButton("Help", Screen.width - 110, 10, 100, 30, 0.8f, 0);
		buttons[2] = new FancyButton("Create New Game", Screen.width / 2 - 350, Screen.height - 55, 700, 40, 0.8f, 2);
			
		float inc = (Screen.width - 40) / 3;
		buttons[3] = new FancyButton("Play", 10 + inc / 4, Screen.height - 110, (int)inc / 2, 40, 0.5f, 2);
		buttons[4] = new FancyButton("Play", inc + 10 * 2 + inc / 4, Screen.height - 110, (int)inc / 2, 40, 0.5f, 2);
		buttons[5] = new FancyButton("Play", 2 * inc + 10 * 3 + inc / 4, Screen.height - 110, (int)inc / 2, 40, 0.5f, 2);
		buttons[6] = new FancyButton("Edit", 10, Screen.height - 100, (int)inc / 6, 20, 0.3f, 2);
		buttons[7] = new FancyButton("Edit", inc + 10 * 2, Screen.height - 100, (int)inc / 6, 20, 0.3f, 2);
		buttons[8] = new FancyButton("Edit", 2 * inc + 10 * 3, Screen.height - 100, (int)inc / 6, 20, 0.3f, 2);
		
		buttons[9] = new FancyButton("<", 10, Screen.height / 2 - 15, 50, 30, 0.8f, 3);
		buttons[10] = new FancyButton(">", Screen.width - 60, Screen.height / 2 - 15, 50, 30, 0.8f, 1);
		
		buttons[11] = new FancyButton("< Back", 10, 10, 100, 30, 0.8f, 0);
		
		buttons[0].Enter ();
		buttons[1].Enter ();
		buttons[2].Enter ();
		buttons[3].Enter ();
		buttons[4].Enter ();
		buttons[5].Enter ();
		buttons[6].Enter ();
		buttons[7].Enter ();
		buttons[8].Enter ();
		buttons[9].Enter ();
		buttons[10].Enter ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].Update();
		}
	}
	
	void OnGUI()
	{		
		switch (master.gameState)
		{
			case cscript_master.GameState.MainMenu:
				MainMenuGUI ();
				break;
			case cscript_master.GameState.Help:
				HelpGUI ();
				break;
			case cscript_master.GameState.About:
				AboutGUI ();
				break;
			case cscript_master.GameState.CreateGame:
				CreateGameGUI ();
				break;
			case cscript_master.GameState.Playing:
				break;
				
		}
		
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].Clicked = GUI.Button (buttons[i].GetRectangle(), buttons[i].Text);
		}
	}
	
	private void MainMenuGUI()
	{		
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "Main Menu");
		
		if (buttons[0].Clicked)
		{
			master.gameState = cscript_master.GameState.About;
			
			for (int i = 0; i < 11; i++)
			{
				buttons[i].Leave();
			}
			
			buttons[11].Enter ();
		}
		
		if (buttons[1].Clicked)
		{
			master.gameState = cscript_master.GameState.Help;
			
			for (int i = 0; i < 11; i++)
			{
				buttons[i].Leave();
			}
			
			buttons[11].Enter ();
		}
		
		if (buttons[2].Clicked)
		{
			master.gameState = cscript_master.GameState.CreateGame;
			
			for (int i = 0; i < 11; i++)
			{
				buttons[i].Leave();
			}
			
			buttons[1].Enter ();
			buttons[11].Enter ();
			
			ResetGameCreation ();
		}
		
		float inc = (Screen.width - 40) / 3;
		
		//Playable Games
		
		for (int i = 0; i < 3; i++)
		{
			GUI.DrawTexture (new Rect(i * inc + 9 + (i * 10), 59, inc + 2, Screen.height - 178), blankBlackTexture);
			GUI.DrawTexture (new Rect(i * inc + 10 * (i + 1), 60, inc, Screen.height - 180), blankWhiteTexture);
			
			GUI.Label (new Rect(i * inc + 10 * (i + 1), 60, inc, Screen.height - 180), games[i + position].name, game.label);
			GUI.Label (new Rect(i * inc + 10 * (i + 1), Screen.height - 160, inc, 20), "By " + games[i + position].author, game.label);
			
			if (buttons[3 + i].Clicked)
			{
				//Launch games[i + position]
			}
			
			//Edit Game
			if (i + position < 3)
			{
				//buttons[6 + i].Hide(); // INSTANT
				buttons[6 + i].Leave ();
			}
			else
			{
				//buttons[6 + i].Unhide(); // INSTANT
				buttons[6 + i].Enter();	
				
				if (buttons[6 + i].Clicked)
				{
					gameName = games[i + position].name;
					authorName = games[i + position].author;
					selectedCreateGame = Convert.ToInt32(games[i + position].type);
					questions.AddRange (games[i + position].questions);
					
					master.gameState = cscript_master.GameState.CreateGame;
				}
			}
		}
		
		if (buttons[9].Clicked)
		{			
			if (position > 0)
				position--;
			
		}
		
		if (buttons[10].Clicked)
		{
			if (position < games.Length - 3)
				position++;
		}
	}
	
	private void HelpGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "Help");
		
		if (buttons[11].Clicked)
		{
			master.gameState = cscript_master.GameState.MainMenu;
			
			for (int i = 0; i < 11; i++)
			{
				buttons[i].Enter();
			}
			
			buttons[11].Leave ();
		}
	}
	
	private void AboutGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "About");
		
		if (buttons[11].Clicked)
		{
			master.gameState = cscript_master.GameState.MainMenu;
			
			for (int i = 0; i < 11; i++)
			{
				buttons[i].Enter();
			}
			
			buttons[11].Leave ();
		}
	}
	
	private void CreateGameGUI()
	{
		if (addQuestion == false)
		{
			GUI.Label(new Rect(Screen.width / 2 - 60, 10, 120, 50), "Create your Game");
		
			if (buttons[11].Clicked)
			{
				master.gameState = cscript_master.GameState.MainMenu;
					
				for (int i = 0; i < 11; i++)
				{
					buttons[i].Enter();
				}
				
				buttons[11].Leave ();
			}
			
			if (buttons[1].Clicked)
			{
				master.gameState = cscript_master.GameState.Help;
				
			}
			
			gameName = GUI.TextField (new Rect((Screen.width / 4 - 10) / 2, 60, Screen.width / 4, 20), gameName);
			authorName = GUI.TextField (new Rect((Screen.width / 4 - 10) * 2.5f, 60, Screen.width / 4, 20), authorName);
			
			GUIContent[] g = new GUIContent[3];
			
			g[0] = new GUIContent("Football");
			g[1] = new GUIContent("Plane");
			g[2] = new GUIContent("Train");
			
			float centre = (Screen.width / 4);
			
			selectedCreateGame = GUI.SelectionGrid (new Rect(centre, 100, Screen.width / 2 - 10, 60), selectedCreateGame, g, 3);
			
			if (GUI.Button (new Rect(Screen.width / 2 - 75, 170, 150, 40), "Add Background"))
			{
				//Add Background Code
			}
			
			//Add Question System
			int position = 0;
			
			for (int i = 0; i < questions.Count; i++, position++) //(you know, you could just replace i with position here. :D)
			{
				// Open existing question.
				if (GUI.Button (new Rect(10 * (i + 1) + i * 100, 250, 100, 100), questions[i].text))
				{
					questionPosition = i;
					
					multipleChoiceQuestion = questions[i].text;
					answers.AddRange (questions[i].answers);
					
					addQuestion = true;
				}
			}
			
			// Create new question.
			if (GUI.Button (new Rect(10 * (position + 1) + position * 100, 250, 100, 100), "Add Question"))
			{
				questionPosition = questions.Count + 1;
				addQuestion = true;
				ResetQuestionCreation ();
			}
			
			if (GUI.Button (new Rect(Screen.width / 2 - 25, 390, 50, 25), "Save"))
			{
				//Save Code
				Debug.Log (selectedCreateGame.ToString ());
				Game game = new Game(gameName, authorName, selectedCreateGame.ToString (), questions.ToArray ());
				game.GameToXML ();
				
				master.gameState = cscript_master.GameState.MainMenu;
			}
		}
		else
		{
			if (buttons[11].Clicked)
			{
				addQuestion = false;
			}
			
			multipleChoiceQuestion = GUI.TextArea (new Rect((Screen.width / 4 - 10) * 1.5f, 60, Screen.width / 4, 20), multipleChoiceQuestion);

			//Add Answer System
			int position = 0;
			
			for (int i = 0; i < answers.Count; i++, position++)
			{
				GUISkin temp;
				
				if (answers[i].correct == true)
					temp = correctAnswer;
				else
					temp = incorrectAnswer;
				
				if (GUI.Button (new Rect(10 * (i + 1) + i * 100,180, 100, 100), answers[i].text, temp.button))
				{
					if (answers[i].correct == true)
						answers[i].correct = false;
					else
						answers[i].correct = true;
				}
			}
			
			if (GUI.Button (new Rect(25 + 10 * (position + 1) + position * 100, 225, 100, 50), "Add Answer"))
				answers.Add (new Answer(true, answer));
			
			//answer = GUI.TextArea (new Rect((Screen.width / 4 - 10) * 1.5f, 90, Screen.width / 4, 20), answer);
			answer = GUI.TextArea (new Rect(10 * (position + 1) + position * 100, 200, 145, 20), answer);
			
			GUI.Label (new Rect(Screen.width / 4 - 42, 330, Screen.width - 40, 20), "Tap on object once for incorrect answer, double tap for correct, flick away to delete");
			
			if (GUI.Button (new Rect(Screen.width / 2 - 25, 390, 50, 25), "Save"))
			{
				if (questionPosition > questions.Count)
					questions.Add(new Question(multipleChoiceQuestion, answers.ToArray ()));
				else
					questions[questionPosition] = new Question(multipleChoiceQuestion, answers.ToArray ());
				
				addQuestion = false;
			}
		}
	}
	
	private void ResetGameCreation()
	{		
		gameName = "Add Game Name Here";
		authorName = "Add Author Here";
		questions = new List<Question>();
	}
	
	private void ResetQuestionCreation()
	{
		multipleChoiceQuestion = "Add Multiple Choice Question Here";
		answer = "Add Answer Text Here";
		answers = new List<Answer>();	
	}
}
