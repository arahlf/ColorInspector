//
//  InspectButton.swift
//  ColorInspector
//
//  Created by Alan Rahlf on 11/1/15.
//  Copyright © 2015 Alan Rahlf. All rights reserved.
//

import Cocoa

protocol InspectButtonDelegate {
    func onMouseDown(inspectButton: InspectButton);
}

class InspectButton: NSButton {
    
    var delegate: InspectButtonDelegate?;

    override func mouseDown(theEvent: NSEvent) {
        // Specifically not calling super here because it prevents mouseUp from ever firing.
        
        self.highlighted = true;
        
        self.delegate?.onMouseDown(self);
    }
    
    override func mouseUp(theEvent: NSEvent) {
        self.highlighted = false;
    }
    
}
