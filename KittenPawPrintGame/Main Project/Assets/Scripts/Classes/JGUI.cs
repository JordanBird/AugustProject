using UnityEngine;
using System.Collections;

public class JGUI
{
	//--------------------------\\
	//Message Box Variables
	//--------------------------\\
	int returnButton = -1;
	string[] buttons;
	
	public int MessageBox(int iID, string iTitle, string[] iButtons)
	{
		
		Texture2D overlay = new Texture2D(1, 1);
		overlay.SetPixel (0, 0, new Color(1, 1, 1, 0.5f));
		overlay.Apply ();
		
		GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), overlay);
		
		buttons = iButtons;

		//GUILayout.Window (iID, new Rect((Screen.width / 2) - (iButtons.Length * 50), Screen.height / 2 - 25, iButtons.Length * 100, 50), MessageBoxDisplay, iTitle);
		GUILayout.Window (iID, new Rect((Screen.width / 2) - 100, (Screen.height / 2) - (iButtons.Length * 40) / 2, 200, iButtons.Length * 40 + 10), MessageBoxDisplay, iTitle);
	
		return returnButton;
	}
	
	private void MessageBoxDisplay(int id)
	{
		GUILayout.Space (10);
		int mark = 10;
		
		for (int i = 0; i < buttons.Length; i++)
		{
			if (GUILayout.Button(buttons[i]))
				returnButton = i;
		}
		
		GUILayout.Space (10);
	}
	
	public void ResetMessageBox()
	{
		returnButton = -1;	
	}
}