////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InAppPurchaseManager : EventDispatcher {
	
	public const string PRODUCT_BOUGHT 		= "product_bought";
	public const string TRANSACTION_FAILED 	= "transaction_failed";
	
	
	public bool isStoreLoaded = false;
	
	private List<string> _productsIds =  new List<string>();
	private List<ProductTemplate> _products    =  new List<ProductTemplate>();
	
	
	private static InAppPurchaseManager _instance;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public static InAppPurchaseManager instance {
		get {
			if(_instance ==  null) {
				GameObject go =  new GameObject("InAppPurchaseManager");
				DontDestroyOnLoad(go);
				_instance =  go.AddComponent<InAppPurchaseManager>();
			}
			
			return _instance;
		}
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public void loadStore() {
		string ids = "";
		int len = _productsIds.Count;
		for(int i = 0; i < len; i++) {
			if(i != 0) {
				ids += ",";
			}
			
			ids += _productsIds[i];
		}
		
		IOSNative.loadStore(ids);
		
	}
	
	public void buyProduct(string productId) {
		IOSNative.buyProduct(productId);
	}
	
	public void addProductId(string productId) {
		_productsIds.Add(productId);
	}
	
	public void restorePurchases() {
		IOSNative.restorePurchases();
	}
	
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	public void onStoreDataRecived(string data) {
		if(data.Equals(string.Empty)) {
			Debug.Log("InAppPurchaseManager, no products avaiable: " + _products.Count.ToString());
			return;
		}


		string[] storeData;
		storeData = data.Split("," [0]);
		
		for(int i = 0; i < storeData.Length; i+=5) {
			ProductTemplate tpl =  new ProductTemplate();
			tpl.id 				= storeData[i];
			tpl.title 			= storeData[i + 1];
			tpl.description 	= storeData[i + 2];
			tpl.localizedPrice 	= storeData[i + 3];
			tpl.price 			= storeData[i + 4];
			_products.Add(tpl);
		}
		
		Debug.Log("InAppPurchaseManager, tottal products loaded: " + _products.Count.ToString());
	}
	
	public void onProductBought(string productId) {
		dispatch(PRODUCT_BOUGHT, productId);
	}
	
	public void onTransactionFailed(string descr) {
		dispatch(TRANSACTION_FAILED, descr);
	}
	
	
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
