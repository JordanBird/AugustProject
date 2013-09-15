//
//  FileOpener.h
//  Unity-iPhone
//
//  Created by Joshua O'Rourke on 15/09/2013.
//
//

#import <Foundation/Foundation.h>

@interface FileOpener : NSObject <UIDocumentInteractionControllerDelegate>
{
	UIDocumentInteractionController *dic;
}

@property(nonatomic, retain) UIDocumentInteractionController *dic;

+ (FileOpener*)singleton;
- (void)openFile:(NSURL*)file fromRect:(CGRect)rect;

@end
