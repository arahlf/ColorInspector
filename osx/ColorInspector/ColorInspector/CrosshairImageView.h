//
//  CrosshairImageView.h
//  ColorInspector
//
//  Created by Alan Rahlf on 8/16/15.
//  Copyright (c) 2015 Alan Rahlf. All rights reserved.
//

#import <Cocoa/Cocoa.h>

@protocol ColorSelectionDelegate <NSObject>

- (void)onColorSelected:(NSColor *)color;

@end


@interface CrosshairImageView : NSImageView

@property (weak, nonatomic) id<ColorSelectionDelegate> colorSelectionDelegate;

@property (assign, nonatomic) BOOL showCrosshairCursor;

@end
