//
//  MouseHook.swift
//  ColorInspector
//
//  Created by Alan Rahlf on 11/1/15.
//  Copyright © 2015 Alan Rahlf. All rights reserved.
//

import Cocoa

@objc protocol MouseHookDelegate {
    func onMouseMove(point: CGPoint);
    func onMouseUp(point: CGPoint);
}

// Haven't been able to figure out how to pass in a value for the refcon parameter of CGEventTapCreate
// yet so the workaround for now is to just store off a singleton-ish reference to it (feelsbadman)
private var instance: MouseHook?;

func handleCGEvent(proxy: CGEventTapProxy, type: CGEventType, event: CGEvent, refcon: UnsafeMutablePointer<Void>) -> Unmanaged<CGEvent>? {
    if (type == .MouseMoved || type == .LeftMouseDragged) {
        instance!.delegate?.onMouseMove(CGEventGetLocation(event));
    }
    else if (type == .LeftMouseUp) {
        instance!.delegate?.onMouseUp(CGEventGetLocation(event));
    }
    else if (type == .TapDisabledByTimeout) {
        CGEventTapEnable(instance!.eventTap!, true);
    }
    
    return Unmanaged.passRetained(event);
}

@objc class MouseHook: NSObject {
    
    var delegate: MouseHookDelegate?;
    
    private var eventTap: CFMachPortRef?;

    override init() {
        super.init();
        
        instance = self;
        
        self.setupMouseHook();
    }
    
    func setupMouseHook() {
        self.eventTap = CGEventTapCreate(.CGHIDEventTap, .HeadInsertEventTap, .ListenOnly, ~CGEventMask(0), handleCGEvent, nil);
        let runLoopSource = CFMachPortCreateRunLoopSource(kCFAllocatorDefault, self.eventTap, 0);
        CFRunLoopAddSource(CFRunLoopGetMain(), runLoopSource, kCFRunLoopCommonModes);
    }
}
