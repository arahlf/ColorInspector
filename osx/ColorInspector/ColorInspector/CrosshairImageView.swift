//
//  CrosshairImageView.swift
//  ColorInspector
//
//  Created by Alan Rahlf on 11/1/15.
//  Copyright © 2015 Alan Rahlf. All rights reserved.
//

import Cocoa

protocol ColorSelectionDelegate {
    func onColorSelected(color: NSColor);
}

class CrosshairImageView: NSImageView {
    
    var colorSelectionDelegate: ColorSelectionDelegate?;
    var showCrosshairCursor: Bool;
    
    required init?(coder: NSCoder) {
        self.showCrosshairCursor = false;
        
        super.init(coder: coder);
    }
    
    override func viewDidMoveToWindow() {
        if (self.showCrosshairCursor == true) {
            self.window?.acceptsMouseMovedEvents = true;
        }
    }
    
    override func resetCursorRects() {
        if (self.showCrosshairCursor == true) {
            let crosshairCursor = NSCursor.crosshairCursor();
            
            self.addCursorRect(self.visibleRect, cursor: crosshairCursor);
            crosshairCursor.setOnMouseEntered(true);
        }
    }

    override func drawRect(dirtyRect: NSRect) {
        super.drawRect(dirtyRect)

        if (self.image != nil) {
            let width: CGFloat = self.frame.size.width;
            let height: CGFloat = self.frame.size.height;
            
            let path = NSBezierPath();
            
            path.moveToPoint(NSMakePoint(width / 2, 0));
            path.lineToPoint(NSMakePoint(width / 2, height));
            
            path.moveToPoint(NSMakePoint(0, height / 2));
            path.lineToPoint(NSMakePoint(width, height / 2));
            
            path.stroke();
        }
    }
    
    override func mouseDown(theEvent: NSEvent) {
        if (self.colorSelectionDelegate == nil || self.image == nil) {
            return;
        }
        
        var point = self.convertPoint(theEvent.locationInWindow, fromView: nil);
        point.x = floor(point.x);
        point.y = floor(point.y);
        
        // need to convert the origin being on the bottom to the top to fetch the color
        point.y = abs(point.y - self.frame.size.height);
        
        let usableSpace = NSMakeRect(1, 1, self.image!.size.width, self.image!.size.height);
        
        if (!NSPointInRect(point, usableSpace)) {
            return;
        }
        
        let imageRep = NSBitmapImageRep(data: self.image!.TIFFRepresentation!);
        let scale = NSScreen.mainScreen()!.backingScaleFactor;
        
        let color = imageRep?.colorAtX(Int((point.x - 1) * scale), y: Int((point.y - 1) * scale));
        
        self.colorSelectionDelegate?.onColorSelected(color!);
    }
}
