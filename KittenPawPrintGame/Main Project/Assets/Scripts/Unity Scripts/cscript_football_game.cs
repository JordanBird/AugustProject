using UnityEngine;
using System;
using System.Collections;

public class cscript_football_game : MonoBehaviour {
	
	public bool playing = false;
	int currentPlayer;
	public Texture2D pitch;
	bool GoalMode = false;
	
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
	
	Answer[] CurrentSet;
	
	
	public Texture2D football;
	Vector2 currentFootballLoc;
	Vector2 destinationFootballLoc;
	float speed = 0.05f;
	
	float timer = 60.0f;
	
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(playing)
		{
			currentFootballLoc = Vector2.Lerp(currentFootballLoc, destinationFootballLoc,speed);
			if(timer > 0)
			{
			timer -= Time.deltaTime;	
			}
			if(timer <= 0)
			{
				playing = false;	
				timer = 60.0f;
			}
		}
		
	}
	
	void OnGUI()
	{
		if(playing)
		{
		GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), background);
		GUI.DrawTexture (new Rect(50, 100, Screen.width - 100, Screen.height -200), pitch);	
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
		
		if (!playing)
		{
			if(GUI.Button(new Rect(300,300,200,40),"Play"))
			{
			playing = true;	
			currentPlayer = 1;
			}
		}
		else
		{
			GUI.Label (new Rect(10, 10, Screen.width - 20, 50), "Team 1 " + Player1Score + "  -  " + Player2Score +"  Team 2", GUIMaster.scores.label);
			GUI.Label (new Rect(10, Screen.height - 120, Screen.width - 20, 100), currentQuestion.text, GUIMaster.questions.label);
			
			switch(currentPlayer)
			{
				case 1:
					GUI.Label (new Rect(10, Screen.height - 80, Screen.width - 20, 100), "Player 1's Turn  Time Left:   " + Math.Ceiling(timer) + " Seconds", GUIMaster.scores.label);
					break;
				case 2:
					GUI.Label (new Rect(10, Screen.height - 80, Screen.width - 20, 100), "Player 2's Turn  Time Left:   " + Math.Ceiling(timer)+ " Seconds", GUIMaster.scores.label);
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
				if(GUI.Button(new Rect(150 + (((Screen.width - 300) / 4) * x),120 + (((Screen.height - 240) / 3) * y),(Screen.width - 300) / 4,(Screen.height - 240) / 3), CurrentSet[y].text,GUIMaster.footySkin.button))
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
									Player1Score++;
									CurrentRow = 0;
									destinationFootballLoc = new Vector2(50,Screen.height/2 - 50);
									}
									break;
								case 2:
									CurrentRow--;
									if(CurrentRow == -1)
									{
									Player2Score++;
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
			
			//Draw the football:
			GUI.DrawTexture (new Rect(currentFootballLoc.x, currentFootballLoc.y, 100, 100), football);
			
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
}
