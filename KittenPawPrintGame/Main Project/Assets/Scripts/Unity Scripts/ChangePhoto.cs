using UnityEngine;
using System.Collections;

public class ChangePhoto : MonoBehaviour {
	
	public Material targetMaterial = null;
	public bool useOriginalImageSize = false;
	public bool iPadPopover_CloseWhenSelectImage = false;

	private int textureWidth;
	private int textureHeight;
	private bool saveAsPng = true;

	private string lastMessage = "";

	void Start () {
		targetMaterial = GameObject.Find("Cube").renderer.material;
		textureWidth = targetMaterial.mainTexture.width;
		textureHeight = targetMaterial.mainTexture.height;
	}

	void Update () {
	}

	void OnGUI () {
		// Swithes
		GUI.Label(new Rect(0,Screen.height*0.5f, 100,30), "Options:");
		useOriginalImageSize = GUI.Toggle(new Rect(0, Screen.height*0.5f+30, 400, 30), useOriginalImageSize, "useOriginalImageSize");
		iPadPopover_CloseWhenSelectImage = GUI.Toggle(new Rect(0, Screen.height*0.5f+60, 400, 30), iPadPopover_CloseWhenSelectImage, "iPadPopover_CloseWhenSelectImage");

		// Buttons
		float buttonWidth = Screen.width/3;
		float buttonHeight = Screen.height/5;
		float buttonMargine = buttonWidth/3;
		Rect buttonRect = new Rect(0, Screen.height-buttonHeight, buttonWidth, buttonHeight);
		buttonRect.x = buttonMargine;
		if (GUI.Button(buttonRect, "Camera\n(iOS ONLY)")) {
			#if UNITY_IPHONE
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					LoadTextureFromImagePicker.SetPopoverToCenter();
					LoadTextureFromImagePicker.ShowCamera(gameObject.name, "OnFinishedImagePicker");
				}
			#endif
		}
		buttonRect.x = buttonMargine + buttonWidth + buttonMargine;
		if (GUI.Button(buttonRect, "Load Image\nfrom PhotoLibrary\n(iOS ONLY)")) {
			#if UNITY_IPHONE
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					LoadTextureFromImagePicker.SetPopoverAutoClose(iPadPopover_CloseWhenSelectImage);
					LoadTextureFromImagePicker.SetPopoverTargetRect(buttonRect.x, buttonRect.y, buttonWidth, buttonHeight);
					LoadTextureFromImagePicker.ShowPhotoLibrary(gameObject.name, "OnFinishedImagePicker");
				}
			#endif
		}
		//
		// for Save Image
		buttonRect.width = Screen.width/4;
		buttonRect.height = Screen.height/6;
		buttonMargine = 0;
		buttonRect.y = 0;
		buttonRect.x = buttonMargine + (buttonRect.width + buttonMargine) * 1;
		if (GUI.Button(buttonRect, "Save JPG\nto PhotoLibrary\n(iOS ONLY)")) {
			#if UNITY_IPHONE
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					saveAsPng = false;
					StartCoroutine("CaptureScreen");
				}
			#endif
		}
		buttonRect.x = buttonMargine + (buttonRect.width + buttonMargine) * 2;
		if (GUI.Button(buttonRect, "Save PNG\nto PhotoLibrary\n(iOS ONLY)")) {
			#if UNITY_IPHONE
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					saveAsPng = true;
					StartCoroutine("CaptureScreen");
				}
			#endif
		}

		// Disp Texture Size
		if (targetMaterial) {
			Texture targetTexture = targetMaterial.mainTexture;
			GUI.Label(new Rect(0,0, 400,100), "targetTexture:\n"+"width="+targetTexture.width+", height="+targetTexture.height);
		}

		// Disp Last Message
		GUI.Label(new Rect(0,80, 200,60), "Last Result:\n"+lastMessage);
	}

	#if UNITY_IPHONE
	private void OnFinishedImagePicker (string message) {
		lastMessage = message;
		if (LoadTextureFromImagePicker.IsLoaded()) {
			int width, height;
			if (useOriginalImageSize || (targetMaterial == null)) {
				width = LoadTextureFromImagePicker.GetLoadedTextureWidth();
				height = LoadTextureFromImagePicker.GetLoadedTextureHeight();
			} else {
				width = textureWidth;
				height = textureHeight;
			}
			Texture2D texture = LoadTextureFromImagePicker.GetLoadedTexture(message, width, height);
			if (texture) {
				// Loaded
				if (targetMaterial) {
					Texture lastTexture = targetMaterial.mainTexture;
					targetMaterial.mainTexture = texture;
					Destroy(lastTexture);
				}
			} else {
				// Closed
				LoadTextureFromImagePicker.Release();
			}
		} else {
			// Closed
			LoadTextureFromImagePicker.Release();
		}
	}

	private IEnumerator CaptureScreen() {
		yield return new WaitForEndOfFrame();

		// Save to PhotoLibrary
		Texture screenShot = ScreenCapture.Capture();
		if (saveAsPng) {
			bool withTransparency = false;
			if (withTransparency) {
				// PNG with transparency
				LoadTextureFromImagePicker.SaveAsPngWithTransparencyToPhotoLibrary(screenShot, gameObject.name, "OnFinishedSaveImage");
			} else {
				// PNG
				LoadTextureFromImagePicker.SaveAsPngToPhotoLibrary(screenShot, gameObject.name, "OnFinishedSaveImage");
			}
		} else {
			// JPG
			LoadTextureFromImagePicker.SaveAsJpgToPhotoLibrary(screenShot, gameObject.name, "OnFinishedSaveImage");
		}
	}

	private void OnFinishedSaveImage (string message) {
		lastMessage = message;
		if (message == LoadTextureFromImagePicker.strCallbackResultMessage_Saved) {
			// Save Succeed
		} else {
			// Failed
		}
	}
	#endif
}
