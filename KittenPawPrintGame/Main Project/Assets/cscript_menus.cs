using UnityEngine;
using System.Collections;

public class cscript_navigation : MonoBehaviour 
{
	cscript_master master;
	
	Texture2D blankWhiteTexture = new Texture2D(1, 1);
	Texture2D blankBlackTexture = new Texture2D(1, 1);
	
	//Create Game Variables
	string gameName = "Add Game Name Here";
	string authorName = "Add Author Here";
	
	int selectedCreateGame = 0;
	
	public void Init(cscript_master m)
	{
		master = m;
	}
	
	// Use this for initialization
	void Start ()
	{
		blankWhiteTexture.SetPixel (0, 0, Color.white);
		blankWhiteTexture.Apply();
			
		blankBlackTexture.SetPixel (0, 0, Color.black);
		blankBlackTexture.Apply();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnGUI()
	{
		switch (master.gameState)
		{
			case cscript_master.GameState.MainMenu:
				MainMenuGUI ();
				break;
			case cscript_master.GameState.Help:
				HelpGUI ();
				break;
			case cscript_master.GameState.About:
				AboutGUI ();
				break;
			case cscript_master.GameState.CreateGame:
				CreateGameGUI ();
				break;
			case cscript_master.GameState.Playing:
				break;
				
		}
	}
	
	private void MainMenuGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "Main Menu");
		
		if (GUI.Button (new Rect(10, 10, 100, 30), "About"))
			master.gameState = cscript_master.GameState.About;
		
		if (GUI.Button (new Rect(Screen.width - 110, 10, 100, 30), "Help"))
			master.gameState = cscript_master.GameState.Help;
		
		if (GUI.Button (new Rect(Screen.width / 2 - 50, Screen.height - 40, 100, 30), "Create Game"))
			master.gameState = cscript_master.GameState.CreateGame;
		
		float inc = (Screen.width - 40) / 3;
		
		//Border
		GUI.DrawTexture (new Rect(9, 59, inc + 2, Screen.height - 178), blankBlackTexture);
		GUI.DrawTexture (new Rect(inc + 19, 59, inc + 2, Screen.height - 178), blankBlackTexture);
		GUI.DrawTexture (new Rect(inc * 2 + 29, 59, inc + 2, Screen.height - 178), blankBlackTexture);
		
		//Content
		GUI.DrawTexture (new Rect(10, 60, inc, Screen.height - 180), blankWhiteTexture);
		GUI.DrawTexture (new Rect(inc + 20, 60, inc, Screen.height - 180), blankWhiteTexture);
		GUI.DrawTexture (new Rect(inc * 2 + 30, 60, inc, Screen.height - 180), blankWhiteTexture);
	}
	
	private void HelpGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "Help");
		
		if (GUI.Button (new Rect(10, 10, 100, 30), "< Back"))
			master.gameState = cscript_master.GameState.MainMenu;
	}
	
	private void AboutGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 50, 10, 100, 50), "About");
		
		if (GUI.Button (new Rect(10, 10, 100, 30), "< Back"))
			master.gameState = cscript_master.GameState.MainMenu;
	}
	
	private void CreateGameGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 60, 10, 120, 50), "Create your Game");
		
		if (GUI.Button (new Rect(10, 10, 100, 30), "< Back"))
			master.gameState = cscript_master.GameState.MainMenu;
		
		if (GUI.Button (new Rect(Screen.width - 110, 10, 100, 30), "Help"))
			master.gameState = cscript_master.GameState.Help;
		
		gameName = GUI.TextField (new Rect((Screen.width / 4 - 10) / 2, 60, Screen.width / 4, 20), gameName);
		authorName = GUI.TextField (new Rect((Screen.width / 4 - 10) * 2.5f, 60, Screen.width / 4, 20), authorName);
		
		GUIContent[] g = new GUIContent[3];
		
		g[0] = new GUIContent("Football");
		g[1] = new GUIContent("Plane");
		g[2] = new GUIContent("Train");
		
		float centre = (Screen.width / 4);
		
		selectedCreateGame = GUI.SelectionGrid (new Rect(centre, 100, Screen.width / 2 - 10, 60), selectedCreateGame, g, 3);
	}
}
