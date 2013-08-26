////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaymnetManagerExample {
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public const string SMALL_PACK 	=  "your.in.app.purchase.id1";
	public const string NC_PACK 	=  "your.in.app.purchase.id2";



	public static void init() {
		InAppPurchaseManager.instance.addProductId(SMALL_PACK);
		InAppPurchaseManager.instance.addProductId(NC_PACK);
		
		InAppPurchaseManager.instance.addEventListener(InAppPurchaseManager.PRODUCT_BOUGHT, onProductBought);
		InAppPurchaseManager.instance.addEventListener(InAppPurchaseManager.TRANSACTION_FAILED, onTransactionFailed);

		InAppPurchaseManager.instance.loadStore();
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public static void buyItem(string productId) {
		InAppPurchaseManager.instance.buyProduct(productId);
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	private static void onProductBought(CEvent e) {
		
		string productId = (string) e.data;
		Debug.Log("STORE KIT GOT BUY: " + productId);
		
		IOSNative.showMessage("Success", "product " + productId + " is purchased");
	}
	
	private static void onTransactionFailed() {
		IOSNative.showMessage("Fail", "Transaction was failed");
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
