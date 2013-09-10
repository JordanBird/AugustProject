﻿using UnityEngine;
using System.Collections;

public class FancyButton
{
	public bool Clicked { get; set; }
	public Vector2 Position { get; private set; }
	public GUIContent Content { get; private set; }
	public int Width { get; private set; }
	public int Height { get; private set; }
	public GUISkin Skin { get; private set; }
	public Vector2 targetPosition { get; private set; }
	
	private float introTimer = 0;
	private float introDelay;
	private Vector2 startPosition;
	private int status = 0;	// -1 = exit, 0 = stationary, 1 = enter
	
	public FancyButton(string text, float targetX, float targetY, int width, int height, float introDelay, int side)
	{
		Content = new GUIContent(text);
		Width = width;
		Height = height;
		this.introDelay = introDelay;	
		targetPosition = startPosition = new Vector2(targetX, targetY);
	
		switch (side)
		{
			// Top
			case 0:
				startPosition.y = -Height;
				break;
			
			// Right
			case 1:
				startPosition.x = Screen.width;
				break;
			
			// Bottom
			case 2:
				startPosition.y = Screen.height;
				break;
			
			// Left
			case 3:
				startPosition.x = -Width;
				break;
		}
		
		Position = startPosition;
	}
	
	public FancyButton(string text, float targetX, float targetY, int width, int height, float introDelay, int side, GUISkin iGUISkin)
	{
		Content = new GUIContent(text);
		Width = width;
		Height = height;
		this.introDelay = introDelay;	
		targetPosition = startPosition = new Vector2(targetX, targetY);
		Skin = iGUISkin;
	
		switch (side)
		{
			// Top
			case 0:
				startPosition.y = -Height;
				break;
			
			// Right
			case 1:
				startPosition.x = Screen.width;
				break;
			
			// Bottom
			case 2:
				startPosition.y = Screen.height;
				break;
			
			// Left
			case 3:
				startPosition.x = -Width;
				break;
		}
		
		Position = startPosition;
	}
	
	public FancyButton(GUIContent g, float targetX, float targetY, int width, int height, float introDelay, int side, GUISkin iGUISkin)
	{
		Content = g;
		Width = width;
		Height = height;
		this.introDelay = introDelay;	
		targetPosition = startPosition = new Vector2(targetX, targetY);
		Skin = iGUISkin;
	
		switch (side)
		{
			// Top
			case 0:
				startPosition.y = -Height;
				break;
			
			// Right
			case 1:
				startPosition.x = Screen.width;
				break;
			
			// Bottom
			case 2:
				startPosition.y = Screen.height;
				break;
			
			// Left
			case 3:
				startPosition.x = -Width;
				break;
		}
		
		Position = startPosition;
	}
	
	public FancyButton(GUIContent g, float targetX, float targetY, int width, int height, float introDelay, int side)
	{
		Content = g;
		Width = width;
		Height = height;
		this.introDelay = introDelay;	
		targetPosition = startPosition = new Vector2(targetX, targetY);
	
		switch (side)
		{
			// Top
			case 0:
				startPosition.y = -Height;
				break;
			
			// Right
			case 1:
				startPosition.x = Screen.width;
				break;
			
			// Bottom
			case 2:
				startPosition.y = Screen.height;
				break;
			
			// Left
			case 3:
				startPosition.x = -Width;
				break;
		}
		
		Position = startPosition;
	}
	
	private void RunMovement()
	{
		if (status != 0)
		{
			Position = new Vector2(Mathf.SmoothStep (startPosition.x, targetPosition.x, introTimer / introDelay), Mathf.SmoothStep (startPosition.y, targetPosition.y, introTimer / introDelay));
		}
	}
	
	public void Enter()
	{
		status = 1;	
	}
	
	public void Leave()
	{
		status = -1;	
	}
	
	public void Hide()
	{
		status = 0;
		Position = startPosition;
	}
	
	public void Unhide()
	{
		status = 0;
		Position = targetPosition;
	}
	
	public Rect GetRectangle()
	{
		return new Rect(Position.x, Position.y, Width, Height);
	}
	
	public void SetSkin(GUISkin skin)
	{
		Skin = skin;	
	}
	
	public void Update ()
	{
		switch(status)
		{
			// Leaving.
			case -1:
				if (introTimer < 0)
				{
					introTimer = 0;
					status = 0;
				}
				else
				{
					introTimer -= Time.deltaTime;
				}
				break;
			
			// Stationary.
			case 0:
				break;
			
			// Entering
			case 1:
				if (introTimer > introDelay)
				{
					introTimer = introDelay;
					status = 0;
				}
				else
				{
					introTimer += Time.deltaTime;
				}
				break;
		}
		
		RunMovement();
	}
}
