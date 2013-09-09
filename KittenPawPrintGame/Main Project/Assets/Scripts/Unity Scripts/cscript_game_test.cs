using UnityEngine;
using System;
using System.Collections;

public class cscript_game_test : MonoBehaviour {
	
	public bool playing = false;
	Game game;
	cscript_master master;
	
	Texture2D background;
	
	Question currentQuestion;
	
	cscript_GUI_master GUIMaster;
	
	//Player Variables
	int score = 0;
	
	// Use this for initialization
	void Start () 
	{
		background = game.background;
		
		GUIMaster = GameObject.FindGameObjectWithTag ("GUI Master").GetComponent<cscript_GUI_master>();
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
			
			//Banners
			GUI.DrawTexture (new Rect(0, 0, Screen.width, 64), GUIMaster.bannerTexture);
			GUI.DrawTexture (new Rect(0, Screen.height - 100, Screen.width, 100), GUIMaster.bannerTexture);
			
			if (GUI.Button (new Rect(10, 10, 100, 30), "Quit"))
			{
				master.gameState = cscript_master.GameState.MainMenu;
				master.gameObject.GetComponent<cscript_navigation>().MainMenuLoad ();
				Destroy(this.gameObject);
			}

			GUI.Label (new Rect(10, 10, Screen.width - 20, 50), "Score: " + score, GUIMaster.scores.label);
			GUI.Label (new Rect(10, Screen.height - 100, Screen.width - 20, 100), currentQuestion.text, GUIMaster.questions.label);

			int inc = 1;
			
			foreach (Answer a in currentQuestion.answers)
			{
				if (a.type == 0)
				{
					//Text
					if (GUI.Button(new Rect(inc * 10 + (inc * (Screen.width / (currentQuestion.answers.Length + 2))), 78, Screen.width / (currentQuestion.answers.Length + 2), 150), a.text))
					{
						if (a.correct == true)
						{
							//Win
							score++;
							NewQuestion ();
						}
						else
						{
							//Lose
						}
					}
				}
				else if (a.type == 1)
				{
					//Image
					if (GUI.Button(new Rect(inc * 10 + (inc * (Screen.width / (currentQuestion.answers.Length + 2))), 78, Screen.width / (currentQuestion.answers.Length + 2), 150), a.texture))
					{
						if (a.correct == true)
						{
							//Win
							score++;
							NewQuestion ();
						}
						else
						{
							//Lose
						}
					}
				}
				
				inc++;
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
