using UnityEngine;
using System.Collections;

public class cscript_plane_game : MonoBehaviour
{
	public Question currentQuestion { get; private set; }
	public Transform AnswerSpawner;
	public Transform Aeroplane;
	public bool playing;
	
	private Game game;
	private cscript_master master;
	private cscript_sound_master sound;
	private Texture2D background;
	private cscript_GUI_master GUIMaster;
	private FancyButton BackButton;
	
	// Use this for initialization
	void Start ()
	{
		GUIMaster = GameObject.FindGameObjectWithTag ("GUI Master").GetComponent<cscript_GUI_master>();
		BackButton = new FancyButton("Back To Menu", Screen.width / 2 - 100, Screen.height / 2 + 300, 200, 60, 0.5f, 2);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!playing)
		{
			if (Aeroplane.transform.position.x < 10)	
			{
				Aeroplane.transform.position += new Vector3(5f * Time.deltaTime, 0, 0);
			}
			
			if (BackButton.Clicked)
			{
				Stop ();	
			}
			
			BackButton.Update ();
		}
	}
	
	private void NewQuestion()
	{
		currentQuestion = game.GetQuestion ();
		currentQuestion.RandomizeAnswers ();
	}
	
	public void Begin(Game g, cscript_master m)
	{
		AnswerSpawner = GameObject.Find ("AeroplaneAnswerSpawner").transform;
		Aeroplane = GameObject.Find ("Aeroplane").transform;
		Aeroplane.GetComponent<AeroplaneController>().enabled = true;
		Aeroplane.GetComponent<AeroplaneController>().skin.gameObject.SetActive(true);
		Aeroplane.GetComponent<AeroplaneController>().Reset();
		
		playing = true;
		game = g;
		master = m;
		
		try
		{
			background = game.background;
		}
		catch {}
		
		NewQuestion ();
		
		AnswerSpawner.GetComponent<AnswerSpawner>().Begin (this);
	}
	
	public void OnGUI()
	{
		//Banners
		//Banners
		GUI.DrawTexture (new Rect(0, 0, Screen.width, 81), GUIMaster.blankBlackTexture);
		GUI.DrawTexture (new Rect(0, Screen.height -101, Screen.width, 100), GUIMaster.blankBlackTexture);
		GUI.DrawTexture (new Rect(0, 0, Screen.width, 80), GUIMaster.bannerTexture);
		GUI.DrawTexture (new Rect(0, Screen.height - 100, Screen.width, 100), GUIMaster.bannerTexture);
		
		if (playing)
		{
			GUI.Label (new Rect(10, -20, Screen.width - 20, 100), currentQuestion.text, GUIMaster.questions.label);
			GUI.Label (new Rect(10, Screen.height - 120,Screen.width - 20, 100), "Correct Answers Collected: " + AnswerSpawner.GetComponent<AnswerSpawner>().GetCorrectScore().ToString() + "/" + currentQuestion.GetNumberOfCorrectAnswers().ToString(), GUIMaster.questions.label);
			GUI.Label (new Rect(10, Screen.height - 80,Screen.width - 20, 100), "Wrong Answers Collected: " + AnswerSpawner.GetComponent<AnswerSpawner>().GetIncorrectScore().ToString(), GUIMaster.questions.label);
			
			if (GUI.Button (new Rect(15, 15, 40, 40), "",GUIMaster.deleteButton.button))
			{			
				master.gameState = cscript_master.GameState.MainMenu;
				master.gameObject.GetComponent<cscript_navigation>().MainMenuLoad ();
				
				Stop ();
			}
		}
		else
		{			
			GUI.Label (new Rect(10, 240, Screen.width - 20, 100), "You have completed: \n" + currentQuestion.text + "!", GUIMaster.questions.label);
			
			int incorrect = AnswerSpawner.GetComponent<AnswerSpawner>().GetIncorrectScore();
			string remark = 
				incorrect == 0 ? "Perfect!" : 
				incorrect == 1 ? "Almost perfect." :
				incorrect == 2 ? "Good attempt." :
				incorrect < 5 ? "Oh no!" : "Very bad!";
			
			GUI.Label (new Rect(10, 300, Screen.width - 20, 200), remark + "\n\n\nYou " + ((incorrect != 0 && incorrect < 3) ? "only hit " : "hit ") + incorrect.ToString() + " wrong answers!", GUIMaster.questions.label);
			
			BackButton.Clicked = GUI.Button (BackButton.GetRectangle(), BackButton.Content);
		}
	}
	
	public void Stop()
	{
		Aeroplane.GetComponent<AeroplaneController>().skin.gameObject.SetActive(false);
		Aeroplane.GetComponent<AeroplaneController>().enabled = false;
		playing = false;
		AnswerSpawner.GetComponent<AnswerSpawner>().Cleanup();
		
		master.gameState = cscript_master.GameState.MainMenu;
		master.gameObject.GetComponent<cscript_navigation>().MainMenuLoad ();
		
		Destroy (this.gameObject);
	}
	
	public void WinGame()
	{
		playing = false;
		Aeroplane.GetComponent<AeroplaneController>().Controllable = false;
		BackButton.Enter ();
	}
}
