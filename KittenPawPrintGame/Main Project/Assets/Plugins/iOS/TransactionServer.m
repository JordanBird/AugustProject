//
//  TransactionServer.m
//
//  Created by Osipov Stanislav on 1/16/13.
//

#import "TransactionServer.h"
#import "Unity3d.h"

@implementation TransactionServer

- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions {
    for (SKPaymentTransaction *transaction in transactions) {
        switch (transaction.transactionState) {
            case SKPaymentTransactionStatePurchased:
                [self completeTransaction:transaction];
                break;
            case SKPaymentTransactionStateFailed:
                [self failedTransaction:transaction];
                break;
            case SKPaymentTransactionStateRestored:
                [self restoreTransaction:transaction];
            default:
                break;
        }
    }
}


- (void)provideContent:(NSString *)productIdentifier {
    NSLog(@"provideContent for: %@", productIdentifier);
    UnitySendMessage("InAppPurchaseManager", "onProductBought", [Unity3d NSStringToChar:productIdentifier]);
    
}

- (void)recordTransaction:(SKPaymentTransaction *)transaction {
    // TODO: Record the transaction on the server side...
}

- (void)completeTransaction:(SKPaymentTransaction *)transaction {
    NSLog(@"completeTransaction...");
    
    [self recordTransaction: transaction];
    [self provideContent: transaction.payment.productIdentifier];
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];
    
}

- (void)restoreTransaction:(SKPaymentTransaction *)transaction {
    NSLog(@"restoreTransaction...");
    
    [self recordTransaction: transaction];
    [self provideContent: transaction.originalTransaction.payment.productIdentifier];
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];
    
}

- (void)failedTransaction:(SKPaymentTransaction *)transaction {
    NSLog(@"Transaction Failed with code : %i", transaction.error.code);
    NSLog(@"Transaction error: %@", transaction.error.localizedDescription);
    
    if(transaction.error.code != SKErrorPaymentCancelled) {
        
        UIAlertView *alert = [[UIAlertView alloc] init];
        [alert setTitle:@"Transaction Error"];
        [alert setMessage:transaction.error.localizedDescription];
        [alert addButtonWithTitle:@"Ok"];
        [alert setDelegate:NULL];
        [alert show];
        [alert release];
    }
    
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];
    UnitySendMessage("InAppPurchaseManager", "onTransactionFailed", [Unity3d NSStringToChar:transaction.error.localizedDescription]);
    
    
}


- (void)paymentQueue:(SKPaymentQueue *)queue restoreCompletedTransactionsFailedWithError:(NSError *)error {
    NSLog(@"%@",error);
}

- (void) paymentQueueRestoreCompletedTransactionsFinished:(SKPaymentQueue *)queue
{
    NSLog(@"received restored transactions: %i", queue.transactions.count);
    
    for (SKPaymentTransaction *transaction in queue.transactions)
    {
        NSString *productID = transaction.payment.productIdentifier;
        
        NSLog(@"%@",productID);
    }
}



@end
