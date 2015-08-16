//
//  MouseHook.h
//  ColorInspector
//
//  Created by Alan Rahlf on 8/9/15.
//  Copyright (c) 2015 Alan Rahlf. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol MouseHookDelegate <NSObject>

- (void)onMouseMove:(CGPoint)point;
- (void)onMouseUp:(CGPoint)point;

@end

@interface MouseHook : NSObject

@property (weak, nonatomic) id<MouseHookDelegate> delegate;

@end
