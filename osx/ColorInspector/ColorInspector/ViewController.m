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

@property (weak) IBOutlet NSTextField *mouseLocationTextField;
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

}

- (void)onMouseMove:(CGPoint)point {
    [self.mouseLocationTextField setStringValue:[NSString stringWithFormat:@"Mouse Location: %d, %d", (int)point.x, (int)point.y]];
}

- (void)onMouseUp:(CGPoint)point {

}

- (void)addBorderToImageView:(NSImageView *)imageView {
    imageView.wantsLayer = YES;
    imageView.layer.borderWidth = 1;
    imageView.layer.borderColor = [NSColor blackColor].CGColor;
}

@end
