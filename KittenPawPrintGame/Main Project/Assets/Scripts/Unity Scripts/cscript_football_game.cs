using UnityEngine;
using System;
using System.Collections;

public class cscript_football_game : MonoBehaviour {
	
	public bool playing = false;
	public enum FootballGameState {Menu, MultiplayerGame, SingleplayerGame, ScoreGoalMode};
	public Texture2D pitch;
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
			
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnGUI()
	{
		if (playing == true)
		{
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), background);
			GUI.DrawTexture (new Rect(50, 100, Screen.width - 100, Screen.height -200), pitch);
			
			//Banners
			GUI.DrawTexture (new Rect(0, 0, Screen.width, 81), GUIMaster.blankBlackTexture);
			GUI.DrawTexture (new Rect(0, Screen.height -101, Screen.width, 100), GUIMaster.blankBlackTexture);
			GUI.DrawTexture (new Rect(0, 0, Screen.width, 80), GUIMaster.bannerTexture);
			GUI.DrawTexture (new Rect(0, Screen.height - 100, Screen.width, 100), GUIMaster.bannerTexture);
			
			if (GUI.Button (new Rect(10, 10, 100, 30), "Quit"))
			{
				master.gameState = cscript_master.GameState.MainMenu;
				master.gameObject.GetComponent<cscript_navigation>().MainMenuLoad ();
				Destroy(this.gameObject);
			}

			GUI.Label (new Rect(10, 10, Screen.width - 20, 50), "Team 1 " + Player1Score + "  -  " + Player2Score +"  Team 2", GUIMaster.scores.label);
			GUI.Label (new Rect(10, Screen.height - 100, Screen.width - 20, 100), currentQuestion.text, GUIMaster.questions.label);
			
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
				if(GUI.Button(new Rect(150 + (((Screen.width - 300) / 3) * x),120 + (((Screen.height - 240) / 3) * y),(Screen.width - 300) / 3,(Screen.height - 240) / 3), CurrentSet[y].text))
				{
					if(CurrentSet[y].correct)
					{
					//WINNER
					soundMaster.PlaySound(soundMaster.correctAnswer);
					CurrentRow++;
					QuestionSeed = UnityEngine.Random.Range(1,3);
					newGeneration = true;
					}
					else
					{
					//FAILURE
					soundMaster.PlaySound(soundMaster.incorrectAnswer);
					CurrentRow--;
					QuestionSeed = UnityEngine.Random.Range(1,3);
					newGeneration = true;
					}
					CurrentRow = Mathf.Clamp(CurrentRow,0,2);
				}
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
		playing = true;
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
