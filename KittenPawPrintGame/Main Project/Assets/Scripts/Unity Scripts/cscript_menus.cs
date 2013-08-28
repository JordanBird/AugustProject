﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class cscript_navigation : MonoBehaviour 
{
	cscript_master master;
	cscript_GUI_master GUIMaster;
	
	Texture2D blankWhiteTexture;
	Texture2D blankBlackTexture;
	
	//Main Menu Variables
	public GUISkin game;
	
	Game[] games;
	
	int position = 0;
	
	Texture2D[] gameBackgrounds = new Texture2D[3];
	
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
	
	List<FancyButton> questionButtons = new List<FancyButton>();
	List<FancyButton> answerButtons = new List<FancyButton>();
	
	bool addQuestion = false;
	
	int questionPosition = 0;
	
	FancyButton[] buttons = new FancyButton[14];
	
	public void Init(cscript_master m)
	{
		master = m;
	}
	
	// Use this for initialization
	void Start ()
	{
		blankWhiteTexture = new Texture2D(1, 1);
		blankBlackTexture = new Texture2D(1, 1);
		
		GUIMaster = GameObject.FindGameObjectWithTag ("GUI Master").GetComponent<cscript_GUI_master>();
		
		blankWhiteTexture.SetPixel (0, 0, Color.white);
		blankWhiteTexture.Apply();
			
		blankBlackTexture.SetPixel (0, 0, Color.black);
		blankBlackTexture.Apply();
		
		LoadGames();
		
		//Initialise Backgrounds
		float inc = (Screen.width - 40) / 3;
		
		gameBackgrounds[0] = new Texture2D(Convert.ToInt32 (inc) - 20, Screen.height - 140);
		gameBackgrounds[1] = new Texture2D(Convert.ToInt32 (inc) - 20, Screen.height - 140);
		gameBackgrounds[2] = new Texture2D(Convert.ToInt32 (inc) - 20, Screen.height - 140);
		
		LoadGameBackgrounds ();
		
		buttons[0] = new FancyButton("About", 10, 10, 100, 30, 0.8f, 0);
		buttons[1] = new FancyButton("Help", Screen.width - 110, 10, 100, 30, 0.8f, 0);
		buttons[2] = new FancyButton("Create New Game", Screen.width / 2 - 350, Screen.height - 55, 700, 40, 0.8f, 2);
		
		buttons[3] = new FancyButton("Play", 10 + inc / 4, Screen.height - 110, (int)inc / 2, 40, 0.5f, 2);
		buttons[4] = new FancyButton("Play", inc + 10 * 2 + inc / 4, Screen.height - 110, (int)inc / 2, 40, 0.5f, 2);
		buttons[5] = new FancyButton("Play", 2 * inc + 10 * 3 + inc / 4, Screen.height - 110, (int)inc / 2, 40, 0.5f, 2);
		buttons[6] = new FancyButton("Edit", 10, Screen.height - 100, (int)inc / 6, 20, 0.3f, 2);
		buttons[7] = new FancyButton("Edit", inc + 10 * 2, Screen.height - 100, (int)inc / 6, 20, 0.3f, 2);
		buttons[8] = new FancyButton("Edit", 2 * inc + 10 * 3, Screen.height - 100, (int)inc / 6, 20, 0.3f, 2);
		
		buttons[9] = new FancyButton("<", 10, Screen.height / 2 - 15, 50, 30, 0.8f, 3, GUIMaster.leftArrow);
		buttons[10] = new FancyButton(">", Screen.width - 60, Screen.height / 2 - 15, 50, 30, 0.8f, 1, GUIMaster.rightArrow);
		
		buttons[11] = new FancyButton("< Back", 10, 10, 100, 30, 0.8f, 0);
		
		buttons[12] = new FancyButton("Add Background", Screen.width / 2 - 75, 170, 150, 40, 1f, 1);
		buttons[13] = new FancyButton("Save", Screen.width / 2 - 25, 390, 50, 25, 1f, 3);
		
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
		
		for (int i = 0; i < questionButtons.Count; i++)
		{
			questionButtons[i].Update ();
		}
		
		for (int i = 0; i < answerButtons.Count; i++)
		{
			answerButtons[i].Update ();	
		}
	}
	
	void OnGUI()
	{		
		for (int i = 0; i < questionButtons.Count; i++)
		{
			if (questionButtons[i].Skin == null)
			{
				questionButtons[i].Clicked = GUI.Button (questionButtons[i].GetRectangle(), questionButtons[i].Text);
			}
			else
			{
				questionButtons[i].Clicked = GUI.Button (questionButtons[i].GetRectangle(), questionButtons[i].Text, questionButtons[i].Skin.button);
			}
		}
		
		for (int i = 0; i < answerButtons.Count; i++)
		{
			if (answerButtons[i].Skin == null)
			{
				answerButtons[i].Clicked = GUI.Button (answerButtons[i].GetRectangle(), answerButtons[i].Text);
			}
			else
			{
				answerButtons[i].Clicked = GUI.Button (answerButtons[i].GetRectangle(), answerButtons[i].Text, answerButtons[i].Skin.button);
			}
		}
		
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
<<<<<<< HEAD
			if (buttons[i].Skin == null)
			{
				buttons[i].Clicked = GUI.Button (buttons[i].GetRectangle(), buttons[i].Text);
			}
			else
			{
				buttons[i].Clicked = GUI.Button (buttons[i].GetRectangle(), buttons[i].Text, buttons[i].Skin.button);
			}
=======
			if (buttons[i].texture != null)
				buttons[i].Clicked = GUI.Button (buttons[i].GetRectangle(), buttons[i].texture);
			else
				buttons[i].Clicked = GUI.Button (buttons[i].GetRectangle(), buttons[i].Text);
		}
		
		for (int i = 0; i < questionButtons.Count; i++)
		{
			questionButtons[i].Clicked = GUI.Button (questionButtons[i].GetRectangle(), questionButtons[i].Text);
>>>>>>> origin/Jordan
		}
	}
	
	private void MainMenuGUI()
	{
		GUI.Label(new Rect(10, 10, Screen.width - 20, 50), "Main Menu", GUIMaster.heading.label);
		
		float inc = (Screen.width - 40) / 3;
		
		//Playable Games
		for (int i = 0; i < 3; i++)
		{
			//Shadow and Box
			GUI.DrawTexture (new Rect(i * inc + 9 + (i * 10), 59, inc + 2, Screen.height - 178), blankBlackTexture);
			GUI.DrawTexture (new Rect(i * inc + 10 * (i + 1), 60, inc, Screen.height - 180), blankWhiteTexture);
			
			//Game Background
			GUI.DrawTexture (new Rect(i * inc + 10 * (i + 1), 90, inc - 20, Screen.height - 250), gameBackgrounds[i]);
			
			//Game Title + Author
			GUI.Label (new Rect(i * inc + 10 * (i + 1), 60, inc, Screen.height - 180), games[i + position].name, game.label);
			GUI.Label (new Rect(i * inc + 10 * (i + 1), Screen.height - 160, inc, 20), "By " + games[i + position].author, game.label);

			//Launches Game
			if (buttons[3 + i].Clicked)
			{
				for (int b = 0; b < 11; b++)
				{
					buttons[b].Leave();
				}
				
				master.StartGame (games[i + position]);
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
					questions.Clear ();
					questions.AddRange (games[i + position].questions);
					
					questionButtons.Clear ();
					for (int j = 0; j < questions.Count; j++)
					{
						questionButtons.Add (new FancyButton(questions[j].text, 10 * (j + 1) + j * 100, 250, 100, 100, 0.2f + j / 10f, 2));
						
						questionButtons[j].Enter();
					}
					
					master.gameState = cscript_master.GameState.CreateGame;
					
					for (int j = 0; j < 11; j++)
					{
						buttons[j].Leave();
					}
					
					buttons[1].Enter ();
					buttons[11].Enter ();
					buttons[12].Enter ();
					buttons[13].Enter ();
				}
				
				//Allows Deletion
				if (GUI.Button (new Rect(i * inc + 9 + (i * 10) + inc - 20, 50, 25, 25), "X"))
				{
					File.Delete (games[i + position].location);
					
					if (position >= games.Length - 3)
						position = games.Length - 4;
					
					LoadGames();
				}
			}
		}
		
		if (buttons[9].Clicked)
		{			
			if (position > 0)
			{
				position--;
				LoadGameBackgrounds ();
			}
			
		}
		
		if (buttons[10].Clicked)
		{
			if (position < games.Length - 3)
			{
				position++;
				LoadGameBackgrounds ();
			}
		}
		
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
			buttons[12].Enter ();
			buttons[13].Enter ();
			
			ResetGameCreation ();
		}
	}
	
	private void HelpGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "Help");
		
		if (buttons[11].Clicked)
		{
			master.gameState = cscript_master.GameState.MainMenu;	// NO! This should go back to the gamestate that Help was clicked from, otherwise you could click Help when creating a game and then go back to the main menu!!!!
			
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
			// Having this constantly running doesnt affect performance much, and just reduces the amount of times i need to enter() and leave() the buttons. Consider doing this for all other buttons.
			for (int i = 0; i < questions.Count; i++)
			{
				questionButtons[i].Enter();
			}
			
			GUI.Label(new Rect(Screen.width / 2 - 60, 10, 120, 50), "Create your Game");
		
			if (buttons[11].Clicked)
			{
				master.gameState = cscript_master.GameState.MainMenu;
					
				for (int i = 0; i < 11; i++)
				{
					buttons[i].Enter();
				}
				
				buttons[11].Leave ();
				buttons[12].Leave ();
				buttons[13].Leave ();
			}
			
			if (buttons[1].Clicked)
			{
				master.gameState = cscript_master.GameState.Help;
				
				buttons[12].Leave ();
				buttons[13].Leave ();
			}
			
			gameName = GUI.TextField (new Rect((Screen.width / 4 - 10) / 2, 60, Screen.width / 4, 20), gameName);
			authorName = GUI.TextField (new Rect((Screen.width / 4 - 10) * 2.5f, 60, Screen.width / 4, 20), authorName);
			
			if (buttons[12].Clicked)
			{
				//Add Background Code
			}
			
			//// CAN PARENT THIS TO OTHER BUTTONS IF ANIMATION NEEDED
			GUIContent[] g = new GUIContent[3];
			g[0] = new GUIContent(GUIMaster.footballIcon);
			g[1] = new GUIContent(GUIMaster.planeIcon);
			g[2] = new GUIContent(GUIMaster.trainIcon);
			selectedCreateGame = GUI.SelectionGrid (new Rect(Screen.width / 4, 100, Screen.width / 2 - 10, 60), selectedCreateGame, g, 3);
			
			//Add Question System
			int position = 0;
			
			for (int i = 0; i < questions.Count; i++, position++)
			{
				// Open existing question.
				if (questionButtons[i].Clicked)
				{
					questionPosition = i;
					
					multipleChoiceQuestion = questions[i].text;
					answers.AddRange (questions[i].answers);
					
					answerButtons.Clear ();
					for (int j = 0; j < answers.Count; j++)
					{
						answerButtons.Add (new FancyButton(answers[j].text, 10 * (j + 1) + j * 100, 180, 100, 100, 0.2f + j / 10f, 2));
						answerButtons[j].Enter ();
					}
					
					addQuestion = true;
					buttons[12].Leave ();
					break;
				}
			}
			
			// Create new question.
			if (GUI.Button (new Rect(10 * (position + 1) + position * 100, 250, 100, 100), "Add Question"))
			{
				questionPosition = questions.Count + 1;
				addQuestion = true;
				ResetQuestionCreation ();
				
				buttons[12].Leave ();
				
			}
			
			if (buttons[13].Clicked)
			{
				//Save Code
				Debug.Log (selectedCreateGame.ToString ());
				Game game = new Game(gameName, authorName, selectedCreateGame.ToString (), questions.ToArray (), GameObject.FindGameObjectWithTag ("Master").GetComponent<cscript_master>().dataPath);
				game.GameToXML ();
				
				for (int i = 0; i < 11; i++)
				{
					buttons[i].Enter();
				}
				
				buttons[11].Leave ();
				buttons[12].Leave ();
				buttons[13].Leave ();
				
				master.gameState = cscript_master.GameState.MainMenu;
			}
		}
		else
		{			
			if (buttons[11].Clicked)
			{
				addQuestion = false;
				answers = new List<Answer>();
				buttons[12].Enter ();
				for (int i = 0; i < answerButtons.Count; i++)
				{
					answerButtons[i].Leave ();	
				}
			}
			
			multipleChoiceQuestion = GUI.TextArea (new Rect((Screen.width / 4 - 10) * 1.5f, 60, Screen.width / 4, 20), multipleChoiceQuestion);

			//Add Answer System
			int position = 0;
			
			for (int i = 0; i < answers.Count; i++, position++)
			{					
				if (answerButtons[i].Clicked)
				{
					answers[i].correct = !answers[i].correct;
				}	
				
				if (answers[i].correct == true)
					answerButtons[i].SetSkin (GUIMaster.correctAnswer);
				else
					answerButtons[i].SetSkin (GUIMaster.incorrectAnswer);

				if (GUI.Button (new Rect(answerButtons[i].Position.x + 85, answerButtons[i].Position.y - 10, 25, 25), "X"))
				{
					answers.RemoveAt(i);
					
					answerButtons.Clear ();
					for (int j = 0; j < answers.Count; j++)
					{
						answerButtons.Add (new FancyButton(answers[j].text, 10 * (j + 1) + j * 100, 180, 100, 100, 0.2f + j / 10f, 2));
						answerButtons[j].Unhide ();
					}
				}
			}
			
			if (GUI.Button (new Rect(25 + 10 * (position + 1) + position * 100, 225, 100, 50), "Add Answer"))
			{
				answers.Add (new Answer(false, answer));
				answerButtons.Add (new FancyButton(answer, 10 * (position + 1) + position * 100, 180, 100, 100, 0.2f, 2));	// No extra delay on creation.
				answerButtons[position].Enter ();
			}
			
			//answer = GUI.TextArea (new Rect((Screen.width / 4 - 10) * 1.5f, 90, Screen.width / 4, 20), answer);
			answer = GUI.TextArea (new Rect(10 * (position + 1) + position * 100, 200, 145, 20), answer);
			
			GUI.Label (new Rect(Screen.width / 4 - 42, 330, Screen.width - 40, 20), "Tap on object once for incorrect answer, double tap for correct, flick away to delete");
			
			if (buttons[13].Clicked)
			{
				if (questionPosition > questions.Count)
				{
					questions.Add(new Question(multipleChoiceQuestion, answers.ToArray ()));
					questionButtons.Add (new FancyButton(multipleChoiceQuestion, 10 * questionPosition + (questionPosition - 1) * 100, 250, 100, 100, 0.2f + questionPosition / 10f, 2));
					questionButtons[questionButtons.Count-1].Enter ();
				}
				else
				{
					//questions[questionPosition] = new Question(multipleChoiceQuestion, answers.ToArray ());
					//questionButtons[questionPosition] = new FancyButton(multipleChoiceQuestion, 10 * questionPosition + (questionPosition - 1) * 100, 250, 100, 100, 0.2f + questionPosition / 10f, 2);
				}
				
				addQuestion = false;
				answers = new List<Answer>();
				buttons[12].Enter ();
				
				for (int i = 0; i < answerButtons.Count; i++)
				{
					answerButtons[i].Leave ();	
				}
			}
		}
		
		// Once again, doesnt affect performance and ensures question buttons leave when this function stops running.
		if (addQuestion || master.gameState != cscript_master.GameState.CreateGame)
		{
			for (int i = 0; i < questions.Count; i++)
			{
				questionButtons[i].Leave();
			}
		}
	}
	
	public void LoadGames()
	{
		correctAnswer = Resources.Load ("GUI Skins/GUISkin_Correct_Answer") as GUISkin;
		incorrectAnswer = Resources.Load ("GUI Skins/GUISkin_Incorrect_Answer") as GUISkin;
		game = Resources.Load ("GUI Skins/GUISkin_Main_Menu_Games") as GUISkin;
		
		List<Game> tempGames = new List<Game>();
		
		tempGames.Add (new Game((Resources.Load ("Game Files/Multiplication Football") as TextAsset).ToString (), true));
		tempGames.Add (new Game((Resources.Load ("Game Files/Fly to the Noun") as TextAsset).ToString (), true));
		tempGames.Add (new Game((Resources.Load ("Game Files/Word Train") as TextAsset).ToString (), true));
		
		Directory.CreateDirectory (GameObject.FindGameObjectWithTag ("Master").GetComponent<cscript_master>().dataPath + @"/Game Files");
		
		foreach (string s in Directory.GetFiles (GameObject.FindGameObjectWithTag ("Master").GetComponent<cscript_master>().dataPath + @"/Game Files"))
		{
			try
			{
				tempGames.Add (new Game(s, false));
			}
			catch {}
		}
		
		games = tempGames.ToArray ();
	}
	
	private void ResetGameCreation()
	{		
		gameName = "Add Game Name Here";
		authorName = "Add Author Here";
		questionButtons.Clear ();
		questions.Clear();
	}
	
	private void ResetQuestionCreation()
	{
		multipleChoiceQuestion = "Add Multiple Choice Question Here";
		answer = "Add Answer Text Here";
		answers.Clear ();	
	}
	
	public void MainMenuLoad()
	{
		for (int b = 0; b < 11; b++)
		{
			buttons[b].Enter();
		}
	}
	
	public float GetAspectWidth(float newHeight, float currentHeight, float currentWidth)
    {
        //Resized Width = { (Original Width / Origial Height) * New Height = New Width.
        return (currentWidth / currentHeight) * newHeight;
    }

    public float GetAspectHeight(float newWidth, float currentHeight, float currentWidth)
    {
        //Resized Height = { (Original Height / Origial Width) * New Width = New Height.
        return (currentHeight / currentWidth) * newWidth;
    }
	
	public void LoadGameBackgrounds()
	{
		gameBackgrounds[0].LoadImage (System.Text.Encoding.Default.GetBytes (games[position].background));
		gameBackgrounds[1].LoadImage (System.Text.Encoding.Default.GetBytes (games[position + 1].background));
		gameBackgrounds[2].LoadImage (System.Text.Encoding.Default.GetBytes (games[position + 2].background));
	}
}
