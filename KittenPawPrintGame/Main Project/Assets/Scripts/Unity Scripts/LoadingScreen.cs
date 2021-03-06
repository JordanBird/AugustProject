﻿using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
	public Texture2D loadingTexture;
	
	public bool show = false;
	bool useTimer = false;
	
	float timeLeft = 0;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public void Show()
	{
		show = true;
		
		if (loadingTexture != null)
		{
			try
			{
				//GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), loadingTexture);
				OnGUI ();
				
				Debug.Log ("Worked");
			}
			catch {}
			
			Debug.Log ("Test");
		}
	}
	
	public void Show(float timeToShow)
	{
		useTimer = true;
		timeLeft = timeToShow;
		
		show = true;
	}
	
	public void Hide()
	{
		show = false;
	}
	
	public void Draw()
	{
		GUI.depth = 0;
		Debug.Log(loadingTexture);
		if (loadingTexture != null)
			Graphics.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), loadingTexture);
		else
		{
			Texture2D t = new Texture2D(1, 1);
			t.SetPixel (0, 0, Color.white);
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), t);
			
			GUI.Label (new Rect(10, 10, 100, 30), "Loading..");
		}
	}
	
	void OnGUI()
	{
		if (show)
		{
			if (useTimer)
			{
				if (timeLeft <= 0)
				{
					timeLeft = 0;
					useTimer = false;
					show = false;
				}
			}
			
			Draw ();
			Debug.Log ("Drew");
			//Debug.Break ();
		}
	}
}
