using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class FileOpener {

	/* Interface to native implementation */
	
	[DllImport ("__Internal")] private static extern void _OpenFile (int buttonX, int buttonY, string uri);
	// Starts lookup for some bonjour registered service inside specified domain
	public static void OpenFile (int buttonX, int buttonY, string uri)
	{
		// Call plugin only when running on real device
		if (Application.platform != RuntimePlatform.OSXEditor)
			_OpenFile(buttonX, buttonY, uri);
	}
}
