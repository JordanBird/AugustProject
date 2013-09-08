using UnityEngine;
using System.Collections;

public class cscript_master : MonoBehaviour 
{
	public enum GameState {LoadingScreen, MainMenu, Help, About, CreateGame, Playing};
	public GameState gameState = GameState.MainMenu;
	
	public Game currentGame;
	
	public string dataPath = "";
	
	public GameObject game0;
	public GameObject game1;
	public GameObject game2;
	
	// Use this for initialization
	void Start ()
	{
		//Fetches a permanent, writeable location for the system.
		dataPath = Application.persistentDataPath;
		
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
	
	public void StartGame()
	{
		gameState = GameState.Playing;
		
		switch (currentGame.type)
		{
			case "0":
				game0.GetComponent<cscript_game_test>().Begin (currentGame, this);
				break;
		}
	}
	
	public void StartGame(Game g)
	{
		currentGame = g;
		
		gameState = GameState.Playing;
		
		//For each different game type we load a different script.
		switch (currentGame.type)
		{
			case "0":
				GameObject temp0 = Instantiate (game0, Vector3.zero, Quaternion.identity) as GameObject;
				temp0.GetComponent<cscript_game_test>().Begin (currentGame, this);
				break;
			case "1":
				GameObject temp1 = Instantiate (game1, Vector3.zero, Quaternion.identity) as GameObject;
				temp1.GetComponent<cscript_game_test>().Begin (currentGame, this);
				break;
			case "2":
				GameObject temp2 = Instantiate (game2, Vector3.zero, Quaternion.identity) as GameObject;
				temp2.GetComponent<cscript_game_test>().Begin (currentGame, this);
				break;
		}
	}
	
	public void DealWithBytes()
	{
		//Reads a file and then creates the byte string.
		//string outpuit = System.Text.Encoding.Default.GetString (System.IO.File.ReadAllBytes (@"C:\Users\Jordan\Desktop\IMAG0033.jpg"));

		//File.WriteAllText (@"C:\Users\Jordan\Desktop\BBB.txt", outpuit);
		
		
		//Converts Back
		
		//gameBackgrounds[0].LoadImage (System.Text.Encoding.Default.GetBytes (games[position].background));
	}
}
