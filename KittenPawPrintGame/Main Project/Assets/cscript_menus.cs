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
	string mulitpleChoiceQuestion = "Add Multiple Choice Question Here";
	string answer = "Add Answer Test Here";
	
	int selectedCreateGame = 0;
	
	List<Answer> answers = new List<Answer>();
	List<Question> questions = new List<Question>();
	
	bool addQuestion = false;
	
	int questionPosition = 0;
	
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		
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
	}
	
	private void MainMenuGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "Main Menu");
		
		if (GUI.Button (new Rect(10, 10, 100, 30), "About"))
			master.gameState = cscript_master.GameState.About;
		
		if (GUI.Button (new Rect(Screen.width - 110, 10, 100, 30), "Help"))
			master.gameState = cscript_master.GameState.Help;
		
		if (GUI.Button (new Rect(Screen.width / 2 - 50, Screen.height - 40, 100, 30), "Create Game"))
			master.gameState = cscript_master.GameState.CreateGame;
		
		float inc = (Screen.width - 40) / 3;
		
		//Playable Games
		
		for (int i = 0; i < 3; i++)
		{
			GUI.DrawTexture (new Rect(i * inc + 9 + (i * 10), 59, inc + 2, Screen.height - 178), blankBlackTexture);
			GUI.DrawTexture (new Rect(i * inc + 10 * (i + 1), 60, inc, Screen.height - 180), blankWhiteTexture);
			
			GUI.Label (new Rect(i * inc + 10 * (i + 1), 60, inc, Screen.height - 180), games[i + position].name, game.label);
			GUI.Label (new Rect(i * inc + 10 * (i + 1), Screen.height - 160, inc, 20), "By " + games[i + position].author, game.label);
			
			if (GUI.Button (new Rect(i * inc + 10 * (i + 1) + inc / 4, (Screen.height - 110), inc / 2, 40), "Play"))
			{
				//Launch games[i + position]
			}
			
			//Edit Game
			if (GUI.Button (new Rect(i * inc + 10 * (i + 1), (Screen.height - 100), inc / 6, 20), "Edit"))
			{
				gameName = games[i + position].name;
				authorName = games[i + position].author;
				selectedCreateGame = Convert.ToInt32(games[i + position].type);
				questions.AddRange (games[i + position].questions);
				
				master.gameState = cscript_master.GameState.CreateGame;
			}
		}
		
		if (GUI.Button (new Rect(10, Screen.height / 2 - 15, 50, 30), "<"))
		{
			position--;
			
			if (position < 0)
				position = 0;
		}
		
		if (GUI.Button (new Rect(Screen.width - 60, Screen.height / 2 - 15, 50, 30), ">"))
		{
			position++;
			
			if (position > games.Length - 3)
				position = games.Length - 3;
		}
	}
	
	private void HelpGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "Help");
		
		if (GUI.Button (new Rect(10, 10, 100, 30), "< Back"))
			master.gameState = cscript_master.GameState.MainMenu;
	}
	
	private void AboutGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "About");
		
		if (GUI.Button (new Rect(10, 10, 100, 30), "< Back"))
			master.gameState = cscript_master.GameState.MainMenu;
	}
	
	private void CreateGameGUI()
	{
		if (addQuestion == false)
		{
			GUI.Label(new Rect(Screen.width / 2 - 60, 10, 120, 50), "Create your Game");
		
			if (GUI.Button (new Rect(10, 10, 100, 30), "< Back"))
				master.gameState = cscript_master.GameState.MainMenu;
			
			if (GUI.Button (new Rect(Screen.width - 110, 10, 100, 30), "Help"))
				master.gameState = cscript_master.GameState.Help;
			
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
			
			for (int i = 0; i < questions.Count; i++)
			{
				if (GUI.Button (new Rect(10 * (i + 1) + i * 100, 250, 100, 100), questions[i].text))
				{
					questionPosition = i;
					
					mulitpleChoiceQuestion = questions[i].text;
					answers.AddRange (questions[i].answers);
					
					addQuestion = true;
				}
				
				position++;
			}
			
			if (GUI.Button (new Rect(10 * (position + 1) + position * 100, 250, 100, 100), "Add Question"))
			{
				questionPosition = questions.Count + 1;
				addQuestion = true;
				//Open to be new.
			}
			
			if (GUI.Button (new Rect(Screen.width / 2 - 25, 390, 50, 25), "Save"))
			{
				//Save Code
				Debug.Log (selectedCreateGame.ToString ());
				Game game = new Game(gameName, authorName, selectedCreateGame.ToString (), questions.ToArray ());
				game.GameToXML ();
			}
		}
		else
		{
			if (GUI.Button (new Rect(10, 10, 100, 30), "< Back"))
			{
				mulitpleChoiceQuestion = "Add Multiple Choice Question Here";
				answers = new List<Answer>();
				
				addQuestion = false;
			}
			
			mulitpleChoiceQuestion = GUI.TextArea (new Rect((Screen.width / 4 - 10) * 1.5f, Screen.height / 2 - 50, Screen.width / 4, 20), mulitpleChoiceQuestion);
			
			//Add Answer System
			int position = 0;
			
			for (int i = 0; i < answers.Count; i++)
			{
				GUISkin temp;
				
				if (answers[i].correct == true)
					temp = correctAnswer;
				else
					temp = incorrectAnswer;
				
				if (GUI.Button (new Rect(10 * (i + 1) + i * 100, Screen.height / 2 + 20, 100, 100), answers[i].text, temp.button))
				{
					if (answers[i].correct == true)
						answers[i].correct = false;
					else
						answers[i].correct = true;
				}
				
				position++;
			}
			
			answer = GUI.TextArea (new Rect((Screen.width / 4 - 10) * 1.5f, Screen.height / 2 - 20, Screen.width / 4, 20), answer);
			
			if (GUI.Button (new Rect(10 * (position + 1) + position * 100, 250, 100, 100), "Add Answer"))
				answers.Add (new Answer(true, answer));
			
			GUI.Label (new Rect(20, 370, Screen.width - 40, 20), "Tap on object once for incorrect answer, double tap for correct, flick away to delete");
			
			if (GUI.Button (new Rect(Screen.width / 2 - 25, 390, 50, 25), "Save"))
			{
				if (questionPosition > questions.Count)
					questions.Add(new Question(mulitpleChoiceQuestion, answers.ToArray ()));
				else
					questions[questionPosition] = new Question(mulitpleChoiceQuestion, answers.ToArray ());
					
				mulitpleChoiceQuestion = "Add Multiple Choice Question Here";
				answers = new List<Answer>();
				
				addQuestion = false;
			}
		}
	}
}
