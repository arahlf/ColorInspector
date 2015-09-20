//
//  CrosshairImageView.m
//  ColorInspector
//
//  Created by Alan Rahlf on 8/16/15.
//  Copyright (c) 2015 Alan Rahlf. All rights reserved.
//

#import "CrosshairImageView.h"

@implementation CrosshairImageView

- (void)viewDidMoveToWindow {
    if (self.showCrosshairCursor) {
        [self.window setAcceptsMouseMovedEvents:YES];
    }
}

- (void)resetCursorRects {
    if (self.showCrosshairCursor) {
        NSCursor *crosshairCursor = [NSCursor crosshairCursor];
        
        [self addCursorRect:self.visibleRect cursor:crosshairCursor];
        [crosshairCursor setOnMouseEntered:YES];
    }
}

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

- (void)mouseDown:(NSEvent *)theEvent {
    if (!self.colorSelectionDelegate || !self.image) {
        return;
    }
    
    NSPoint point = [self convertPoint:[theEvent locationInWindow] fromView:nil];
    point.x = (NSInteger)point.x;
    point.y = (NSInteger)point.y;
    
    // need to convert the origin being on the bottom to the top to fetch the color
    point.y = ABS(point.y - self.frame.size.height);
    
    NSRect usableSpace = NSMakeRect(1, 1, self.image.size.width, self.image.size.height);
    
    if (!NSPointInRect(point, usableSpace)) {
        return;
    }
    
    NSBitmapImageRep *imageRep = [[NSBitmapImageRep alloc] initWithData:self.image.TIFFRepresentation];
    CGFloat scale = [NSScreen mainScreen].backingScaleFactor;
    
    NSColor *color = [imageRep colorAtX:((point.x - 1) * scale) y:((point.y - 1) * scale)];
    
    [self.colorSelectionDelegate onColorSelected:color];
}

@end
