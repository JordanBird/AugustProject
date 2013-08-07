using UnityEngine;
using System.Collections;

public class cscript_master : MonoBehaviour 
{
	public enum GameState {LoadingScreen, MainMenu, Help, About, CreateGame, Playing};
	public GameState gameState = GameState.MainMenu;
	
	public Game currentGame;
	
	public static string dataPath = Application.dataPath;
	
	// Use this for initialization
	void Start ()
	{
		gameObject.AddComponent<cscript_navigation>().Init(this);
//		currentGame = new Game(@"C:\Users\Jordan\Desktop\Test XML File.xml");
//		Debug.Log (currentGame.name);
//		Debug.Log (currentGame.author);
//		
//		foreach(Question q in currentGame.questions)
//		{
//			Debug.Log (q.text);
//			
//			foreach(Answer a in q.answers)
//			{
//				Debug.Log (a.text);
//				Debug.Log(a.correct);
//			}
//		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (gameState)
		{
			case GameState.MainMenu:
				break;
			case GameState.Help:
				break;
			case GameState.About:
				break;
			case GameState.CreateGame:
				break;
			case GameState.Playing:
				break;
				
		}
	}
}
