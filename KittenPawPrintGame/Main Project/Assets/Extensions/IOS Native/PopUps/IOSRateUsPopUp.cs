////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IOSRateUsPopUp : BaseIOSPopup {
	
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public static IOSRateUsPopUp Create() {
		return Create("Like the Game?", "Rate US");
	}
	
	public static IOSRateUsPopUp Create(string title, string message) {
		IOSRateUsPopUp rate = new GameObject("IOSRateUsPopUp").AddComponent<IOSRateUsPopUp>();
		rate.title = title;
		rate.message = message;
		rate.init();
			
		return rate;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public void init() {
		IOSNative.showRateUsPopUP(title, message);
	}
	
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	public void onPopUpCallBack(string buttonIndex) {
		int index = System.Convert.ToInt16(buttonIndex);
		switch(index) {
			case 0: 
				dispatch(BaseEvent.COMPLETE, IOSDialogResult.RATED);
				break;
			case 1:
				dispatch(BaseEvent.COMPLETE, IOSDialogResult.REMIND);
				break;
			case 2:
				dispatch(BaseEvent.COMPLETE, IOSDialogResult.DECLINED);
				break;
		}
		
		
		
		Destroy(gameObject);
	} 
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
