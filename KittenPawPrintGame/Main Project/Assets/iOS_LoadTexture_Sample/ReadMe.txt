Unity iOS native plugin

iOS Load Texture From UIImagePicker


---------------------------------------------------------------------------
Description: 
This plugin is run on iOS native code. You can use UIImagePicker for load a texture on Unity.

Features:
- Load Texture from PhotoLibrary on Device
- Take a photo by Camera
- Save Image to PhotoLibrary (JPG/PNG/PNG with transparency)
- Support for Unity 3.5.7/4.0/4.1/4.2


Demo Video:
http://youtu.be/HeL85WW0Y80


How to use (C# Script):

ShowCamera:
	LoadTextureFromImagePicker.ShowCamera(gameObject.name, "OnFinishedImagePicker");

Load Image from PhotoLibrary:
	LoadTextureFromImagePicker.ShowPhotoLibrary(gameObject.name, "OnFinishedImagePicker");

Callback function on finished, and get Image:
	private void OnFinishedImagePicker (string message) {
		Texture lastTexture = gameObject.renderer.material.mainTexture;
		int texWidth = lastTexture.width;
		int texHeight = lastTexture.height;
		Texture2D texture = LoadTextureFromImagePicker.GetLoadedTexture(message, texWidth, texHeight);
		if (texture) {
			Texture lastTexture = targetMaterial.mainTexture;
			targetMaterial.mainTexture = texture;
			Destroy(lastTexture);
		}
	}


Popover position setting for iPad:
	Center of screen
		LoadTextureFromImagePicker.SetPopoverToCenter();

	Set target Rect
		LoadTextureFromImagePicker.SetPopoverTargetRect(buttonPos.x, buttonPos.y, buttonWidth, buttonHeight);


Save Image to PhotoLibrary:
	LoadTextureFromImagePicker.SaveAsJpgToPhotoLibrary(image, gameObject.name, "OnFinishedSaveImage");
	LoadTextureFromImagePicker.SaveAsPngToPhotoLibrary(image, gameObject.name, "OnFinishedSaveImage");
	LoadTextureFromImagePicker.SaveAsPngWithTransparencyToPhotoLibrary(image, gameObject.name, "OnFinishedSaveImage");


If you have any question, send email to support: whitedev.support@gmail.com


---------------------------------------------------------------------------
Version Changes:
1.2:
	- Add "Save to PhotoLibrary" Function (JPG/PNG/PNG with transparency)
	- Fix memory leak
1.1:
	- Support for Unity 4.2
1.0.4:
	- Support for load image with original size
	- Support for no close menu on select image (only iPad)
	- Fixed conflict my plugins.
1.0.3:
	- Support for Unity 3.5.7/4.0/4.1 or Higher
1.0.2:
	- Fix Popover Positioning
1.0:
	- Initial version.
