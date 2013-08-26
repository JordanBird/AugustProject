////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class IOSNative : EventDispatcher {



	public const string APPLICATION_DID_ENTER_BACKGROUND 		= "applicationDidEnterBackground";
	public const string APPLICATION_DID_BECOME_ACTIVE 			= "applicationDidBecomeActive";
	public const string APPLICATION_DID_RECEIVE_MEMORY_WARNING 	= "applicationDidReceiveMemoryWarning";
	public const string APPLICATION_WILL_RESIGN_ACTIVE 	        = "applicationWillResignActive";
	public const string APPLICATION_WILL_TERMINATE 	            = "applicationWillTerminate";
	

	//Your application id from itunes
	public string appId;
	
	[DllImport ("__Internal")]
	private static extern void _initIOSNative(string appId);
	
	
    //--------------------------------------
	//  NATIVE FUNCTIONS
	//--------------------------------------
	
	
	[DllImport ("__Internal")]
	private static extern void _showRateUsPopUp(string title, string message);
	
	[DllImport ("__Internal")]
	private static extern void _showDialog(string title, string message, string yes, string no);
	
	[DllImport ("__Internal")]
	private static extern void _showMessage(string title, string message, string ok);
	
	
    //--------------------------------------
	//  MARKET
	//--------------------------------------
	
	[DllImport ("__Internal")]
	private static extern void _loadStore(string ids);
	
	[DllImport ("__Internal")]
	private static extern void _restorePurchases();
	
	
	[DllImport ("__Internal")]
	private static extern void _buyProduct(string id);

	
	private static IOSNative _instance = null;
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	void Awake() {

		gameObject.name = "IOSNative";

		if(appId == string.Empty) {
			Debug.LogError ("App ID is empty");
		}

		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			_initIOSNative(appId);
		}

		_instance = this;
		DontDestroyOnLoad(gameObject);
	}
	
	public void Create() {
		//fake function to initialize library
		//IOSNative.instance.Create();
	}
	
	public static IOSNative instance {
		get {
			if(_instance == null) {
				GameObject go =  new GameObject("IOSNative");
				_instance =  go.AddComponent<IOSNative>();
			}
			
			return _instance;
		}
	}


	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	
	public static void showRateUsPopUP(string title, string message) {
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			_showRateUsPopUp(title, message);
		}	
	}
	
	
	public static void showDialog(string title, string message) {
		showDialog(title, message, "Yes", "No");
	}
	
	public static void showDialog(string title, string message, string yes, string no) {
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			_showDialog(title, message, yes, no);
		}	
	}
	
	
	public static void showMessage(string title, string message) {
		showMessage(title, message, "Ok");
	}
	
	public static void showMessage(string title, string message, string ok) {
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			_showMessage(title, message, ok);
		}	
	}
	
	public static void loadStore(string ids) {
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			_loadStore(ids);
		}
	}
	
	public static void buyProduct(string id) {
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			_buyProduct(id);
		}
	}
	
	public static void restorePurchases() {
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			_restorePurchases();
		}
	}
	
	
	
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------


	private void applicationDidEnterBackground() {
		dispatch(APPLICATION_DID_ENTER_BACKGROUND);
	}

	private void applicationDidBecomeActive() {
		dispatch(APPLICATION_DID_BECOME_ACTIVE);
	}

	private void applicationDidReceiveMemoryWarning() {
		dispatch(APPLICATION_DID_RECEIVE_MEMORY_WARNING);
	}


	private void applicationWillResignActive() {
		dispatch (APPLICATION_WILL_RESIGN_ACTIVE);
	}


	private void applicationWillTerminate() {
		dispatch (APPLICATION_WILL_TERMINATE);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
