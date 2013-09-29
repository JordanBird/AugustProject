using UnityEngine;
using System;
using System.Collections;

public class cscript_football_game : MonoBehaviour {
	
	public bool playing = false;
	int currentPlayer;
	public Texture2D pitch;
	bool GoalMode = false;
	int playMode = 0; //0 single 1 multi
	public Texture2D awardTexture;
	
	bool endOfGame = false;
	
	Game game;
	cscript_master master;
	
	Texture2D background;
	
	Question currentQuestion;
	Answer[] correctAnswers;
	Answer[] incorrectAnswers;
	int correctTotal = 0;
	int incorrectTotal = 0;
	int CurrentRow = 0;
	int QuestionSeed = 0;
	bool newGeneration = true;
	
	cscript_GUI_master GUIMaster;
	cscript_sound_master soundMaster;
	
	//Player Variables
	int Player1Score = 0;
	int Player2Score = 0;
	string Player1TeamName = "Insert Player 1 Team Name";
	string Player2TeamName = "Insert Player 2 Team Name";
	
	int answerDelay = 0;
	
	Answer[] CurrentSet;
	
	
	public Texture2D football;
	public Texture2D goalShootImage;
	Vector2 currentFootballLoc;
	Vector2 destinationFootballLoc;
	float speed = 0.05f;
	
	Vector2 GoalBallPos;
	Vector2 GoalBallVelocity;
	
	Vector2 DownLocation;
	
	Vector2 mouseLocation;
	
	float timer = 60.0f;
	int selectedtime = 0;
	
	// Use this for initialization
	void Start () 
	{
		background = game.background;
		
		GUIMaster = GameObject.FindGameObjectWithTag ("GUI Master").GetComponent<cscript_GUI_master>();
		soundMaster = GameObject.FindGameObjectWithTag ("Sound Master").GetComponent<cscript_sound_master>();	
		
		NewQuestion();
		
		correctAnswers = new Answer[currentQuestion.answers.Length];
		incorrectAnswers = new Answer[currentQuestion.answers.Length];
		
		for(int i = 0; i < currentQuestion.answers.Length; i++)
		{
			if(currentQuestion.answers[i].correct == true)
			{
				correctAnswers[correctTotal] = currentQuestion.answers[i];
				correctTotal++;
			}
			else
			{
				incorrectAnswers[incorrectTotal] = currentQuestion.answers[i];
				incorrectTotal++;
			}
		}
		
		QuestionSeed = UnityEngine.Random.Range(1,3);
		CurrentSet = new Answer[3];
		
		currentFootballLoc = new Vector2(50,Screen.height/2 - 50);
		destinationFootballLoc = currentFootballLoc;
		
		GoalBallPos = new Vector2(Screen.width/2 - 50,Screen.height - 200);
		GoalBallVelocity = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!endOfGame)
		{
		if(playing)
		{
			currentFootballLoc = Vector2.Lerp(currentFootballLoc, destinationFootballLoc,speed);
			if(!GoalMode)
			{
				if(timer > 0)
				{
				timer -= Time.deltaTime;	
				}
				if(timer <= 0)
				{
					playing = false;
					endOfGame = true;
					timer = 60.0f;
				}
			}
		}
		GoalBallPos += GoalBallVelocity * Time.deltaTime;
		
		
				if(Input.GetMouseButtonDown(0))
				{
					Debug.Log("Down");
					DownLocation = mouseLocation;
				}
				
				if(Input.GetMouseButtonUp(0))
				{
					Debug.Log("Up");
				if(currentPlayer == 2 && playMode == 0)
				{
					//IGNORE PLAYER INPUT
				}
				else
				{
					GoalBallVelocity = -(mouseLocation - DownLocation);
					GoalBallVelocity = new Vector2 (-GoalBallVelocity.x,GoalBallVelocity.y);
				}
				}
			
		if(GoalMode)
		{
			if(GoalBallPos.x < 0)
			{
				GoalMode = false;	
			//CHANGE PLAYER BECAUSE THEY ARE A FAILURE:
				switch(currentPlayer)
				{
					case 1:
						currentPlayer = 2;
						CurrentRow = 3;
						destinationFootballLoc = new Vector2(Screen.width - 150,Screen.height/2 - 50);
						break;
					case 2:
						currentPlayer = 1;
						CurrentRow = 0;
						destinationFootballLoc = new Vector2(50,Screen.height/2 - 50);
						break;
				}
			}
			if(GoalBallPos.x > Screen.width)
			{
				GoalMode = false;	
			//CHANGE PLAYER BECAUSE THEY ARE A FAILURE:
				switch(currentPlayer)
				{
					case 1:
						currentPlayer = 2;
						CurrentRow = 3;
						destinationFootballLoc = new Vector2(Screen.width - 150,Screen.height/2 - 50);
						break;
					case 2:
						currentPlayer = 1;
						CurrentRow = 0;
						destinationFootballLoc = new Vector2(50,Screen.height/2 - 50);
						break;
				}
			}
			if(GoalBallPos.y > Screen.height)
			{
			GoalMode = false;	
			//CHANGE PLAYER BECAUSE THEY ARE A FAILURE:
				switch(currentPlayer)
				{
					case 1:
						currentPlayer = 2;
						CurrentRow = 3;
						destinationFootballLoc = new Vector2(Screen.width - 150,Screen.height/2 - 50);
						break;
					case 2:
						currentPlayer = 1;
						CurrentRow = 0;
						destinationFootballLoc = new Vector2(50,Screen.height/2 - 50);
						break;
				}
			}
			if(GoalBallPos.y < 0)
			{
			GoalMode = false;	
			//CHANGE PLAYER BECAUSE THEY ARE A FAILURE:
				switch(currentPlayer)
				{
					case 1:
						currentPlayer = 2;
						CurrentRow = 3;
						destinationFootballLoc = new Vector2(Screen.width - 150,Screen.height/2 - 50);
						break;
					case 2:
						currentPlayer = 1;
						CurrentRow = 0;
						destinationFootballLoc = new Vector2(50,Screen.height/2 - 50);
						break;
				}
			}
			
			if(currentPlayer == 1)
			{
				if(GoalBallPos.y < (Screen.height/2) - 100)
				{
					if(GoalBallPos.x > (Screen.width / 2) - 100 && GoalBallPos.x < (Screen.width /2) + 100)
					{
					Player1Score++;
					GoalMode = false;
					}
				}
			}
			else
			{
				if(GoalBallPos.y < (Screen.height/2) - 100)
				{
					if(GoalBallPos.x > (Screen.width / 2) - 100 && GoalBallPos.x < (Screen.width /2) + 100)
					{
					Player2Score++;
					GoalMode = false;
					}
				}
			}
		}
		}
	}
	
	void OnGUI()
	{
		
		mouseLocation = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		
		if(playing)
		{
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), background);
			if(GoalMode)
			{
				GUI.DrawTexture (new Rect(0, -100, Screen.width , Screen.height), goalShootImage);	
			}
			else
			{
				GUI.DrawTexture (new Rect(50, 100, Screen.width - 100, Screen.height -200), pitch);
			}
		}
		
		//Banners
		GUI.DrawTexture (new Rect(0, 0, Screen.width, 81), GUIMaster.blankBlackTexture);
		GUI.DrawTexture (new Rect(0, Screen.height -101, Screen.width, 100), GUIMaster.blankBlackTexture);
		GUI.DrawTexture (new Rect(0, 0, Screen.width, 80), GUIMaster.bannerTexture);
		GUI.DrawTexture (new Rect(0, Screen.height - 100, Screen.width, 100), GUIMaster.bannerTexture);
		
		if (GUI.Button (new Rect(15, 15, 40, 40), "",GUIMaster.deleteButton.button))
		{	
			master.gameState = cscript_master.GameState.MainMenu;
			master.gameObject.GetComponent<cscript_navigation>().MainMenuLoad ();
			Destroy(this.gameObject);
		}
		
		if(!endOfGame)
		{
		if (!playing)
		{
			
			//Change the default skin so we can effect the buttons of the GUIContent.
			GUISkin tempSkin = GUI.skin;
			GUI.skin = GUIMaster.buttons;
			
			GUIContent[] g = new GUIContent[2];
			g[0] = new GUIContent(GUIMaster.PlayerIcon);
			g[1] = new GUIContent(GUIMaster.PlayerIcon);
			playMode = GUI.SelectionGrid (new Rect(Screen.width / 2 - 125, 100, 250, 150), playMode, g, 2);
			
			GUI.Label (new Rect(10, 300, Screen.width - 20, 50), "Game Length (Mins)", GUIMaster.heading.label);
			
			GUIContent[] timeige = new GUIContent[3];
			timeige[0] = new GUIContent("2");
			timeige[1] = new GUIContent("5");
			timeige[2] = new GUIContent("10");
			selectedtime = GUI.SelectionGrid (new Rect(Screen.width / 2 - 70, 350,130, 50),selectedtime, timeige, 3);
			
			
			Player1TeamName = GUI.TextArea(new Rect(Screen.width/4 - (Screen.width/8),260,Screen.width/4,30),Player1TeamName, GUIMaster.createGameTextFields.textField);
			Player2TeamName = GUI.TextArea(new Rect(Screen.width/4 * 3 - (Screen.width/8),260,Screen.width/4,30),Player2TeamName, GUIMaster.createGameTextFields.textField);
			
			
			switch(selectedtime)
			{
			case 0:
				timer = 60f * 2;
				break;
			case 1:
				timer = 60f * 5;
				break;
			case 2:
				timer = 60f * 10;
				break;
			}
			
			GUI.skin = tempSkin;
			
			if(GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 70, 120, 40),"Play",GUIMaster.questions.label))
			{
			playing = true;	
			currentPlayer = 1;
			}
			
			GUI.Label (new Rect(10, 20, Screen.width - 20, 50), game.name, GUIMaster.heading.label);
		}
		else
		{
			if(!GoalMode)
			{
			//THE ACTUAL GAME:
			GUI.Label (new Rect(10, 10, Screen.width - 20, 50), Player1TeamName + "  " + Player1Score + "  -  " + Player2Score +"  " + Player2TeamName, GUIMaster.scores.label);
			GUI.Label (new Rect(10, Screen.height - 120, Screen.width - 20, 100), currentQuestion.text, GUIMaster.questions.label);
			
			switch(currentPlayer)
			{
				case 1:
					GUI.Label (new Rect(10, Screen.height - 80, Screen.width - 20, 100), "Player 1's Turn     Time Left: " + Math.Ceiling(timer) + " Seconds", GUIMaster.scores.label);
					break;
				case 2:
					GUI.Label (new Rect(10, Screen.height - 80, Screen.width - 20, 100), "Player 2's Turn     Time Left: " + Math.Ceiling(timer)+ " Seconds", GUIMaster.scores.label);
					break;
			}
			

			if(newGeneration)
			{
			switch (QuestionSeed)
				{
				case 1: //TFF
					CurrentSet[0] = correctAnswers[UnityEngine.Random.Range(0,correctTotal)];
					CurrentSet[1] = incorrectAnswers[UnityEngine.Random.Range(0,incorrectTotal)];
					CurrentSet[2] = incorrectAnswers[UnityEngine.Random.Range(0,incorrectTotal)];
					break;
				case 2: //FTF
					CurrentSet[0] = incorrectAnswers[UnityEngine.Random.Range(0,incorrectTotal)];	
					CurrentSet[1] = correctAnswers[UnityEngine.Random.Range(0,correctTotal)];
					CurrentSet[2] = incorrectAnswers[UnityEngine.Random.Range(0,incorrectTotal)];
					break;
				case 3: //FFT
					CurrentSet[0] = incorrectAnswers[UnityEngine.Random.Range(0,incorrectTotal)];	
					CurrentSet[1] = incorrectAnswers[UnityEngine.Random.Range(0,incorrectTotal)];
					CurrentSet[2] = correctAnswers[UnityEngine.Random.Range(0,correctTotal)];
					break;
				}
				newGeneration = false;
			}
			
			int x = CurrentRow;	
			for(int y = 0; y < 3; y++)
			{
				if(playMode == 0) //SINGLEPLAYER
				{
					if(currentPlayer == 1)
					{
						if(CurrentSet[y].texture == null)
						{
							if(GUI.Button(new Rect(150 + (((Screen.width - 300) / 4) * x),120 + (((Screen.height - 240) / 3) * y),(Screen.width - 300) / 4,(Screen.height - 240) / 3), CurrentSet[y].text,GUIMaster.footySkin.button))
							{
								AnswerHandling (x, y);
							}
						}
						else
						{
							if(GUI.Button(new Rect(150 + (((Screen.width - 300) / 4) * x),120 + (((Screen.height - 240) / 3) * y),(Screen.width - 300) / 4,(Screen.height - 240) / 3), CurrentSet[y].texture,GUIMaster.footySkin.button))
							{
								AnswerHandling (x, y);
							}
						}
					}
					else //AI
					{
						if(CurrentSet[y].texture == null)
						{
							if(GUI.Button(new Rect(150 + (((Screen.width - 300) / 4) * x),120 + (((Screen.height - 240) / 3) * y),(Screen.width - 300) / 4,(Screen.height - 240) / 3), CurrentSet[y].text,GUIMaster.footySkin.button))
							{
								
							}
						}
						else
						{
							if(GUI.Button(new Rect(150 + (((Screen.width - 300) / 4) * x),120 + (((Screen.height - 240) / 3) * y),(Screen.width - 300) / 4,(Screen.height - 240) / 3), CurrentSet[y].texture,GUIMaster.footySkin.button))
							{
								
							}
						}
						if(answerDelay == 400)
						{
						int randomnumber = UnityEngine.Random.Range(0,100);
							
							switch (QuestionSeed)
							{
							case 1: //TFF
								if(randomnumber <= 75)
								{
								AnswerHandling(x, 0);
								}
								else
								{
								AnswerHandling(x, 1);
								}
							break;
							case 2: //FTF
								if(randomnumber <= 75)
									{
									AnswerHandling(x, 1);
									}
									else
									{
									AnswerHandling(x, 0);
									}
							break;
							case 3: //FFT
								if(randomnumber <= 75)
								{
								AnswerHandling(x, 2);
								}
								else
								{
								AnswerHandling(x, 1);
								}
							break;
							}
						answerDelay = 0;
						}
						else
						{
							answerDelay++;
						}
					}
				}
				else //MULTIPLAYER:
				{
					if(currentPlayer == 1)
					{
						if(CurrentSet[y].texture == null)
						{
							if(GUI.Button(new Rect(150 + (((Screen.width - 300) / 4) * x),120 + (((Screen.height - 240) / 3) * y),(Screen.width - 300) / 4,(Screen.height - 240) / 3), CurrentSet[y].text,GUIMaster.footySkin.button))
							{
								AnswerHandling (x, y);
							}
						}
						else
						{
							if(GUI.Button(new Rect(150 + (((Screen.width - 300) / 4) * x),120 + (((Screen.height - 240) / 3) * y),(Screen.width - 300) / 4,(Screen.height - 240) / 3), CurrentSet[y].texture,GUIMaster.footySkin.button))
							{
								AnswerHandling (x, y);
							}
						}
					}
					else
					{
						if(CurrentSet[y].texture == null)
						{
							if(GUI.Button(new Rect(150 + (((Screen.width - 300) / 4) * x),120 + (((Screen.height - 240) / 3) * y),(Screen.width - 300) / 4,(Screen.height - 240) / 3), CurrentSet[y].text,GUIMaster.footySkin.button))
							{
								AnswerHandling (x, y);
							}
						}
						else
						{
							if(GUI.Button(new Rect(150 + (((Screen.width - 300) / 4) * x),120 + (((Screen.height - 240) / 3) * y),(Screen.width - 300) / 4,(Screen.height - 240) / 3), CurrentSet[y].texture,GUIMaster.footySkin.button))
							{
								AnswerHandling (x, y);
							}
						}
					}	
				}
			}
			
			//Draw the football:
			GUI.DrawTexture (new Rect(currentFootballLoc.x, currentFootballLoc.y, 100, 100), football);
			}
			else
			{
			GUI.DrawTexture (new Rect(GoalBallPos.x, GoalBallPos.y, 0.5f * GoalBallPos.y, 0.5f * GoalBallPos.y), football);
			//Banners
			GUI.DrawTexture (new Rect(0, 0, Screen.width, 81), GUIMaster.blankBlackTexture);
			GUI.DrawTexture (new Rect(0, Screen.height -101, Screen.width, 100), GUIMaster.blankBlackTexture);
			GUI.DrawTexture (new Rect(0, 0, Screen.width, 80), GUIMaster.bannerTexture);
			GUI.DrawTexture (new Rect(0, Screen.height - 100, Screen.width, 100), GUIMaster.bannerTexture);
		
			if (GUI.Button (new Rect(15, 15, 40, 40), "",GUIMaster.deleteButton.button))
			{	
				master.gameState = cscript_master.GameState.MainMenu;
				master.gameObject.GetComponent<cscript_navigation>().MainMenuLoad ();
				Destroy(this.gameObject);
			}
			}
		}
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width / 5 * 2, 130,Screen.width / 5,Screen.height / 3), awardTexture);
			
			if(Player1Score > Player2Score)
			{
			GUI.Label (new Rect(10, Screen.height - 240, Screen.width - 20, 100), "Congratulations " + Player1TeamName + " You Win!", GUIMaster.questions.label);
			}
			else if (Player2Score > Player1Score)
			{
			GUI.Label (new Rect(10, Screen.height - 240, Screen.width - 20, 100), "Congratulations " + Player2TeamName + " You Win!", GUIMaster.questions.label);
			}
			else
			{
			GUI.Label (new Rect(10, Screen.height - 240, Screen.width - 20, 100), "It's a tie! So close!", GUIMaster.questions.label);	
			}
		}
	}
	
	private void NewQuestion()
	{
		currentQuestion = game.GetQuestion ();
		currentQuestion.RandomizeAnswers ();
	}
	
	public void Begin(Game g, cscript_master m)
	{
		//playing = true;
		game = g;
		master = m;
		
		try
		{
			background = game.background;
		}
		catch {}
		
		NewQuestion ();
	}
	
	public void Stop()
	{
		playing = false;
		Destroy (this.gameObject);
	}
	
	void AnswerHandling (int x, int y)
	{
		if(CurrentSet[y].correct)
			{
			//WINNER
				soundMaster.PlaySound(soundMaster.correctAnswer);
				
				destinationFootballLoc = new Vector2 (190 + (((Screen.width - 300) / 4) * x),130 + (((Screen.height - 240) / 3) * y));
				
				switch(currentPlayer)
				{
					case 1:
						CurrentRow++;
						if(CurrentRow == 4)
						{
						GoalMode = true;
						GoalBallVelocity = Vector2.zero;
						GoalBallPos = new Vector2(Screen.width/2 - 50,Screen.height - 200);
						
						CurrentRow = 0;
						destinationFootballLoc = new Vector2(50,Screen.height/2 - 50);
						}
						break;
					case 2:
						CurrentRow--;
						if(CurrentRow == -1)
						{
						GoalMode = true;
						if(playMode == 0)
					{
						GoalBallVelocity = new Vector2(UnityEngine.Random.Range(-100,100),-170);
					}
					else
					{
						GoalBallVelocity = Vector2.zero;
					}
						GoalBallPos = new Vector2(Screen.width/2 - 50,Screen.height - 200);
						
						CurrentRow = 3;
						destinationFootballLoc = new Vector2(Screen.width - 150,Screen.height/2 - 50);
						}
						break;
				}
				
				QuestionSeed = UnityEngine.Random.Range(1,3);
				newGeneration = true;
				
			}
			else
			{
				//FAILURE
				soundMaster.PlaySound(soundMaster.incorrectAnswer);
				
				QuestionSeed = UnityEngine.Random.Range(1,3);
				newGeneration = true;
				//CHANGE PLAYER BECAUSE THEY ARE A FAILURE:
				switch(currentPlayer)
				{
					case 1:
						currentPlayer = 2;
						CurrentRow = 3;
						destinationFootballLoc = new Vector2(Screen.width - 150,Screen.height/2 - 50);
						break;
					case 2:
						currentPlayer = 1;
						CurrentRow = 0;
						destinationFootballLoc = new Vector2(50,Screen.height/2 - 50);
						break;
				}
			}
			CurrentRow = Mathf.Clamp(CurrentRow,0,3);
		}
	}

