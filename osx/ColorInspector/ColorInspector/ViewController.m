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
@property (assign, nonatomic) CGColorSpaceRef colorSpace;

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
    
    [self loadColorInfo:scanImageRef];
    
    
    CGContextRelease(scanContext);
    CGContextRelease(zoomContext);
    CGImageRelease(zoomImageRef);
    CGImageRelease(scanImageRef);
    CGImageRelease(imageRef);
    CGImageRelease(zoomedImage);
}

- (void)loadColorInfo:(CGImageRef)imageRef {
    size_t bytesPerRow = CGImageGetBytesPerRow(imageRef);
    size_t bitsPerPixel = CGImageGetBitsPerPixel(imageRef);
    size_t bitsPerComponent = CGImageGetBitsPerComponent(imageRef);
    size_t bytesPerPixel = bitsPerPixel / bitsPerComponent;
    
    size_t imageWidth = CGImageGetWidth(imageRef);
    size_t imageHeight = CGImageGetHeight(imageRef);
    size_t centerX = (imageWidth / 2) - 1;
    size_t centerY = (imageHeight / 2) - 1;
    
    CFDataRef dataRef = CGDataProviderCopyData(CGImageGetDataProvider(imageRef));
    const UInt8 *data = CFDataGetBytePtr(dataRef);
    
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
    
    CFRelease(dataRef);
}

@end
