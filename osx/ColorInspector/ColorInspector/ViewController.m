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
#import "InspectButton.h"
#import "MouseHook.h"

@interface ViewController() <InspectButtonDelegate, MouseHookDelegate>

@property (strong, nonatomic) MouseHook *mouseHook;
@property (assign, nonatomic) BOOL scanning;

@property (weak) IBOutlet NSTextField *mouseLocationField;
@property (weak) IBOutlet NSTextField *rgbField;
@property (weak) IBOutlet NSTextField *hexField;
@property (weak) IBOutlet NSImageView *scanView;
@property (weak) IBOutlet NSImageView *zoomView;
@property (weak) IBOutlet NSImageView *colorView;
@property (weak) IBOutlet InspectButton *inspectButton;

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    self.inspectButton.delegate = self;
    
    self.mouseHook = [[MouseHook alloc] init];
    self.mouseHook.delegate = self;
    
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
    int size = 83;
    int xLoc = (int)point.x;
    int yLoc = (int)point.y;
     
    CGRect rect = CGRectMake(xLoc - (size / 2), yLoc - (size / 2), size, size);
     
    CGImageRef imageRef = CGDisplayCreateImageForRect(CGMainDisplayID(), rect);
     
    CFDataRef dataRef = CGDataProviderCopyData(CGImageGetDataProvider(imageRef));
     
    const UInt8 *data = CFDataGetBytePtr(dataRef);
    
    size_t bytesPerRow = CGImageGetBytesPerRow(imageRef);
    size_t bitsPerPixel = CGImageGetBitsPerPixel(imageRef);
    size_t bitsPerComponent = CGImageGetBitsPerComponent(imageRef);
    size_t bytesPerPixel = bitsPerPixel / bitsPerComponent;
    
    size_t imageWidth = CGImageGetWidth(imageRef);
    size_t imageHeight = CGImageGetHeight(imageRef);
    size_t centerX = (imageWidth / 2) - 1;
    size_t centerY = (imageHeight / 2) - 1;
    size_t pixelInfo = (centerY * bytesPerRow) + (centerX * bytesPerPixel);
    
    CGBitmapInfo bitmapInfo = CGImageGetBitmapInfo(imageRef);
    
    BOOL isBGRA = (bitmapInfo & kCGBitmapByteOrderMask) == kCGBitmapByteOrder32Little;
    int redIndexOffset = isBGRA ? 2 : 0;
    int blueIndexOffset = isBGRA ? 0 : 2;
    
    UInt8 red = data[pixelInfo + redIndexOffset];
    UInt8 green = data[(pixelInfo + 1)];
    UInt8 blue = data[(pixelInfo + blueIndexOffset)];
    
    self.rgbField.stringValue = [NSString stringWithFormat:@"%d, %d, %d", red, green, blue];
    self.hexField.stringValue = [[NSString stringWithFormat:@"#%02X%02X%02X", red, green, blue] lowercaseString];
    
    NSColor *color = [NSColor colorWithRed:red/255.0f green:green/255.0f blue:blue/255.0f alpha:1];
    
    self.colorView.layer.backgroundColor = color.CGColor;
     
    NSImage *image = [[NSImage alloc] initWithCGImage:imageRef size:NSMakeSize(size, size)];
     
    self.scanView.image = image;
     
    CFRelease(dataRef);
    CGImageRelease(imageRef);
}

@end
