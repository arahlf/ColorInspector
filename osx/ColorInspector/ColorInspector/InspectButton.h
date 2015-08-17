//
//  InspectButton.h
//  ColorInspector
//
//  Created by Alan Rahlf on 8/16/15.
//  Copyright (c) 2015 Alan Rahlf. All rights reserved.
//

#import <Cocoa/Cocoa.h>

@class InspectButton;

@protocol InspectButtonDelegate <NSObject>

- (void)onMouseDown:(InspectButton *)inspectButton;

@end

@interface InspectButton : NSButton

@property (weak, nonatomic) id<InspectButtonDelegate> delegate;

@end
