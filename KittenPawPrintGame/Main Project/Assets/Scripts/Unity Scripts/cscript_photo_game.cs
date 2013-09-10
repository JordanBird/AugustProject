using UnityEngine;
using System;
using System.Collections;

public class cscript_photo_game : MonoBehaviour {
	
	public bool playing = false;
	Game game;
	cscript_master master;
	cscript_sound_master soundMaster;
	
	//-----------------------\\
	//iOS Variables
	//-----------------------\\
	public Material targetMaterial = null;
	public bool useOriginalImageSize = false;
	public bool iPadPopover_CloseWhenSelectImage = true;
	private int textureWidth;
	private int textureHeight;
	private bool saveAsPng = false;
	private bool imageInputChoiceMenu = false;
	private bool imageAnswerChoiceMenu = false;
	
	JGUI jG = new JGUI();
	
	Texture2D background;
	
	Texture2D[] photoGroup;
	int GroupCounter;
	
	Question currentQuestion;
	
	cscript_GUI_master GUIMaster;
	
	//Player Variables
	int score = 0;
	
	// Use this for initialization
	void Start () 
	{
		background = game.background;
		
		GUIMaster = GameObject.FindGameObjectWithTag ("GUI Master").GetComponent<cscript_GUI_master>();
		soundMaster = GameObject.FindGameObjectWithTag ("Sound Master").GetComponent<cscript_sound_master>();
		
		photoGroup = new Texture2D[6];
		
		for(int i = 0; i < 6; i++)
		{
		photoGroup[i] = GUIMaster.trainIcon;
		}
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnGUI()
	{
		if (playing == true)
		{
			//GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), background);
			
			//Banners
			GUI.DrawTexture (new Rect(0, 0, Screen.width, 64), GUIMaster.bannerTexture);
			GUI.DrawTexture (new Rect(0, Screen.height - 100, Screen.width, 100), GUIMaster.bannerTexture);
			
			if (GUI.Button (new Rect(10, 10, 100, 30), "Quit"))
			{
				master.gameState = cscript_master.GameState.MainMenu;
				master.gameObject.GetComponent<cscript_navigation>().MainMenuLoad ();
				Destroy(this.gameObject);
			}

			//GUI.Label (new Rect(10, Screen.height - 100, Screen.width - 20, 50), "Score: " + score, GUIMaster.scores.label);
			GUI.Label (new Rect(10, -20, Screen.width - 20, 100), currentQuestion.text, GUIMaster.questions.label);

			
			if (GUI.Button(new Rect(10, Screen.height - 100, Screen.width - 20, 50), "Save To Camera Roll", GUIMaster.scores.label))
			{
				soundMaster.PlaySound (soundMaster.correctAnswer);
				StartCoroutine("CaptureScreen");
			}
			
			int inc = 0;
			
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if(GUI.Button(new Rect(40 + (j * Screen.width / 3), 78 + (i * Screen.height/2.8f), Screen.width / (4), (Screen.height / 3)),photoGroup[inc]))
					{
						imageAnswerChoiceMenu = true;
						GroupCounter = inc;
					}
					inc ++;
				}
			}
			
			//Open Answer Image Popup:
			//Gets the background from camera roll or camera.
			if (Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				if(imageAnswerChoiceMenu)
				{
					Debug.Log(jG.MessageBox(0, "Add Background", new string[] {"Use Camera", "Use Camera Roll"}));
					switch (jG.MessageBox(0, "Add Background", new string[] {"Use Camera", "Use Camera Roll"}))
					{
						case 0:
						Debug.Log ("Case0");
							imageAnswerChoiceMenu = false;
							//Directly From Camera:
							LoadTextureFromImagePicker.SetPopoverAutoClose(iPadPopover_CloseWhenSelectImage);
							LoadTextureFromImagePicker.SetPopoverToCenter();
							LoadTextureFromImagePicker.ShowCamera(gameObject.name, "OnFinishedImagePicker");
						
							imageInputChoiceMenu = false;
							break;
						case 1:
						Debug.Log ("Case1");
							imageAnswerChoiceMenu = false;
							//From Camera Roll:
							LoadTextureFromImagePicker.SetPopoverAutoClose(iPadPopover_CloseWhenSelectImage);
							LoadTextureFromImagePicker.SetPopoverTargetRect((Screen.width / 4 - 10) / 2, 100, Screen.width / 4,20);
							LoadTextureFromImagePicker.ShowPhotoLibrary(gameObject.name, "OnFinishedImagePicker");
	
						break;
					}
				}
			}
			
			
		}
	}
	
	private void NewQuestion()
	{
		currentQuestion = game.GetQuestion ();
		currentQuestion.RandomizeAnswers ();
	}
	
	public void Begin(Game g, cscript_master m)
	{
		playing = true;
		game = g;
		master = m;
		
		try
		{
			background = game.background;
		}
		catch {}
		
		NewQuestion ();
	}
	
	public void Stop()
	{
		playing = false;
		Destroy (this.gameObject);
	}
	
	//-----------------------\\
	//iOS Specific Functions
	//-----------------------\\
	#region iOS Functions
	private void OnFinishedImagePicker (string message)
	{
	
		if (LoadTextureFromImagePicker.IsLoaded())
		{
			int width, height;
			
			if (useOriginalImageSize || (targetMaterial == null))
			{
				width = LoadTextureFromImagePicker.GetLoadedTextureWidth();
				height = LoadTextureFromImagePicker.GetLoadedTextureHeight();
			}
			else
			{
				width = textureWidth;
				height = textureHeight;
			}
			
			Texture2D texture = LoadTextureFromImagePicker.GetLoadedTexture(message, width, height);
			
			//IT GETS HERE
			if (texture)
			{
				// Loaded
				//if (targetMaterial) {
					//BUT NOT HERE
					photoGroup[GroupCounter] = texture;
					//Texture lastTexture = targetMaterial.mainTexture;
					//targetMaterial.mainTexture = texture;
					//Destroy(lastTexture);
				//}
				jG.ResetMessageBox();
			}
			else
			{
				// Closed
				LoadTextureFromImagePicker.Release();
			}
		}
		else
		{
			// Closed
			LoadTextureFromImagePicker.Release();
		}
	}
	

	private IEnumerator CaptureScreen()
	{
		
		yield return new WaitForEndOfFrame();

		// Save to PhotoLibrary
		Texture screenShot = ScreenCapture.Capture();
		if (saveAsPng)
		{
			bool withTransparency = false;
			if (withTransparency)
			{
				// PNG with transparency
				LoadTextureFromImagePicker.SaveAsPngWithTransparencyToPhotoLibrary(screenShot, gameObject.name, "OnFinishedSaveImage");
			}
			else
			{
				// PNG
				LoadTextureFromImagePicker.SaveAsPngToPhotoLibrary(screenShot, gameObject.name, "OnFinishedSaveImage");
			}
		}
		else
		{
			// JPG
			LoadTextureFromImagePicker.SaveAsJpgToPhotoLibrary(screenShot, gameObject.name, "OnFinishedSaveImage");
		}
	}

	private void OnFinishedSaveImage (string message)
	{
		if (message == LoadTextureFromImagePicker.strCallbackResultMessage_Saved)
		{
			// Save Succeed
		}
		else 
		{
			// Failed
		}
	}
	#endregion
	
	
}
