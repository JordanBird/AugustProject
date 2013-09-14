using UnityEngine;
using System.Collections;

public class cscript_plane_game : MonoBehaviour
{
	public Transform AnswerSpawner;
	public Transform Aeroplane;
	public bool playing;
	
	private Game game;
	private cscript_master master;
	private cscript_sound_master sound;
	private Texture2D background;
	public Question currentQuestion { get; private set; }
	
	// Use this for initialization
	void Start ()
	{
		
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
	
	public void Stop()
	{
		playing = false;
		AnswerSpawner.GetComponent<AnswerSpawner>().Cleanup();
		Destroy (this.gameObject);
	}
	
	public void WinGame()
	{
		// ???
		Stop ();
	}
}
