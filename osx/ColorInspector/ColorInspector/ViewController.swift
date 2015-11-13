//
//  ViewController2.swift
//  ColorInspector
//
//  Created by Alan Rahlf on 11/3/15.
//  Copyright © 2015 Alan Rahlf. All rights reserved.
//

import Cocoa

class ViewController: NSViewController, InspectButtonDelegate, MouseHookDelegate, ColorSelectionDelegate {
    
    private var mouseHook: MouseHook?;
    private var scanning: Bool = false;
    private var colorSpace: CGColorSpaceRef?;
    
    @IBOutlet weak var mouseLocationField: NSTextField!
    @IBOutlet weak var rgbField: NSTextField!
    @IBOutlet weak var hexField: NSTextField!
    @IBOutlet weak var scanView: CrosshairImageView!
    @IBOutlet weak var zoomView: CrosshairImageView!
    @IBOutlet weak var colorView: NSImageView!
    @IBOutlet weak var inspectButton: InspectButton!

    override func viewDidLoad() {
        super.viewDidLoad();
        
        self.inspectButton.delegate = self;
        self.zoomView.colorSelectionDelegate = self;
        
        self.mouseHook = MouseHook();
        self.mouseHook?.delegate = self;
        
        self.colorSpace = CGColorSpaceCreateWithName(kCGColorSpaceGenericRGB);
        
        self.addBorderToImageView(self.scanView);
        self.addBorderToImageView(self.zoomView);
        self.addBorderToImageView(self.colorView);
    }
    
    func onMouseDown(inspectButton: InspectButton) {
        self.scanning = true;
    }
    
    func onMouseMove(point: CGPoint) {
        self.mouseLocationField.stringValue = String(format: "Mouse Location: %d, %d", Int(point.x), Int(point.y));
        
        if (self.scanning) {
            self.updateScanView(point);
        }
    }
    
    func onMouseUp(point: CGPoint) {
        if (self.scanning) {
            self.scanning = false;
        }
    }
    
    func addBorderToImageView(imageView: NSImageView) {
        imageView.wantsLayer = true;
        imageView.layer?.borderWidth = 1;
        imageView.layer?.borderColor = NSColor.blackColor().CGColor;
    }
    
    func updateScanView(point: CGPoint) {
        let size = 81;
        let halfSize = size / 2;
        let xLoc = Int(point.x);
        let yLoc = Int(point.y);
        
        let wide = Int(CGDisplayPixelsWide(CGMainDisplayID()) - 1);
        let high = Int(CGDisplayPixelsHigh(CGMainDisplayID()) - 1);
        
        let frameX = max(0, min(wide - size, xLoc - halfSize));
        let frameY = max(0, min(high - size, yLoc - halfSize));
        
        let rect = CGRectMake(CGFloat(frameX), CGFloat(frameY), CGFloat(size), CGFloat(size));
        
        let imageRef = CGDisplayCreateImageForRect(CGMainDisplayID(), rect);
        
        let scale = Int(NSScreen.mainScreen()!.backingScaleFactor);
        
        let scanContext = CGBitmapContextCreate(nil, size * scale, size * scale, 8, 0, self.colorSpace, CGImageAlphaInfo.NoneSkipLast.rawValue);
        let zoomContext = CGBitmapContextCreate(nil, size * scale, size * scale, 8, 0, self.colorSpace, CGImageAlphaInfo.NoneSkipLast.rawValue);
        
        var adjustedX = 0;
        var adjustedY = 0;
        
        if (xLoc < halfSize) {
            adjustedX = (halfSize - xLoc) * scale;
        }
        
        if (yLoc < halfSize) {
            adjustedY = -((halfSize - yLoc) * scale);
        }
        
        if (xLoc > (wide - halfSize)) {
            adjustedX = -((xLoc - (wide - halfSize)) * scale);
        }
        
        if (yLoc > (high - halfSize)) {
            adjustedY = (yLoc - (high - halfSize)) * scale;
        }
        
        CGContextDrawImage(scanContext, CGRectMake(CGFloat(adjustedX), CGFloat(adjustedY), rect.size.width * CGFloat(scale), rect.size.height * CGFloat(scale)), imageRef);
        
        let scanImageRef = CGBitmapContextCreateImage(scanContext)!;
        
        let zoomImageRect = CGRectMake(38 * CGFloat(scale), 38 * CGFloat(scale), 9, 9);
        
        let zoomedImage = CGImageCreateWithImageInRect(scanImageRef, zoomImageRect);
        
        CGContextSetInterpolationQuality(zoomContext, CGInterpolationQuality.None);
        
        CGContextDrawImage(zoomContext, CGRectMake(0, 0, CGFloat(size * scale), CGFloat(size * scale)), zoomedImage);
        
        let zoomImageRef = CGBitmapContextCreateImage(zoomContext)!;
        
        let image = NSImage(CGImage: scanImageRef, size: CGSizeMake(CGFloat(size), CGFloat(size)));
        let debugImage = NSImage(CGImage: zoomImageRef, size: CGSizeMake(CGFloat(size), CGFloat(size)));
        
        
        self.scanView.image = image;
        self.zoomView.image = debugImage;
        
        self.loadColorInfo(debugImage);
    }
    
    func loadColorInfo(image: NSImage) {
        let imageRep = NSBitmapImageRep(data: image.TIFFRepresentation!);
        
        let centerX = imageRep!.pixelsWide / 2;
        let centerY = imageRep!.pixelsHigh / 2;
        
        let color = imageRep?.colorAtX(centerX, y: centerY);
        
        self.updateColorSelection(color!);
    }
    
    func onColorSelected(color: NSColor) {
        self.updateColorSelection(color);
    }
    
    func updateColorSelection(color: NSColor) {
        let red = UInt8(color.redComponent * 255);
        let green = UInt8(color.greenComponent * 255);
        let blue = UInt8(color.blueComponent * 255);
        
        self.rgbField.stringValue = String(format: "%d, %d, %d", red, green, blue);
        self.hexField.stringValue = String(format: "#%02X%02X%02X", red, green, blue).lowercaseString;
        
        self.colorView.layer?.backgroundColor = color.CGColor;
    }
}
