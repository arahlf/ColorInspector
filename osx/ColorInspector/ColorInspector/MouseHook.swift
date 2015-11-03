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

func handleCGEvent(proxy: CGEventTapProxy, type: CGEventType, event: CGEvent, refcon: UnsafeMutablePointer<Void>) -> Unmanaged<CGEvent>? {
    let mouseHook: MouseHook = Unmanaged.fromOpaque(COpaquePointer(refcon)).takeUnretainedValue();
    
    if (type == .MouseMoved || type == .LeftMouseDragged) {
        mouseHook.delegate?.onMouseMove(CGEventGetLocation(event));
    }
    else if (type == .LeftMouseUp) {
        mouseHook.delegate?.onMouseUp(CGEventGetLocation(event));
    }
    else if (type == .TapDisabledByTimeout) {
        CGEventTapEnable(mouseHook.eventTap!, true);
    }
    
    return Unmanaged.passRetained(event);
}

@objc class MouseHook: NSObject {
    
    var delegate: MouseHookDelegate?;
    
    private var eventTap: CFMachPortRef?;

    override init() {
        super.init();
        
        self.setupMouseHook();
    }
    
    func setupMouseHook() {
        let refcon = UnsafeMutablePointer<Void>(Unmanaged.passUnretained(self).toOpaque());
        
        self.eventTap = CGEventTapCreate(.CGHIDEventTap, .HeadInsertEventTap, .ListenOnly, ~CGEventMask(0), handleCGEvent, refcon);
        let runLoopSource = CFMachPortCreateRunLoopSource(kCFAllocatorDefault, self.eventTap, 0);
        CFRunLoopAddSource(CFRunLoopGetMain(), runLoopSource, kCFRunLoopCommonModes);
    }
}
