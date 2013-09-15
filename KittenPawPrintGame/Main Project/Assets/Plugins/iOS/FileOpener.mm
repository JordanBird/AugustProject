//
//  FileOpener.mm
//  Unity-iPhone
//
//  Created by Joshua O'Rourke on 15/09/2013.
//
//

#import "FileOpener.h"

@implementation FileOpener

@synthesize dic;

NSString* CreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

+ (FileOpener*)singleton
{
	static FileOpener* singleton = nil;
	
	if(singleton == nil)
	{
		singleton = [[FileOpener alloc] init];
	}
	
	return singleton;
}

- (void)openFile:(NSURL*)file fromRect:(CGRect)rect
{
	if(![[NSFileManager defaultManager] fileExistsAtPath:file.path])
	{
		NSLog(@"File does not exist at %@", file.path);
	}
	
	self.dic = [UIDocumentInteractionController interactionControllerWithURL:file];
	self.dic.delegate = self;
	
	BOOL success = [self.dic presentOpenInMenuFromRect:rect
										   inView:[[[UIApplication sharedApplication] keyWindow] subviews][0]
										 animated:YES];
	
	if(!success)
	{
		[[[UIAlertView alloc] initWithTitle:@"Cannot Open File" message:@"There are no Apps installed on your device which can open this file" delegate:nil cancelButtonTitle:@"Dismiss" otherButtonTitles: nil] show];
	}
}

- (void) documentInteractionController:(UIDocumentInteractionController *)controller willBeginSendingToApplication:(NSString *)application
{
	NSLog(@"Sending file to %@", application);
}

- (void) documentInteractionController:(UIDocumentInteractionController *)controller didEndSendingToApplication:(NSString *)application
{
	NSLog(@"File sent to %@", application);
}

- (void) documentInteractionControllerDidDismissOpenInMenu: (UIDocumentInteractionController *) controller
{
	//self.dic = nil;
}

extern "C"
{
	void _OpenFile (int buttonX, int buttonY, const char* uri)
	{
		NSURL *URI = [NSURL fileURLWithPath:CreateNSString(uri)];
		
		[[FileOpener singleton] openFile:URI fromRect:CGRectMake(buttonX, buttonY, 1, 1)];
	}
}

@end
