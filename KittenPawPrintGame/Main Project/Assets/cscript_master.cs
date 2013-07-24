using UnityEngine;
using System.Collections;

public class cscript_master : MonoBehaviour 
{
	public enum GameState {LoadingScreen, MainMenu, Help, About, CreateGame, Playing};
	public GameState gameState = GameState.MainMenu;
	
	// Use this for initialization
	void Start () 
	{
		gameObject.AddComponent<cscript_navigation>().Init(this);
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
