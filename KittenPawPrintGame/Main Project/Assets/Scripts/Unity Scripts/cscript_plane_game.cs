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
	
	// Use this for initialization
	void Start ()
	{
		GUIMaster = GameObject.FindGameObjectWithTag ("GUI Master").GetComponent<cscript_GUI_master>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
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
		
		GUI.Label (new Rect(10, -20, Screen.width - 20, 100), currentQuestion.text, GUIMaster.questions.label);
		
		if (GUI.Button (new Rect(10, 10, 100, 30), "Quit"))
		{			
			master.gameState = cscript_master.GameState.MainMenu;
			master.gameObject.GetComponent<cscript_navigation>().MainMenuLoad ();
			
			Stop ();
		}
	}
	
	public void Stop()
	{
		Aeroplane.GetComponent<AeroplaneController>().skin.gameObject.SetActive(false);
		Aeroplane.GetComponent<AeroplaneController>().enabled = false;
		playing = false;
		AnswerSpawner.GetComponent<AnswerSpawner>().Cleanup();
		Destroy (this.gameObject);
	}
	
	public void WinGame()
	{
		// ???
		master.gameState = cscript_master.GameState.MainMenu;
		master.gameObject.GetComponent<cscript_navigation>().MainMenuLoad ();
		
		Stop ();
	}
}
