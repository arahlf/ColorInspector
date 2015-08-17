//
//  CrosshairImageView.m
//  ColorInspector
//
//  Created by Alan Rahlf on 8/16/15.
//  Copyright (c) 2015 Alan Rahlf. All rights reserved.
//

#import "CrosshairImageView.h"

@implementation CrosshairImageView

- (void)drawRect:(NSRect)dirtyRect {
    [super drawRect:dirtyRect];
    
    if (self.image) {
        CGFloat width = self.frame.size.width;
        CGFloat height = self.frame.size.height;
        
        NSBezierPath *path = [NSBezierPath bezierPath];
        
        [path moveToPoint:NSMakePoint(width / 2, 0)];
        [path lineToPoint:NSMakePoint(width / 2, height)];
        
        [path moveToPoint:NSMakePoint(0, height / 2)];
        [path lineToPoint:NSMakePoint(width, height / 2)];
        
        [path stroke];
    }
}

@end
