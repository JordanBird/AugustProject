using UnityEngine;
using System.Collections;

public class cscript_GUI_master : MonoBehaviour 
{
	public Texture2D blankWhiteTexture;
	public Texture2D blankBlackTexture;

	public GUISkin heading;
	public GUISkin text;
	
	public GUISkin correctAnswer;
	public GUISkin incorrectAnswer;
	public GUISkin mainMenuGames;
	public GUISkin questions;
	public GUISkin scores;
	
	public Texture2D footballIcon;
	public Texture2D planeIcon;
	public Texture2D trainIcon;
	
	public Texture2D bannerTexture;
	
	public GUISkin leftArrow;
	public GUISkin rightArrow;
	
	public Texture2D bin;
	
	// Use this for initialization
	void Start () 
	{
		blankWhiteTexture = new Texture2D(1, 1);
		blankBlackTexture = new Texture2D(1, 1);
		
		blankWhiteTexture.SetPixel (0, 0, Color.white);
		blankWhiteTexture.Apply();
			
		blankBlackTexture.SetPixel (0, 0, Color.black);
		blankBlackTexture.Apply();
		
		bannerTexture = new Texture2D(1, 1);
		bannerTexture.SetPixel (0, 0, new Color32(197, 198, 200, 200));
		bannerTexture.Apply();
	}
}
