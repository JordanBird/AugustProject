////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopUpExamples : MonoBehaviour {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	void OnGUI() {
		if(GUI.Button(new Rect(10, 10, 150, 50), "Rate PopUp simple")) {
			IOSNative.showRateUsPopUP("Your title", "Your Message");
		}
		
		
		if(GUI.Button(new Rect(10, 70, 150, 50), "Rate PopUp with events")) {
			//IOSRateUsPopUp rate = IOSRateUsPopUp.Create();
			IOSRateUsPopUp rate = IOSRateUsPopUp.Create("Like this game?", "Please rate to support future updates!");


			rate.addEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
		}
		
		
		if(GUI.Button(new Rect(10, 130, 150, 50), "Dialog PopUp")) {
			IOSDialog dialog = IOSDialog.Create("Dialog Titile", "Dialog message");
			dialog.addEventListener(BaseEvent.COMPLETE, onDialogClose);
		}
		
		if(GUI.Button(new Rect(10, 190, 150, 50), "Message PopUp")) {
			IOSMessage msg = IOSMessage.Create("Message Titile", "Message message");
			msg.addEventListener(BaseEvent.COMPLETE, onMessageClose);
		}
		
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	private void onRatePopUpClose(CEvent e) {
		(e.dispatcher as IOSRateUsPopUp).removeEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
		string result = e.data.ToString();
		IOSNative.showMessage("Result", result + " button pressed");
	}
	
	private void onDialogClose(CEvent e) {

		//romoving listner
		(e.dispatcher as IOSDialog).removeEventListener(BaseEvent.COMPLETE, onDialogClose);

		//parsing result
		switch((IOSDialogResult)e.data) {
		case IOSDialogResult.YES:
			Debug.Log ("Yes button pressed");
			break;
		case IOSDialogResult.NO:
			Debug.Log ("Yes button pressed");
			break;

		}

		string result = e.data.ToString();
		IOSNative.showMessage("Result", result + " button pressed");
	}
	
	private void onMessageClose(CEvent e) {
		(e.dispatcher as IOSMessage).removeEventListener(BaseEvent.COMPLETE,  onMessageClose);
		IOSNative.showMessage("Result", "Message Closed");
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
