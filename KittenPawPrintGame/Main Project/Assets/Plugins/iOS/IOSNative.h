//
//  IOSNative.h
//
//  Created by Osipov Stanislav on 1/11/13.
//
//

#import <Foundation/Foundation.h>

@interface IOSNative : NSObject

+ (void) setAppId:(NSString*)appId;
+ (NSString *) getAppId;
- (void)showRateUsPopUp: (NSString *) title message: (NSString*) msg;

@end
