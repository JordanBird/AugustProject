#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GameCenterManager.h"


@interface IOSGameCenterManager : NSObject <GKGameCenterControllerDelegate, GKAchievementViewControllerDelegate>{
    BOOL isAchievementsWasLoaded;
    UIView* curentView;
    NSString* requestedLeaderBordId;
    GameCenterManager *gameCenterManager;
}



- (void) reportScore: (int) score forCategory: (NSString*) category;

- (void) authenticateLocalPlayer;
- (void) showLeaderBoard: (NSString*)leaderBoradrId;

- (void) showAchievements;
- (void) resetAchievements; 
- (void) submitAchievement: (double) percentComplete identifier: (NSString*) identifier notifayComplete: (BOOL) notifayComplete;

- (BOOL)isGameCenterAvailable;

- (void)leaderboardViewControllerDidFinish:(GKLeaderboardViewController *)vc;


@end