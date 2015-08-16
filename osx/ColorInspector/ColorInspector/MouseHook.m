//
//  MouseHook.m
//  ColorInspector
//
//  Created by Alan Rahlf on 8/9/15.
//  Copyright (c) 2015 Alan Rahlf. All rights reserved.
//

#import "MouseHook.h"

@interface MouseHook()

@property (assign, nonatomic) CFMachPortRef eventTap;

@end

@implementation MouseHook

- (id)init {
    if (self = [super init]) {
        [self setupMouseHook];
    }
    return self;
}

CGEventRef handleCGEvent(CGEventTapProxy proxy, CGEventType type, CGEventRef eventRef, void *refcon)
{
    MouseHook *mouseHook = (__bridge MouseHook*)refcon;
    
    if (type == kCGEventMouseMoved) {
        [mouseHook.delegate onMouseMove:CGEventGetLocation(eventRef)];
    }
    else if (type == kCGEventLeftMouseUp) {
        [mouseHook.delegate onMouseUp:CGEventGetLocation(eventRef)];
    }
    else if (type == kCGEventTapDisabledByTimeout) {
        CGEventTapEnable(mouseHook.eventTap, true);
    }
    
    return eventRef;
}

- (void)setupMouseHook {
    self.eventTap = CGEventTapCreate(kCGHIDEventTap, kCGHeadInsertEventTap, kCGEventTapOptionListenOnly, kCGEventMaskForAllEvents, handleCGEvent, (__bridge void *)(self));
    CFRunLoopSourceRef runLoopSource = CFMachPortCreateRunLoopSource(kCFAllocatorDefault, self.eventTap, 0);
    CFRunLoopAddSource(CFRunLoopGetMain(), runLoopSource, kCFRunLoopCommonModes);
}

@end
