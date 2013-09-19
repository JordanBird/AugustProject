using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class cscript_navigation : MonoBehaviour 
{
	//-----------------------\\
	//Default Variables
	//-----------------------\\
	cscript_master master;
	cscript_GUI_master GUIMaster;
	cscript_sound_master soundMaster;
	
	string dataPath = "";
	
	bool loadGames = false;
	
	//-----------------------\\
	//Main Menu Variables
	//-----------------------\\
	public GUISkin game;
	
	Game[] games;
	
	int position = 0;
	
	Texture2D[] gameBackgrounds = new Texture2D[3];
	
	//-----------------------\\
	//Create Game Variables
	//-----------------------\\
	string gameName = "Add Game Name Here";
	string authorName = "Add Author Here";
	string multipleChoiceQuestion = "Add Multiple Choice Question Here";
	string answer = "Add Answer Text Here";
	Texture2D answerImage;
	Texture2D background;
	
	int selectedCreateGame = 0;
	
	List<Answer> answers = new List<Answer>();
	List<Question> questions = new List<Question>();
	
	List<FancyButton> questionButtons = new List<FancyButton>();
	List<FancyButton> answerButtons = new List<FancyButton>();
	
	bool addQuestion = false;
	
	int questionPosition = 0;
	
	FancyButton[] buttons = new FancyButton[14];
	
	//-----------------------\\
	//iOS Variables
	//-----------------------\\
	public Material targetMaterial = null;
	public bool useOriginalImageSize = false;
	public bool iPadPopover_CloseWhenSelectImage = true;
	private int textureWidth;
	private int textureHeight;
	private bool saveAsPng = false;
	private bool imageInputChoiceMenu = false;
	private bool imageAnswerChoiceMenu = false;
	
	
	JGUI jG = new JGUI();

	public void Init(cscript_master m)
	{
		master = m;
	}
	
	// Use this for initialization
	void Start ()
	{
		dataPath = GameObject.FindGameObjectWithTag ("Master").GetComponent<cscript_master>().dataPath;
		GUIMaster = GameObject.FindGameObjectWithTag ("GUI Master").GetComponent<cscript_GUI_master>();
		soundMaster = GameObject.FindGameObjectWithTag ("Sound Master").GetComponent<cscript_sound_master>();
		LoadGames();

		//Initialise Backgrounds
		float inc = (Screen.width - 40) / 3;
		
		gameBackgrounds[0] = new Texture2D(Convert.ToInt32 (inc) - 20, Screen.height - 140);
		gameBackgrounds[1] = new Texture2D(Convert.ToInt32 (inc) - 20, Screen.height - 140);
		gameBackgrounds[2] = new Texture2D(Convert.ToInt32 (inc) - 20, Screen.height - 140);
		
		LoadGameBackgrounds ();
		
		#region Fancy Button Initialisation
		buttons[0] = new FancyButton("About", 10, 10, 100, 30, 0.8f, 0, GUIMaster.buttons);
		buttons[1] = new FancyButton("Help", Screen.width - 110, 10, 100, 30, 0.8f, 0, GUIMaster.buttons);
		buttons[2] = new FancyButton("Create New Game", Screen.width / 2 - 350, Screen.height - 55, 700, 40, 0.8f, 2, GUIMaster.buttons);

		buttons[3] = new FancyButton("Play", 10 + inc / 4, Screen.height - 150, (int)inc / 2, 40, 0.5f, 2, GUIMaster.buttons);
		buttons[4] = new FancyButton("Play", inc + 10 * 2 + inc / 4, Screen.height - 150, (int)inc / 2, 40, 0.5f, 2, GUIMaster.buttons);
		buttons[5] = new FancyButton("Play", 2 * inc + 10 * 3 + inc / 4, Screen.height - 150, (int)inc / 2, 40, 0.5f, 2, GUIMaster.buttons);
		
		buttons[6] = new FancyButton("Edit", 10, Screen.height - 145, (int)inc / 6, 30, 0.3f, 2, GUIMaster.saveButton);
		buttons[7] = new FancyButton("Edit", inc + 10 * 2, Screen.height - 145, (int)inc / 6, 30, 0.3f, 2, GUIMaster.saveButton);
		buttons[8] = new FancyButton("Edit", 2 * inc + 10 * 3, Screen.height - 145, (int)inc / 6, 30, 0.3f, 2, GUIMaster.saveButton);
		
		buttons[9] = new FancyButton("", 10, Screen.height / 2 - 15, 50, 50, 0.8f, 3, GUIMaster.leftArrow);
		buttons[10] = new FancyButton("", Screen.width - 60, Screen.height / 2 - 15, 50, 50, 0.8f, 1, GUIMaster.rightArrow);
		
		buttons[11] = new FancyButton("< Back", 10, 10, 100, 30, 0.8f, 0, GUIMaster.buttons);
		
		buttons[12] = new FancyButton("<color=#c8e9ac>Add Background</color>", Screen.width / 2 - 100, 180, 200, 40, 1f, 1, GUIMaster.buttons);
		buttons[13] = new FancyButton("Save", Screen.width / 2 - 50, Screen.height - 35, 100, 25, 1f, 3, GUIMaster.saveButton);
		
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
		#endregion
	}
	
	// Update is called once per frame
	void Update () 
	{
		//In order for the loading screen to work, new games must be loaded on the next update.
		if (loadGames == true)
		{
			LoadGames();
		}
		
		#region Fancy Button Updates
		//Updates all buttons in game.
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].Update();
		}

		for (int i = 0; i < answerButtons.Count; i++)
		{
			answerButtons[i].Update ();	
		}
		#endregion
	}
	
	void OnGUI()
	{		
		GUI.depth = 1;
		
		//Makes buttons fill text.
		GUI.skin.button.wordWrap = true;
		
		//iPadPopover_CloseWhenSelectImage = GUI.Toggle(new Rect(0, Screen.height*0.5f+60, 400, 30), iPadPopover_CloseWhenSelectImage, "iPadPopover_CloseWhenSelectImage");
			
		for (int i = 0; i < questionButtons.Count; i++)
		{
			if (questionButtons[i].Skin == null)
			{
				questionButtons[i].Clicked = GUI.Button (questionButtons[i].GetRectangle(), questionButtons[i].Content);
			}
			else
			{
				questionButtons[i].Clicked = GUI.Button (questionButtons[i].GetRectangle(), questionButtons[i].Content, questionButtons[i].Skin.button);
			}
		}
		
		for (int i = 0; i < answerButtons.Count; i++)
		{
			if (answerButtons[i].Skin == null)
			{
				answerButtons[i].Clicked = GUI.Button (answerButtons[i].GetRectangle(), answerButtons[i].Content);
			}
			else
			{
				answerButtons[i].Clicked = GUI.Button (answerButtons[i].GetRectangle(), answerButtons[i].Content, answerButtons[i].Skin.button);
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
			if (buttons[i].Skin == null)
			{
				buttons[i].Clicked = GUI.Button (buttons[i].GetRectangle(), buttons[i].Content);
			}
			else
			{
				buttons[i].Clicked = GUI.Button (buttons[i].GetRectangle(), buttons[i].Content, buttons[i].Skin.button);
			}
		}
		
		if (GUIMaster.GetComponent<LoadingScreen>().show == true)
			GUIMaster.GetComponent<LoadingScreen>().Draw ();
	}
	
	private void MainMenuGUI()
	{
		
		//Banners
		GUI.DrawTexture (new Rect(0, 0, Screen.width, 81), GUIMaster.blankBlackTexture);
		GUI.DrawTexture (new Rect(0, Screen.height -101, Screen.width, 100), GUIMaster.blankBlackTexture);
		GUI.DrawTexture (new Rect(0, 0, Screen.width, 80), GUIMaster.bannerTexture);
		GUI.DrawTexture (new Rect(0, Screen.height - 100, Screen.width, 100), GUIMaster.bannerTexture);
		
		//GUI.Label(new Rect(10, 10, Screen.width - 20, 50), "Main Menu", GUIMaster.heading.label);
		GUI.DrawTexture(new Rect(Screen.width / 2 - 19, 10, 38, 60), GUIMaster.iLearn2Logo);
		float inc = (Screen.width - 40) / 3;
		
		//Playable Games
		for (int i = 0; i < 3; i++)
		{
			//Shadow and Box
			GUI.DrawTexture (new Rect(i * inc + 9 + (i * 10), 99, inc + 2, Screen.height - 258), GUIMaster.blankBlackTexture);
			GUI.DrawTexture (new Rect(i * inc + 10 * (i + 1), 100, inc, Screen.height - 260), GUIMaster.blankWhiteTexture);
			
			//Game Background
			GUI.DrawTexture (new Rect(i * inc + 9 + (i * 10) + 10, 140, inc - 20, Screen.height - 340), gameBackgrounds[i]);
			
			//Game Title + Author
			GUI.Label (new Rect(i * inc + 10 * (i + 1), 110, inc, Screen.height - 180), games[i + position].name, game.label);
			GUI.Label (new Rect(i * inc + 10 * (i + 1), Screen.height - 190, inc, 20), "By " + games[i + position].author, game.label);

			//Edit and Delete Game
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
					soundMaster.PlaySound (soundMaster.correctAnswer);
					//Assign working edit variables with game's content. //MUST BE EDITED IF VARIABLES ARE ADDED OR REMOVED!!
					gameName = games[i + position].name;
					authorName = games[i + position].author;
					selectedCreateGame = Convert.ToInt32(games[i + position].type);
					background = games[i + position].background;
					
					questions.Clear ();
					questions.AddRange (games[i + position].questions);
					answers.AddRange (games[i + position].questions[0].answers);
					
					CreateFancyAnswerButtons(true);
					
					questionButtons.Clear ();
					
					master.gameState = cscript_master.GameState.CreateGame;
					
					for (int j = 0; j < 11; j++)
					{
						buttons[j].Leave();
					}
					
					buttons[1].Enter ();
					buttons[11].Enter ();
					buttons[12].Enter ();
					buttons[13].Enter ();
					break;
				}
				
				//Deletion
				if (GUI.Button (new Rect(i * inc + 9 + (i * 10) + inc - 35, Screen.height - 145, 25, 25), "", GUIMaster.deleteButton.button))
				{
					soundMaster.PlaySound (soundMaster.incorrectAnswer);
					File.Delete (games[i + position].location);
					
					if (position >= games.Length - 3)
						position = games.Length - 4;
					
					//Resets the game view.
					GUIMaster.GetComponent<LoadingScreen>().Show ();
					loadGames = true;
				}
				
				//Open In Other Application
				if (GUI.Button (new Rect(i * inc + 9 + (i * 10) + inc - 65, Screen.height - 145, 25, 25), "", GUIMaster.openIn.button))
				{
					string uri = games[i + position].location;
					
					FileOpener.OpenFile((int)(i * inc + 9 + (i * 10) + inc - 65), (int)(Screen.height - 145),uri);
				}
			}
			
			//Launches Game
			if (buttons[3 + i].Clicked)
			{
				for (int b = 0; b < 11; b++)
				{
					buttons[b].Leave();
				}
				soundMaster.PlaySound (soundMaster.correctAnswer);
				master.StartGame (games[i + position]);
				break;
			}
		}
		
		//Previos Game
		if (buttons[9].Clicked)
		{			
			if (position > 0)
			{
				soundMaster.PlaySound (soundMaster.navigation);
				position--;
				LoadGameBackgrounds ();
			}
		}
		
		//Next Game
		if (buttons[10].Clicked)
		{
			if (position < games.Length - 3)
			{
				soundMaster.PlaySound (soundMaster.navigation);
				position++;
				LoadGameBackgrounds ();
			}
		}
		
		//About
		if (buttons[0].Clicked)
			Application.OpenURL("http://www.ilearn2.co.uk");
		
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
			soundMaster.PlaySound (soundMaster.correctAnswer);
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
		//HelpGUI() will become redundant when we implement overlay help.
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "Help");
		
		if (buttons[11].Clicked)
		{
			master.gameState = cscript_master.GameState.MainMenu;
			soundMaster.PlaySound (soundMaster.correctAnswer);
			GUIMaster.GetComponent<LoadingScreen>().Show ();
			loadGames = true;
			
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
			soundMaster.PlaySound (soundMaster.correctAnswer);
			GUIMaster.GetComponent<LoadingScreen>().Show ();
			loadGames = true;
			for (int i = 0; i < 11; i++)
			{
				buttons[i].Enter();
			}
			
			buttons[11].Leave ();
		}
	}
	
	private void CreateGameGUI()
	{		
		//Banners
			GUI.DrawTexture (new Rect(0, 0, Screen.width, 64), GUIMaster.bannerTexture);
			GUI.DrawTexture (new Rect(0, Screen.height - 100, Screen.width, 100), GUIMaster.bannerTexture);
		
		if (true)
		{
			if (imageInputChoiceMenu == true)
				GUI.enabled = false;
			else
				GUI.enabled = true;
			
//			// Having this constantly running doesnt affect performance much, and just reduces the amount of times i need to enter() and leave() the buttons. Consider doing this for all other buttons.
//			for (int i = 0; i < questions.Count; i++)
//			{
//				questionButtons[i].Enter();
//			}
			
			GUI.Label(new Rect(10, 10, Screen.width - 20, 50), "Create your Game", GUIMaster.heading.label);
		
			//Back
			if (buttons[11].Clicked)
			{
				soundMaster.PlaySound (soundMaster.correctAnswer);
				ResetGameCreation();
				ResetQuestionCreation ();
				
				master.gameState = cscript_master.GameState.MainMenu;
				
				GUIMaster.GetComponent<LoadingScreen>().Show ();
				loadGames = true;
				for (int i = 0; i < 11; i++)
				{
					buttons[i].Enter();
				}
				
				buttons[11].Leave ();
				buttons[12].Leave ();
				buttons[13].Leave ();
			}
			
			//Help
			if (buttons[1].Clicked)
			{
				soundMaster.PlaySound (soundMaster.correctAnswer);
				master.gameState = cscript_master.GameState.Help;
				
				buttons[12].Leave ();
				buttons[13].Leave ();
			}
			
			gameName = GUI.TextField (new Rect((Screen.width / 4 - 10) / 2, 80, Screen.width / 4, 20), gameName, GUIMaster.createGameTextFields.textField);
			authorName = GUI.TextField (new Rect((Screen.width / 4 - 10) * 2.5f, 80, Screen.width / 4, 20), authorName, GUIMaster.createGameTextFields.textField);
			
			//Get Background Button - Opens the Pop-Up to allow the user to choose an image input method:
			if (buttons[12].Clicked)
			{
				soundMaster.PlaySound (soundMaster.correctAnswer);
				if (Application.platform == RuntimePlatform.IPhonePlayer) 
				{
					imageInputChoiceMenu = true;
				}
				else
				{
					//Mostly for debug purposes.
					background = new Texture2D(1, 1);
					background.SetPixel (0, 0, Color.red);
					background.Apply ();
				}
			}

			//Change the default skin so we can effect the buttons of the GUIContent.
			GUISkin tempSkin = GUI.skin;
			GUI.skin = GUIMaster.buttons;
			
			//// CAN PARENT THIS TO OTHER BUTTONS IF ANIMATION NEEDED
			GUIContent[] g = new GUIContent[3];
			g[0] = new GUIContent(GUIMaster.footballIcon);
			g[1] = new GUIContent(GUIMaster.planeIcon);
			g[2] = new GUIContent(GUIMaster.photoIcon);
			selectedCreateGame = GUI.SelectionGrid (new Rect(Screen.width / 2 - 100, 110, 200, 60), selectedCreateGame, g, 3);
			
			GUI.skin = tempSkin;

			//Save
			if (buttons[13].Clicked)
			{
				soundMaster.PlaySound (soundMaster.correctAnswer);
				//Saves the currently open game.
				Debug.Log ("Saving Game");
				
				Game game = new Game(gameName, authorName, selectedCreateGame.ToString (), new Question[] { new Question(multipleChoiceQuestion, answers.ToArray ()) }, background);
				game.GameToXML ();
				
				ResetGameCreation();
				ResetQuestionCreation ();
				
				//Return to Main Menu
				for (int i = 0; i < 11; i++)
				{
					buttons[i].Enter();
				}
				
				buttons[11].Leave ();
				buttons[12].Leave ();
				buttons[13].Leave ();
				
				master.gameState = cscript_master.GameState.MainMenu;
				
				GUIMaster.GetComponent<LoadingScreen>().Show ();
				loadGames = true;
				
				Debug.Log ("Saved Game");
			}
			
			//Gets the background from camera roll or camera.
			if (Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				if(imageInputChoiceMenu)
				{
					Debug.Log(jG.MessageBox(0, "Add Background", new string[] {"Use Camera", "Use Camera Roll"}));
					
					switch (jG.MessageBox(0, "Add Background", new string[] {"Use Camera", "Use Camera Roll"}))
					{
						
						case 0:
						Debug.Log ("Case0");
							imageInputChoiceMenu = false;
							//Directly From Camera:
							LoadTextureFromImagePicker.SetPopoverAutoClose(iPadPopover_CloseWhenSelectImage);
							LoadTextureFromImagePicker.SetPopoverToCenter();
							LoadTextureFromImagePicker.ShowCamera(gameObject.name, "OnFinishedImagePicker");
						
							imageInputChoiceMenu = false;
							break;
						case 1:
						Debug.Log ("Case1");
							imageInputChoiceMenu = false;
							//From Camera Roll:
							LoadTextureFromImagePicker.SetPopoverAutoClose(iPadPopover_CloseWhenSelectImage);
							LoadTextureFromImagePicker.SetPopoverTargetRect((Screen.width / 4 - 10) / 2, 100, Screen.width / 4,20);
							LoadTextureFromImagePicker.ShowPhotoLibrary(gameObject.name, "OnFinishedImagePicker");
	
						break;
					}
					
//					if(GUI.Button (new Rect((Screen.width / 4 - 10) / 2, 150, Screen.width / 4, 20), "USE CAMERA"))
//					{
//						imageInputChoiceMenu = false;
//						//Directly From Camera:
//						LoadTextureFromImagePicker.SetPopoverToCenter();
//						LoadTextureFromImagePicker.ShowCamera(gameObject.name, "OnFinishedImagePicker");
//					}
//					if(GUI.Button (new Rect((Screen.width / 4 - 10) * 2.5f, 150, Screen.width / 4, 20), "USE CAMERA ROLL"))
//					{
//						imageInputChoiceMenu = false;
//						//From Camera Roll:
//						LoadTextureFromImagePicker.SetPopoverAutoClose(iPadPopover_CloseWhenSelectImage);
//						LoadTextureFromImagePicker.SetPopoverTargetRect((Screen.width / 4 - 10) / 2, 100, Screen.width / 4,20);
//						LoadTextureFromImagePicker.ShowPhotoLibrary(gameObject.name, "OnFinishedImagePicker");
//					}
				}
			}
			
			#region Answers Start
			if (buttons[11].Clicked)
			{
				addQuestion = false;
				answers = new List<Answer>();
				
				for (int i = 0; i < answerButtons.Count; i++)
				{
					answerButtons[i].Leave ();	
				}
			}
			
			multipleChoiceQuestion = GUI.TextArea (new Rect((Screen.width / 4) * 1.5f, 220, Screen.width / 4, 20), multipleChoiceQuestion, GUIMaster.createGameTextFields.textField);

			//Add Answer System
			int position = 0;
			
			for (int i = 0; i < answers.Count; i++, position++)
			{					
				if (answerButtons[i].Clicked)
				{
					soundMaster.PlaySound(soundMaster.correctAnswer);
					answers[i].correct = !answers[i].correct;
				}	
				
				if (answers[i].correct == true)
					answerButtons[i].SetSkin (GUIMaster.correctAnswer);
				else
					answerButtons[i].SetSkin (GUIMaster.incorrectAnswer);

				if (GUI.Button (new Rect(answerButtons[i].Position.x + 140, answerButtons[i].Position.y - 10, 25, 25), "", GUIMaster.deleteButton.button))
				{
					soundMaster.PlaySound (soundMaster.incorrectAnswer);
					answers.RemoveAt(i);
					CreateFancyAnswerButtons(false);
				}
			}
			
			DrawAddAnswerStuff();
			
			//Open Answer Image Popup:
			//Gets the background from camera roll or camera.
			if (Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				if(imageAnswerChoiceMenu)
				{
					Debug.Log(jG.MessageBox(0, "Add Background", new string[] {"Use Camera", "Use Camera Roll"}));
					switch (jG.MessageBox(0, "Add Background", new string[] {"Use Camera", "Use Camera Roll"}))
					{
						case 0:
						Debug.Log ("Case0");
							imageAnswerChoiceMenu = false;
							//Directly From Camera:
							LoadTextureFromImagePicker.SetPopoverAutoClose(iPadPopover_CloseWhenSelectImage);
							LoadTextureFromImagePicker.SetPopoverToCenter();
							LoadTextureFromImagePicker.ShowCamera(gameObject.name, "OnFinishedAnswerPicker");
						
							imageInputChoiceMenu = false;
							break;
						case 1:
						Debug.Log ("Case1");
							imageAnswerChoiceMenu = false;
							//From Camera Roll:
							LoadTextureFromImagePicker.SetPopoverAutoClose(iPadPopover_CloseWhenSelectImage);
							LoadTextureFromImagePicker.SetPopoverTargetRect((Screen.width / 4 - 10) / 2, 100, Screen.width / 4,20);
							LoadTextureFromImagePicker.ShowPhotoLibrary(gameObject.name, "OnFinishedAnswerPicker");
	
						break;
					}
				}
			}
			
			//Instruction Text
			GUI.skin.label.alignment = TextAnchor.UpperCenter;
			GUI.skin.label.fontSize = 20;
			GUI.Label (new Rect(10, Screen.height - 80, Screen.width - 20, 40), "<color=#df4c4c>Tap on object once for incorrect answer,</color> <color=#93d758>double tap for correct,</color> <color=#438ee4>click the cross to delete</color>");
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			
			if (buttons[13].Clicked)
			{
				
				if (true)
				{
					questions.Add(new Question(multipleChoiceQuestion, answers.ToArray ()));
					
					if (questionButtons.Count > 0)
					{
						if (questionButtons[questionButtons.Count-1].targetPosition.x + 220 > Screen.width)
						{
							soundMaster.PlaySound(soundMaster.correctAnswer);
							Debug.Log ("New Row");
							//Adding new row.
							questionButtons.Add (new FancyButton(multipleChoiceQuestion, 10, questionButtons[questionButtons.Count-1].targetPosition.y + 310, 150, 150, 0.2f + questionPosition / 10f, 2));
						}
						else
						{
							soundMaster.PlaySound(soundMaster.correctAnswer);
							Debug.Log ("Add Normal");
							//Add Normal
							questionButtons.Add (new FancyButton(multipleChoiceQuestion, questionButtons[questionButtons.Count-1].targetPosition.x + 310, questionButtons[questionButtons.Count-1].targetPosition.y, 150, 150, 0.2f + questionPosition / 10f, 2));
						}
					}
					else
					{
						questionButtons.Add (new FancyButton(multipleChoiceQuestion, 10 * questionPosition + (questionPosition - 1) * 150, 250, 150, 150, 0.2f + questionPosition / 10f, 2));
					}

					questionButtons[questionButtons.Count-1].Enter ();
				}
				else
				{
					//questions[questionPosition] = new Question(multipleChoiceQuestion, answers.ToArray ());
					//questionButtons[questionPosition] = new FancyButton(multipleChoiceQuestion, 10 * questionPosition + (questionPosition - 1) * 100, 250, 100, 100, 0.2f + questionPosition / 10f, 2);
					questions[0] = new Question(multipleChoiceQuestion, answers.ToArray ());
				}
				
				addQuestion = false;
				answers = new List<Answer>();
				//buttons[12].Enter ();
				
				for (int i = 0; i < answerButtons.Count; i++)
				{
					answerButtons[i].Leave ();	
				}
			}
		}
		else
		{
		}
		#endregion
		
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
		cscript_master.GameState tempState = master.gameState;
		master.gameState = cscript_master.GameState.LoadingScreen;
		GUIMaster.GetComponent<LoadingScreen>().Show ();

		game = Resources.Load ("GUI Skins/GUISkin_Main_Menu_Games") as GUISkin;
		
		List<Game> tempGames = new List<Game>();
		
		//Prepackaged Games, loaded from the app.
		tempGames.Add (new Game((Resources.Load ("Game Files/Multiplication Football") as TextAsset).ToString (), true));
		tempGames.Add (new Game((Resources.Load ("Game Files/Fly to the Noun") as TextAsset).ToString (), true));
		tempGames.Add (new Game((Resources.Load ("Game Files/Word Train") as TextAsset).ToString (), true));
		
		//Games that are user created.
		Directory.CreateDirectory (dataPath + @"/Game Files");
		
		foreach (string s in Directory.GetFiles (dataPath + @"/Game Files"))
		{
			try
			{
				tempGames.Add (new Game(s, false));
			}
			catch {}
		}
		
		games = tempGames.ToArray ();
		
		LoadGameBackgrounds ();
		
		master.gameState = tempState;
		GUIMaster.GetComponent<LoadingScreen>().Hide ();
		loadGames = false;
	}
	
	private void ResetGameCreation()
	{
		gameName = "Add Game Name Here";
		authorName = "Add Author Here";
		
		background = new Texture2D(1, 1);
		background.SetPixel (0, 0, Color.magenta);
		background.Apply ();
		
		questionButtons.Clear ();
		questions.Clear();
		
		ResetQuestionCreation ();
	}
	
	private void ResetQuestionCreation()
	{
		multipleChoiceQuestion = "Add Multiple Choice Question Here";
		answer = "Add Answer Text Here";
		answerImage = null;
		
		answerButtons.Clear ();
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
		gameBackgrounds[0] = games[position].background;
		gameBackgrounds[1] = games[position + 1].background;
		gameBackgrounds[2] = games[position + 2].background;
	}
	
	public void CreateFancyAnswerButtons(bool show)
	{
		answerButtons.Clear ();
		
		for (int j = 0; j < answers.Count; j++)
		{
			GUIContent content;
			
			if (answers[j].type == 1)
				content = new GUIContent(answers[j].texture);
			else
				content = new GUIContent(answers[j].text);
			
			if (answerButtons.Count > 0)
			{
				if (answerButtons[answerButtons.Count-1].targetPosition.x + 220 > Screen.width)
				{
					Debug.Log ("New Row");
					//Adding new row.
					answerButtons.Add (new FancyButton(content, 10, answerButtons[answerButtons.Count-1].targetPosition.y + 160, 150, 150, 0.2f + j / 10f, 2));
				}
				else
				{
					Debug.Log ("Add Normal - Create");
					//Add Normal
					answerButtons.Add (new FancyButton(content, answerButtons[answerButtons.Count-1].targetPosition.x + 160, answerButtons[answerButtons.Count-1].targetPosition.y, 150, 150, 0.2f + j / 10f, 2));
				}
			}
			else
				answerButtons.Add (new FancyButton(content, 10, 250, 150, 150, 0.2f + j / 10f, 2));

			if (show == true)
				answerButtons[j].Enter ();
			else
				answerButtons[j].Unhide ();
		}
	}
	
	public void AddFancyAnswerButton()
	{
		GUIContent content;
		
		if (answerImage != null)
			content = new GUIContent(answerImage);
		else
			content = new GUIContent(answer);
		
		if (answerButtons.Count > 0)
		{
			if (answerButtons[answerButtons.Count-1].targetPosition.x + 500 > Screen.width)
			{
				Debug.Log ("New Row");
				//Adding new row.
				answerButtons.Add (new FancyButton(content, 10, answerButtons[answerButtons.Count-1].targetPosition.y + 160, 150, 150, 0.2f, 2));
			}
			else
			{
				Debug.Log ("Add Normal - Add");
				//Add Normal
				answerButtons.Add (new FancyButton(content, answerButtons[answerButtons.Count-1].targetPosition.x + 160, answerButtons[answerButtons.Count-1].targetPosition.y, 150, 150, 0.2f, 2));
			}
		}
		else
			answerButtons.Add (new FancyButton(answer, 10, 250, 150, 150, 0.2f + position / 10f, 2));
		
		//answerButtons.Add (new FancyButton(answer, 10 * (position + 1) + position * 100, 180, 100, 100, 0.2f, 2));	// No extra delay on creation.
		answerButtons[answerButtons.Count - 1].Enter ();
		
		answer = "Add Answer Text Here";
		answerImage = null;
	}
	
	public void DrawAddAnswerStuff()
	{
		float x = -150;
		float y = 250;
		
		if (answerButtons.Count > 0)
		{
			x = answerButtons[answerButtons.Count - 1].targetPosition.x;
			y = answerButtons[answerButtons.Count - 1].targetPosition.y;
		}
		
		if (GUI.Button (new Rect(x + 180, y + 90, 160, 40), "Add Answer", GUIMaster.saveButton.button))
		{
			soundMaster.PlaySound(soundMaster.correctAnswer);
			if (answer == "Add Answer Text Here")
				answer = "";
			
			if (answerImage != null)
				answers.Add (new Answer(false, answer, answerImage));
			else
				answers.Add (new Answer(false, answer));
			
			AddFancyAnswerButton ();
		}

		answer = GUI.TextArea (new Rect(x + 180, y + 20, 160, 20), answer, GUIMaster.createGameTextFields.textField);
	
		if (GUI.Button(new Rect(x + 180, y + 50, 160, 30), "Click to Add Image", GUIMaster.saveButton.button))
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				imageAnswerChoiceMenu = true;
			}
			else
			{
				//For debug purposes.
				answerImage = new Texture2D(1, 1);
				answerImage.SetPixel (0, 0, Color.red);
				answerImage.Apply ();
			}
		}
	}
	
	//-----------------------\\
	//iOS Specific Functions
	//-----------------------\\
	#region iOS Functions
	private void OnFinishedImagePicker (string message)
	{
	
		if (LoadTextureFromImagePicker.IsLoaded())
		{
			int width, height;
			
			if (useOriginalImageSize || (targetMaterial == null))
			{
				width = LoadTextureFromImagePicker.GetLoadedTextureWidth();
				height = LoadTextureFromImagePicker.GetLoadedTextureHeight();
			}
			else
			{
				width = textureWidth;
				height = textureHeight;
			}
			
			Texture2D texture = LoadTextureFromImagePicker.GetLoadedTexture(message, width, height);
			
			//IT GETS HERE
			if (texture)
			{
				// Loaded
				//if (targetMaterial) {
					//BUT NOT HERE
					background = texture;
					//Texture lastTexture = targetMaterial.mainTexture;
					//targetMaterial.mainTexture = texture;
					//Destroy(lastTexture);
				//}
				jG.ResetMessageBox();
			}
			else
			{
				// Closed
				LoadTextureFromImagePicker.Release();
			}
		}
		else
		{
			// Closed
			LoadTextureFromImagePicker.Release();
		}
	}
	
	private void OnFinishedAnswerPicker (string message)
	{
	
		if (LoadTextureFromImagePicker.IsLoaded())
		{
			int width, height;
			
			if (useOriginalImageSize || (targetMaterial == null))
			{
				width = LoadTextureFromImagePicker.GetLoadedTextureWidth();
				height = LoadTextureFromImagePicker.GetLoadedTextureHeight();
			}
			else
			{
				width = textureWidth;
				height = textureHeight;
			}
			
			Texture2D texture = LoadTextureFromImagePicker.GetLoadedTexture(message, width, height);
			
			//IT GETS HERE
			if (texture)
			{
				TextureScale.Point(texture,texture.width/5,texture.height/5);
				answerImage = texture;
				jG.ResetMessageBox();
			}
			else
			{
				// Closed
				LoadTextureFromImagePicker.Release();
			}
		}
		else
		{
			// Closed
			LoadTextureFromImagePicker.Release();
		}
	}
	


	private IEnumerator CaptureScreen()
	{
		yield return new WaitForEndOfFrame();

		// Save to PhotoLibrary
		Texture screenShot = ScreenCapture.Capture();
		if (saveAsPng)
		{
			bool withTransparency = false;
			if (withTransparency)
			{
				// PNG with transparency
				LoadTextureFromImagePicker.SaveAsPngWithTransparencyToPhotoLibrary(screenShot, gameObject.name, "OnFinishedSaveImage");
			}
			else
			{
				// PNG
				LoadTextureFromImagePicker.SaveAsPngToPhotoLibrary(screenShot, gameObject.name, "OnFinishedSaveImage");
			}
		}
		else
		{
			// JPG
			LoadTextureFromImagePicker.SaveAsJpgToPhotoLibrary(screenShot, gameObject.name, "OnFinishedSaveImage");
		}
	}

	private void OnFinishedSaveImage (string message)
	{
		if (message == LoadTextureFromImagePicker.strCallbackResultMessage_Saved)
		{
			// Save Succeed
		}
		else 
		{
			// Failed
		}
	}
	#endregion
}
