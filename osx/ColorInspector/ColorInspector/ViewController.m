//
//  ViewController.m
//  ColorInspector
//
//  Created by Alan Rahlf on 8/9/15.
//  Copyright (c) 2015 Alan Rahlf. All rights reserved.
//

#import <ApplicationServices/ApplicationServices.h>
#import <CoreGraphics/CoreGraphics.h>
#import "ViewController.h"
#import <ColorInspector-Swift.h>

@interface ViewController() <InspectButtonDelegate, MouseHookDelegate, ColorSelectionDelegate>

@property (strong, nonatomic) MouseHook *mouseHook;
@property (assign, nonatomic) BOOL scanning;
@property (assign, nonatomic) CGColorSpaceRef colorSpace;

@property (weak) IBOutlet NSTextField *mouseLocationField;
@property (weak) IBOutlet NSTextField *rgbField;
@property (weak) IBOutlet NSTextField *hexField;
@property (weak) IBOutlet CrosshairImageView *scanView;
@property (weak) IBOutlet CrosshairImageView *zoomView;
@property (weak) IBOutlet NSImageView *colorView;
@property (weak) IBOutlet InspectButton *inspectButton;

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    self.inspectButton.delegate = self;
    self.zoomView.colorSelectionDelegate = self;
    
    self.mouseHook = [[MouseHook alloc] init];
    self.mouseHook.delegate = self;
    
    self.colorSpace = CGColorSpaceCreateWithName(kCGColorSpaceGenericRGB);
    
    [self addBorderToImageView:self.scanView];
    [self addBorderToImageView:self.zoomView];
    [self addBorderToImageView:self.colorView];
}

- (void)onMouseDown:(InspectButton *)inspectButton {
    self.scanning = YES;
}

- (void)onMouseMove:(CGPoint)point {
    self.mouseLocationField.stringValue = [NSString stringWithFormat:@"Mouse Location: %d, %d", (int)point.x, (int)point.y];
    
    if (self.scanning) {
        [self updateScanView:point];
    }
}

- (void)onMouseUp:(CGPoint)point {
    if (self.scanning) {
        self.scanning = NO;
    }
}

- (void)addBorderToImageView:(NSImageView *)imageView {
    imageView.wantsLayer = YES;
    imageView.layer.borderWidth = 1;
    imageView.layer.borderColor = [NSColor blackColor].CGColor;
}

- (void)updateScanView:(CGPoint)point {
    int size = 81;
    int halfSize = size / 2;
    int xLoc = (int)point.x;
    int yLoc = (int)point.y;
    
    int wide = (int)CGDisplayPixelsWide(CGMainDisplayID()) - 1;
    int high = (int)CGDisplayPixelsHigh(CGMainDisplayID()) - 1;
    
    int frameX = MAX(0, MIN(wide - size, xLoc - halfSize));
    int frameY = MAX(0, MIN(high - size, yLoc - halfSize));
    
    CGRect rect = CGRectMake(frameX, frameY, size, size);
    
    CGImageRef imageRef = CGDisplayCreateImageForRect(CGMainDisplayID(), rect);
    
    CGFloat scale = [NSScreen mainScreen].backingScaleFactor;
    
    CGContextRef scanContext = CGBitmapContextCreate(NULL, size * scale, size * scale, 8, 0, self.colorSpace, (CGBitmapInfo)kCGImageAlphaNoneSkipLast);
    CGContextRef zoomContext = CGBitmapContextCreate(NULL, size * scale, size * scale, 8, 0, self.colorSpace, (CGBitmapInfo)kCGImageAlphaNoneSkipLast);
    
    int adjustedX = 0;
    int adjustedY = 0;
    
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
    
    
    CGContextDrawImage(scanContext, CGRectMake(adjustedX, adjustedY, rect.size.width * scale, rect.size.height * scale), imageRef);
    
    CGImageRef scanImageRef = CGBitmapContextCreateImage(scanContext);
    
    CGRect zoomImageRect = CGRectMake(38 * scale, 38 * scale, 9, 9);
    
    CGImageRef zoomedImage = CGImageCreateWithImageInRect(scanImageRef, zoomImageRect);

    CGContextSetInterpolationQuality(zoomContext, kCGInterpolationNone);
    
    CGContextDrawImage(zoomContext, CGRectMake(0, 0, size * scale, size * scale), zoomedImage);
    
    CGImageRef zoomImageRef = CGBitmapContextCreateImage(zoomContext);
    
    NSImage *image = [[NSImage alloc] initWithCGImage:scanImageRef size:CGSizeMake(size, size)];
    NSImage *debugImage = [[NSImage alloc] initWithCGImage:zoomImageRef size:CGSizeMake(size, size)];

    
    self.scanView.image = image;
    self.zoomView.image = debugImage;
    
    [self loadColorInfo:debugImage];
    
    CGContextRelease(scanContext);
    CGContextRelease(zoomContext);
    CGImageRelease(zoomImageRef);
    CGImageRelease(scanImageRef);
    CGImageRelease(imageRef);
    CGImageRelease(zoomedImage);
}

- (void)loadColorInfo:(NSImage *)image {
    NSBitmapImageRep *imageRep = [[NSBitmapImageRep alloc] initWithData:image.TIFFRepresentation];
    
    NSInteger centerX = (imageRep.pixelsWide / 2);
    NSInteger centerY = (imageRep.pixelsHigh / 2);
    
    NSColor *color = [imageRep colorAtX:centerX y:centerY];
    
    [self updateColorSelection:color];
}

- (void)onColorSelected:(NSColor *)color {
    [self updateColorSelection:color];
}

- (void)updateColorSelection:(NSColor *)color {
    UInt8 red = color.redComponent * 255;
    UInt8 green = color.greenComponent * 255;
    UInt8 blue = color.blueComponent * 255;
    
    self.rgbField.stringValue = [NSString stringWithFormat:@"%d, %d, %d", red, green, blue];
    self.hexField.stringValue = [[NSString stringWithFormat:@"#%02X%02X%02X", red, green, blue] lowercaseString];
    
    self.colorView.layer.backgroundColor = color.CGColor;
}

@end
