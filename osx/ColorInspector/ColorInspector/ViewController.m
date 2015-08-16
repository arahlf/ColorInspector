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
#import "MouseHook.h"

@interface ViewController() <MouseHookDelegate>

@property (strong, nonatomic) MouseHook *mouseHook;

@property (weak) IBOutlet NSTextField *mouseLocationTextField;

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    self.mouseHook = [[MouseHook alloc] init];
    self.mouseHook.delegate = self;
}

- (void)onMouseMove:(CGPoint)point {
    [self.mouseLocationTextField setStringValue:[NSString stringWithFormat:@"Mouse Location: %d, %d", (int)point.x, (int)point.y]];
}

- (void)onMouseUp:(CGPoint)point {

}

@end
