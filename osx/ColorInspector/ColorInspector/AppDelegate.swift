//
//  AppDelegate2.swift
//  ColorInspector
//
//  Created by Alan Rahlf on 11/2/15.
//  Copyright © 2015 Alan Rahlf. All rights reserved.
//

import Cocoa

@NSApplicationMain
class AppDelegate: NSObject, NSApplicationDelegate {

    func applicationShouldTerminateAfterLastWindowClosed(sender: NSApplication) -> Bool {
        return true;
    }
}
