#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>




@interface Utils : NSObject

+ (UIImage*) getRotatedImage : (UIImage*) image
                            rotation: (CGFloat) rotation
             horizontalMargin: (CGFloat) horizontalMargin
               verticalMargin: (CGFloat) verticalMargin
;


+ (CGFloat) DegreesToRadians: (CGFloat) degrees;

+ (CGFloat) RadiansToDegrees: (CGFloat) radians;

@end


@implementation Utils

+ (UIImage*) getRotatedImage : (UIImage*) image
                     rotation: (CGFloat) rotation
             horizontalMargin: (CGFloat) horizontalMargin
               verticalMargin: (CGFloat) verticalMargin
{
    
    // Calculate Destination Size
    CGAffineTransform t = CGAffineTransformMakeRotation([Utils DegreesToRadians:rotation]);
    CGRect sizeRect = CGRectMake(0, 0, image.size.width + horizontalMargin, image.size.height + verticalMargin);
    CGRect destRect = CGRectApplyAffineTransform(sizeRect, t);
    CGSize destinationSize = CGSizeMake(destRect.size.width, destRect.size.height);
    
    
    // Draw image
    UIGraphicsBeginImageContext(destinationSize);
    CGContextRef context = UIGraphicsGetCurrentContext();
    CGContextTranslateCTM(context, destinationSize.width / 2.0f, destinationSize.height / 2.0f);
    CGContextRotateCTM(context, [Utils DegreesToRadians:rotation]);
    [image drawInRect:CGRectMake(-(image.size.width + horizontalMargin/2 )/ 2.0f, -(image.size.height + verticalMargin/2 ) / 2.0f, image.size.width, image.size.height)];
    
    // Save image
    UIImage *newImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    return newImage;
}

+ (CGFloat) DegreesToRadians: (CGFloat) degrees {
    return degrees * M_PI / 180;
};

+ (CGFloat) RadiansToDegrees: (CGFloat) radians {
    return radians * 180 / M_PI;
};

@end

@interface ScreenMarkerView : NSObject
//
//  Properties
//
@property (nonatomic, strong) NSString* userInfo;

@property (strong, nonatomic) UIWindow *overlayWindow;

//  Views
@property (nonatomic, strong) UIView* screenMarkerView;
@property (nonatomic, strong) UIImageView* uiImageView;
@property (nonatomic, strong) UIImageView* uiTiledTextView;

@property (nonatomic) CGFloat screenMarkerAlpha;

//  Text View Properties
@property (nonatomic, strong) UIFont* defaultFont;

@property (nonatomic, strong) NSMutableArray* uiTextViewArray;
@property (nonatomic) bool useTextTileMode;
@property (nonatomic, strong) UIImage* tiledTextImage;
@property (nonatomic) NSInteger tiledTextHorizontalMargin;
@property (nonatomic) NSInteger tiledTextVerticalMargin;

//  Image View Properties
@property (nonatomic, strong) UIImage* imageSource;
@property (nonatomic) CGPoint imagePosition;
@property (nonatomic) NSInteger imageRotation;
@property (nonatomic) bool useImageTileMode;
@property (nonatomic, strong) UIImage* tiledImage;
@property (nonatomic) NSInteger tiledImageHorizontalMargin;
@property (nonatomic) NSInteger tiledImageVerticalMargin;


//
//  Functions
//
//  Initializers
- (instancetype) initWithUserId : (NSString*)userInfo;
- (instancetype) initClearMode: (NSString*)userInfo;
- (void) setUserInfo:(NSString * _Nonnull)userInfo;
- (void) setDefaultLayout;

//  ScreenMarker Functions
- (void)showScreenMarker;
- (void)hideScreenMarker;
- (void)setScreenMarkerAlpha: (CGFloat) alpha;


//  Text Functions
- (void) addTextWithRect: (CGRect) rect
                    text: (nullable NSString*) text
                    font: (nullable UIFont*) font
             colorString: (NSString*) colorString
                   angle: (NSInteger) angle
            useSizeToFit: (bool) useSizeToFit;

- (void) addTextWithCenter: (CGPoint) center
                      text: (nullable NSString*) text
                      font: (nullable UIFont*) font
               colorString: (NSString*) colorString
                     angle: (NSInteger) angle;

- (void) clearTextAll;
- (void) setTextAll: (NSString*) text;
- (void) setTextRotationAll: (NSInteger) angle;
- (void) setTextColorAll: (NSString*) colorString;
- (void) setTextFontAll: (UIFont*) font;
- (void) setTextTileMode: (nullable NSString*) text
                    font: (nullable UIFont*) font
             colorString: (NSString*) colorString
                   angle: (NSInteger) angle
        horizontalMargin: (NSInteger) horizontalMargin
          verticalMargin: (NSInteger) verticalMargin;
- (void) unsetTextTileMod;


//  Image Functions
  - (void) setImageSource: (nullable UIImage*) image;
//  - (void) setImagePosition: (CGPoint) center;
//  - (void) setImageRotation: (NSInteger) angle;
- (void) setImageTileMode:  (UIImage*) image
                    angle: (NSInteger) angle
         horizontalMargin: (NSInteger) horizontalMargin
           verticalMargin: (NSInteger) verticalMargin;
- (void) setImageTileModeWithText:  (UIImage*) image
                             text: (nullable NSString*) text
                             font: (nullable UIFont*) font
                      colorString: (NSString*) colorString
                            angle: (NSInteger) angle
                 horizontalMargin: (NSInteger) horizontalMargin
                   verticalMargin: (NSInteger) verticalMargin;
- (void) unsetImageTileMode;

@end


//
//  ScreenMarkerView.m
//  ScreenMarkingOverlay
//
//  Created by macpro on 2019. 4. 24..
//  Copyright © 2019년 macpro. All rights reserved.
//

#define UIColorFromARGB(argbValue) [UIColor \
    colorWithRed:((float)((argbValue & 0xFF0000) >> 16))/255.0 \
    green:((float)((argbValue & 0xFF00) >> 8))/255.0 \
    blue:((float)(argbValue & 0xFF))/255.0 \
    alpha:((float)((argbValue & 0xFF000000) >> 24))/255.0]

@implementation ScreenMarkerView

- (instancetype) init {
    NSLog(@"Initialize Failed, Use initWithUserId");
    return nil;
}

/**
 * ScreenMarker의 Layout에 아무것도 생성하지 않고 초기화.
 * @param   userInfo 일반적으로 행원번호. ScreenMarker내 텍스트의 초기값
 */
- (instancetype) initClearMode: (NSString*)userInfo {
    
//    ScreenMarker Default Values
    CGSize mainBoundSize = [[UIScreen mainScreen] bounds].size;
    
    _userInfo = userInfo;
    _screenMarkerAlpha = 1;
    _defaultFont = [UIFont systemFontOfSize:20];
    _uiTextViewArray = [ [NSMutableArray alloc] init];
    _imagePosition = CGPointMake(mainBoundSize.width/2, mainBoundSize.height/2);
    _imageRotation = 0;
    
    return self;
}


/**
 * ScreenMarker의 Layout에 하나은행 시안에 Spec에 맞게 View를 구성하여 초기화
 * @param   userInfo 일반적으로 행원번호. ScreenMarker내 텍스트의 초기값
 */
- (instancetype) initWithUserId : (NSString*)userInfo {

//    ScreenMarker Default Values
    _userInfo = userInfo;
    [self setDefaultLayout];
    
    return self;
}

- (void) setUserInfo:(NSString *)userInfo {
    _userInfo = userInfo;
}

- (void) setDefaultLayout {
    CGSize mainBoundSize = [[UIScreen mainScreen] bounds].size;
    
    _screenMarkerAlpha = 1;
    _defaultFont = [UIFont systemFontOfSize:20];
    _uiTextViewArray = [ [NSMutableArray alloc] init];
    
    _useTextTileMode = false;
    
    //
    //    Default Layout Texts
    //    Center Text
    unsigned colorCenter = 0;
    [[NSScanner scannerWithString:@"4c6a6a6a"] scanHexInt:&colorCenter];
    
    UILabel* _uiTextView_center = [[UILabel alloc] init];
    [_uiTextView_center setText:_userInfo];
    [_uiTextView_center setFont:_defaultFont];
    [_uiTextView_center sizeToFit];
    
    _uiTextView_center.center = CGPointMake(mainBoundSize.width/2, mainBoundSize.height/2);
    _uiTextView_center.backgroundColor = [UIColor clearColor];
    _uiTextView_center.textColor = UIColorFromARGB(colorCenter);
    
    [_uiTextViewArray addObject:_uiTextView_center];
    
    //    BottomText
    
    unsigned colorBottom = 0;
    [[NSScanner scannerWithString:@"4cbdbdbd"] scanHexInt:&colorBottom];
    
    UILabel* _uiTextView_bottomCenter = [[UILabel alloc] init];
    [_uiTextView_bottomCenter setText:_userInfo];
    [_uiTextView_bottomCenter setFont:_defaultFont];
    [_uiTextView_bottomCenter sizeToFit];
    _uiTextView_bottomCenter.backgroundColor = [UIColor clearColor];
    _uiTextView_bottomCenter.textColor = UIColorFromARGB(colorBottom);
    _uiTextView_bottomCenter.center = CGPointMake(mainBoundSize.width/2, mainBoundSize.height - 40);
    
    [_uiTextViewArray addObject:_uiTextView_bottomCenter];
    
    //    Default Layout Image
    //    Images Source
    // NSBundle *bundle = [NSBundle bundleForClass: [ScreenMarkerView class]];
    // _imageSource = [UIImage imageWithContentsOfFile:[bundle pathForResource:@"hana_logo" ofType:@"png"]];
    // _imageRotation = 0;
    // _imagePosition = CGPointMake(mainBoundSize.width/2, mainBoundSize.height/2);
}


/**
 *  ScreenMarker 및 구성요소 표시
 */
- (void) showScreenMarker {
    
    if(_uiTiledTextView) [_uiTiledTextView removeFromSuperview];
    if(_uiImageView) [_uiImageView removeFromSuperview];
    if(_screenMarkerView) [_screenMarkerView removeFromSuperview];
   
    
//    Get KeyWindow
    _overlayWindow = [[UIWindow alloc] initWithFrame:CGRectMake(0, 0, [[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height)];
    _overlayWindow.windowLevel = 2005;
    [_overlayWindow setUserInteractionEnabled: false];
//
//    Main View Initialization
    _screenMarkerView = [[UIView alloc] initWithFrame:CGRectMake(0, 0, [[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height)];
    
    [_screenMarkerView setUserInteractionEnabled:false];
    [_screenMarkerView setAlpha: _screenMarkerAlpha];
    
    
//
//    Add TextViews
    if( !_useTextTileMode) {
        // TileMode OfF
        for(int i = 0; i < _uiTextViewArray.count; i++) {
            [_screenMarkerView addSubview:_uiTextViewArray[i] ];
        }
    } else {
         // TileMode ON
        _uiTiledTextView = [[UIImageView alloc] initWithFrame:CGRectMake(0, 0, _screenMarkerView.frame.size.width, _screenMarkerView.frame.size.height)];
        
        _uiTiledTextView.backgroundColor = [UIColor colorWithPatternImage: _tiledTextImage ];
        
        [_screenMarkerView addSubview:_uiTiledTextView];
    }
    
    
//
//    Add ImageView
    
    if(_imageSource) {
        
        if( !_useImageTileMode ) {
            // TileMode OFF
            _uiImageView = [[UIImageView alloc] initWithFrame:CGRectMake(0, 0, _imageSource.size.width, _imageSource.size.height)];
            _uiImageView.contentMode = UIViewContentModeScaleAspectFit;
            
            [_uiImageView setImage:_imageSource];
            _uiImageView.transform = CGAffineTransformMakeRotation([Utils DegreesToRadians:_imageRotation]);
            _uiImageView.center = _imagePosition ;
            
        } else {
            // TileMode ON
            _uiImageView = [[UIImageView alloc] initWithFrame:CGRectMake(0, 0, _screenMarkerView.frame.size.width, _screenMarkerView.frame.size.height)];
            _uiImageView.backgroundColor = [UIColor colorWithPatternImage: _tiledImage];
        }
        
        [_screenMarkerView addSubview:_uiImageView];
    } else {
        [_uiImageView removeFromSuperview];
    }
    
    
    [_overlayWindow addSubview:_screenMarkerView];
    [_overlayWindow makeKeyAndVisible];
    
}

/**
 *  ScreenMarker 감추기
 */
- (void) hideScreenMarker {
    [_screenMarkerView removeFromSuperview];
}

- (void) setScreenMarkerAlpha:(CGFloat)alpha {
    [_screenMarkerView setAlpha: alpha];
}


//
//  Text Controllers
//
- (void) addTextWithRect:(CGRect)rect text:(NSString *)text font:(UIFont *)font colorString:(NSString *)colorString angle:(NSInteger)angle useSizeToFit:(bool)useSizeToFit {
    
    
    unsigned textColor = 0;
    [[NSScanner scannerWithString:colorString] scanHexInt:&textColor];

    UILabel* newTextView = [[UILabel alloc] init];
    if(text) {
        [newTextView setText:text];
    } else {
        [newTextView setText:_userInfo];
    }

     newTextView.frame = rect;
    
    if(font) {
        [newTextView setFont:font];
    } else {
        [newTextView setFont:_defaultFont ];
    }
    

    if(useSizeToFit) {
        [newTextView sizeToFit];
    }
    
    newTextView.transform = CGAffineTransformMakeRotation([Utils DegreesToRadians:angle]);
    newTextView.backgroundColor = [UIColor clearColor];
    newTextView.textColor = UIColorFromARGB(textColor);

    [_uiTextViewArray addObject: newTextView];
    
    [self showScreenMarker];
    
}

- (void) addTextWithCenter:(CGPoint)center text:(NSString *)text font:(UIFont *)font colorString:(NSString *)colorString angle:(NSInteger)angle {
    
    unsigned textColor = 0;
    [[NSScanner scannerWithString:colorString] scanHexInt:&textColor];

    UILabel* newTextView = [[UILabel alloc] init];
    if(text) {
        [newTextView setText:text];
    } else {
        [newTextView setText:_userInfo];
    }
    

    if(font) {
        [newTextView setFont:font];
    } else {
        [newTextView setFont:_defaultFont ];
    }

    [newTextView sizeToFit];
    newTextView.transform = CGAffineTransformMakeRotation([Utils DegreesToRadians:angle]);


    newTextView.center = center;
    newTextView.backgroundColor = [UIColor clearColor];
    newTextView.textColor = UIColorFromARGB(textColor);

    [_uiTextViewArray addObject: newTextView];
    
    [self showScreenMarker];
}


- (void) clearTextAll {
    
    for(int i = 0; i < _uiTextViewArray.count; i++) {
        UILabel* textLabel = _uiTextViewArray[i];
        
        [textLabel removeFromSuperview];
    }
    
    
    [_uiTextViewArray removeAllObjects];
    _uiTextViewArray = [ [NSMutableArray alloc] init];
}

- (void) setTextAll: (NSString*) text {
    for(int i = 0; i < _uiTextViewArray.count; i++) {
        UILabel* textLabel = _uiTextViewArray[i];
        CGPoint center= textLabel.center;

        [textLabel setText:text];
        [textLabel sizeToFit];
        textLabel.center = center;
    }
}

- (void) setTextRotationAll:(NSInteger)angle {
    for(int i = 0; i < _uiTextViewArray.count; i++) {
        UILabel* textLabel = _uiTextViewArray[i];
        CGPoint center= textLabel.center;
        
        textLabel.transform = CGAffineTransformMakeRotation([Utils DegreesToRadians:angle]);
        
        [textLabel sizeToFit];
        textLabel.center = center;
    }
    
}

- (void) setTextColorAll:(NSString *)colorString {
    unsigned textColor = 0;
    [[NSScanner scannerWithString:colorString] scanHexInt:&textColor];
    
    for(int i = 0; i < _uiTextViewArray.count; i++) {
        UILabel* textLabel = _uiTextViewArray[i];

        textLabel.textColor = UIColorFromARGB(textColor);
    }
}

- (void) setTextFontAll:(UIFont *)font {
    for(int i = 0; i < _uiTextViewArray.count; i++) {
        UILabel* textLabel = _uiTextViewArray[i];
        
        [textLabel setFont: font];
        [textLabel sizeToFit];
    }
}

- (void) setTextTileMode: (nullable NSString*) text
                    font: (nullable UIFont*) font
             colorString: (NSString*) colorString
                   angle: (NSInteger) angle
        horizontalMargin: (NSInteger) horizontalMargin
          verticalMargin: (NSInteger) verticalMargin {
    
    
    
    [self clearTextAll];
    
    UILabel* textLabel = [[UILabel alloc] init];
    NSString* tiledText;
    UIFont* tiledFont;
    
    
    if(text) {
        [textLabel setText:text];
        tiledText = text;
    } else {
        [textLabel setText:_userInfo];
        tiledText = _userInfo;
    }
    
    
    if(font) {
        [textLabel setFont:font];
        tiledFont = font;
    } else {
        [textLabel setFont:_defaultFont ];
        tiledFont = _defaultFont;
    }

    [textLabel sizeToFit];
    
    unsigned textColor = 0;
    [[NSScanner scannerWithString:colorString] scanHexInt:&textColor];
    
    
    NSDictionary *attr = [NSDictionary dictionaryWithObjects:
                                @[tiledFont, UIColorFromARGB(textColor)]
                                                           forKeys:
                                @[NSFontAttributeName, NSForegroundColorAttributeName]];

    UIGraphicsBeginImageContext(textLabel.frame.size);
    
    [tiledText drawAtPoint:CGPointMake(0, 0)
 withAttributes: attr];
    UIImage *result = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    _tiledTextImage = [Utils getRotatedImage:result rotation:angle  horizontalMargin:horizontalMargin verticalMargin:verticalMargin];
    
    _useTextTileMode = true;
    
    [self showScreenMarker];
}


- (void) unsetTextTileMod {
    
    _useTextTileMode = false;
    _tiledTextImage = nil;
    
    [self showScreenMarker];
}


//
//  Image Controllers
//
- (void)setImageSource:(nullable UIImage *)image {
    _imageSource = nil;
    _imageSource = image;
    
    [self showScreenMarker];
}

- (void) setImagePosition:(CGPoint)imagePosition {
    _imagePosition = imagePosition;
    
    [self showScreenMarker];
}


- (void) setImageRotation:(NSInteger)imageRotation {
    _imageRotation = imageRotation;
    
    [self showScreenMarker];
}


- (void) setImageTileMode:(UIImage *)image angle:(NSInteger)angle horizontalMargin:(NSInteger)horizontalMargin verticalMargin:(NSInteger)verticalMargin {
    
    _tiledImage = [Utils getRotatedImage:image rotation: angle  horizontalMargin:horizontalMargin verticalMargin:verticalMargin];
    
    _useImageTileMode = true;
    
    [self showScreenMarker];
}


- (void) unsetImageTileMode {
    _useImageTileMode = false;
    _tiledImage = nil;
    
    [self showScreenMarker];
}

- (void) setImageTileModeWithText:(UIImage *)image text:(nullable NSString*) text
                   font: (nullable UIFont*) font
            colorString: (NSString*) colorString
                            angle:(NSInteger)angle
                 horizontalMargin:(NSInteger)horizontalMargin verticalMargin:(NSInteger)verticalMargin {
    

       [self clearTextAll];
       
       UILabel* textLabel = [[UILabel alloc] init];
       NSString* tiledText;
       UIFont* tiledFont;
       
       
       if(text) {
           [textLabel setText:text];
           tiledText = text;
       } else {
           [textLabel setText:_userInfo];
           tiledText = _userInfo;
       }
       
       
       if(font) {
           [textLabel setFont:font];
           tiledFont = font;
       } else {
           [textLabel setFont:_defaultFont ];
           tiledFont = _defaultFont;
       }

       [textLabel sizeToFit];
       
       unsigned textColor = 0;
       [[NSScanner scannerWithString:colorString] scanHexInt:&textColor];
       
       
       NSDictionary *attr = [NSDictionary dictionaryWithObjects:
                                   @[tiledFont, UIColorFromARGB(textColor)]
                                                              forKeys:
                                   @[NSFontAttributeName, NSForegroundColorAttributeName]];

        int newWidth, newHeight;
    
    newWidth = textLabel.frame.size.width > image.size.width ? textLabel.frame.size.width : image.size.width;
    
    newHeight = image.size.height + textLabel.frame.size.height;
    
       UIGraphicsBeginImageContext(CGSizeMake(newWidth, newHeight));
       
    [image drawAtPoint:CGPointMake(newWidth/2 - image.size.width/2, 0)];
    
       [tiledText drawAtPoint:CGPointMake(newWidth/2 - textLabel.frame.size.width/2, image.size.height) withAttributes: attr];
       UIImage *result = UIGraphicsGetImageFromCurrentImageContext();
       UIGraphicsEndImageContext();
    
    
    
    _tiledImage = [Utils getRotatedImage:result rotation: angle  horizontalMargin:horizontalMargin verticalMargin:verticalMargin];
    
    _useImageTileMode = true;
    
    [self showScreenMarker];
}

@end


@interface ScreenMarker : NSObject

//  Library Implementation Test
+ (void)implementationTest;

//  ScreenMarker Functions
+ (void)initScreenMarker: (NSString*) userInfo;

+ (void)showScreenMarker;
+ (void)hideScreenMarker;
+ (void)setScreenMarkerAlpha: (CGFloat) alpha;


//  Text Functions
+ (void) addTextWithRect: (CGRect) rect
                    text: (nullable NSString*) text;
+ (void) addTextWithRect: (CGRect) rect
                    text: (nullable NSString*) text
                    font: (nullable UIFont*) font
             colorString: (NSString*) colorString
                   angle: (NSInteger) angle
            useSizeToFit: (bool) useSizeToFit;


+ (void) addTextWithCenter: (CGPoint) center
                      text: (nullable NSString*) text;
+ (void) addTextWithCenter: (CGPoint) center
                      text: (nullable NSString*) text
                      font: (nullable UIFont*) font
               colorString: (NSString*) colorString
                     angle: (NSInteger) angle;

+ (void) clearTextAll;
+ (void) setTextAll: (NSString*) text;
+ (void) setTextRotationAll: (NSInteger) angle;
+ (void) setTextColorAll: (NSString*) colorString;
+ (void) setTextFontAll: (UIFont*) font;
+ (void) setTextTileMode:   (nullable NSString*) text
                    font: (nullable UIFont*) font
             colorString: (NSString*) colorString
                   angle: (NSInteger) angle
        horizontalMargin: (NSInteger) horizontalMargin
          verticalMargin: (NSInteger) verticalMargin;
+ (void) unsetTextTileMod;


//  Image Functions
+ (void) setImageSource: (nullable UIImage*) image;
+ (void) setImagePosition: (CGPoint) center;
+ (void) setImageRotation: (NSInteger) angle;
+ (void) setImageTileMode:  (UIImage*) image
                    angle: (NSInteger) angle
         horizontalMargin: (NSInteger) horizontalMargin
           verticalMargin: (NSInteger) verticalMargin;

+ (void) setImageTileModeWithText:  (UIImage*) image
text: (nullable NSString*) text
       font: (nullable UIFont*) font
colorString: (NSString*) colorString
           angle: (NSInteger) angle
horizontalMargin: (NSInteger) horizontalMargin
  verticalMargin: (NSInteger) verticalMargin;

+ (void) unsetImageTileMode;

@end



/**
 * Interface 부분으로
 * 일반적으로 동명의 Function을 호출한다.
 */
@implementation ScreenMarker

static ScreenMarkerView* screenMarkerView = nil;
NSString* const defaultColorString = @"FF000000";

/**
 *  라이브러리 버전 코드 출력.
 *  연동여부 확인용 Function
 */
+(void) implementationTest
{
    NSDictionary *info = [[NSBundle bundleForClass: [ScreenMarker class]] infoDictionary];
    NSString *version = [info objectForKey:@"CFBundleShortVersionString"];
    NSLog(@"\nScreenMarkingOverlay\nVersion %@", version);
}


/**
 *  ScreenMarker 초기화
 *  @param userInfo 일반적으로 행원번호. ScreenMarker내 텍스트의 default 값으로 쓰인다.
 */
+ (void) initScreenMarker: (NSString*) userInfo
{
    if(!screenMarkerView) {
        screenMarkerView = [[ScreenMarkerView alloc] initWithUserId: userInfo];
    } else {
        [screenMarkerView setUserInfo: userInfo];
        [screenMarkerView setDefaultLayout];
        [screenMarkerView showScreenMarker];
    }
}

/**
 *  ScreenMarker 보이기
 */
+ (void) showScreenMarker {
    [screenMarkerView showScreenMarker];
}

/**
 *  ScreenMarker 감추기
 */
+ (void) hideScreenMarker {
    [screenMarkerView hideScreenMarker];
}

/**
 *  ScreenMarker 의 전체 Aplha(투명도 조절)
 *  @param   alpha 1~0의 값을 가지며 '0'은 투명 '1'은 불투명
            텍스트, 이미지의 Alpha값과 별개로 동작한다.
 */
+ (void)setScreenMarkerAlpha: (CGFloat) alpha {
    [screenMarkerView setScreenMarkerAlpha: alpha];
}


//Text Functions


/**
 *  ScreenMarker 내 TextView 생성 (Rect 정보 이용)
 *  @param   rect Text가 생성될 위치에 대한 Rect 정보.
 *  @param   text Text의 내용. 'Nil'일 경우 "initScreenMarker"의 {userInfo} 값으로 적용
 *  @param   font Font 및 FontSize 정보. 'Nil' 일경우 시스템 폰트 12pt 사용
 *  @param   colorString ARGB의 32비트 색상String값.
 *  @param   angle TextView의 회전각(Degree)
 *  @param   useSizeToFit {rect}의 사이즈를 텍스트 사이즈에 딱 맞게 조정
 */
+ (void) addTextWithRect:   (CGRect) rect
                            text: (nullable NSString*) text
                            font: (nullable UIFont*) font
                            colorString: (NSString*) colorString
                            angle: (NSInteger) angle
                            useSizeToFit: (bool) useSizeToFit
{
    [screenMarkerView addTextWithRect:rect text:text font:font colorString:colorString angle:angle useSizeToFit:useSizeToFit];
}

+ (void) addTextWithRect:   (CGRect) rect
                    text: (nullable NSString*) text {
    [screenMarkerView addTextWithRect:rect text:text font:nil colorString:defaultColorString angle:0 useSizeToFit:true];
}



/**
 *  ScreenMarker 내 TextView 생성 (Center Point 이용)
 *  @param   center Text가 생성될 텍스트의 중심점 위치.
 *  @param   text Text의 내용. 'Nil'일 경우 "initScreenMarker"의 {userInfo} 값으로 적용
 *  @param   font Font 및 FontSize 정보. 'Nil' 일경우 시스템 폰트 12pt 사용
 *  @param   colorString ARGB의 32비트 색상String값.
 *  @param   angle TextView의 회전각(Degree)
 */

+ (void) addTextWithCenter: (CGPoint) center
                            text: (nullable NSString*) text
                            font: (nullable UIFont*) font
                            colorString: (NSString*) colorString
                            angle: (NSInteger) angle

{
    [screenMarkerView addTextWithCenter:center text:text font:font colorString:colorString angle:angle];
}

+ (void) addTextWithCenter: (CGPoint) center
                      text: (nullable NSString*) text {
    [screenMarkerView addTextWithCenter:center text:text font:nil colorString: defaultColorString angle:0];
}


/**
 *  ScreenMarker 내 모든 Text 제거
 */
+ (void) clearTextAll {
    [screenMarkerView clearTextAll];
}

/**
 *  ScreenMarker 내 모든 Text의 내용 변경
 *  변경 가능한 값은
    1. Text - TextView의 Text 내용
    2. Rotation - TextView의 최초생성 각도로부터의 회전각(Degree)
    3. Color - TextView의 Text 색상
    4. Font - TextView의 Font 및 FontSize
 */
+ (void) setTextAll: (NSString*) text {
    [screenMarkerView setTextAll:text];
}

+ (void) setTextRotationAll: (NSInteger) angle {
    [screenMarkerView setTextRotationAll:angle];
}

+ (void) setTextColorAll: (NSString*) colorString {
    [screenMarkerView setTextColorAll:colorString];
}

+ (void) setTextFontAll: (UIFont*) font {
    [screenMarkerView setTextFontAll:font];
}



/**
 *  ScreenMarker 내 전체 텍스트를 제거 하고
 *  전달받은 Parameter의 대한 텍스트를 생성, 이미지화 하여 타일모드로 적용.
 *  @param   text Text의 내용. 'Nil'일 경우 "initScreenMarker"의 {userInfo} 값으로 적용
 *  @param   font Font 및 FontSize 정보. 'Nil' 일경우 시스템 폰트 12pt 사용
 *  @param   colorString ARGB의 32비트 색상String값.
 *  @param   angle Text의 회전각(Degree)
 *  @param   horizontalMargin Text간 상하 간격
 *  @param   verticalMargin Text간 좌우 간격
 *
 *  Margin 값의 경우 {angle}값에 관계없이 상하좌우에 적용된다.
 */
+ (void) setTextTileMode:   (nullable NSString*) text
                            font: (nullable UIFont*) font
                            colorString: (NSString*) colorString
                            angle: (NSInteger) angle
                            horizontalMargin: (NSInteger) horizontalMargin
                            verticalMargin: (NSInteger) verticalMargin

{
    [screenMarkerView setTextTileMode:text font:font colorString:colorString angle:angle horizontalMargin:horizontalMargin verticalMargin:verticalMargin];
}

/**
 *  ScreenMarker 내 Text의 Tile모드 해제.
 *  해제 시, 전체 텍스트 제거
 */
+ (void) unsetTextTileMod {
    [screenMarkerView unsetTextTileMod];
}



//Image Functions

/**
 *  ScreenMarker 내 이미지는 한개만 사용
 *  변경가능한 속성은
    1. Source - 이미지 파일
    2. Position - 이미지의 중심점 위치
    3. Rotation - 이미지의 최초생성 각도로부터의 회전각(Degree)
 */
+ (void) setImageSource: (nullable UIImage*) image {
    [screenMarkerView setImageSource: image];
}

+ (void) setImagePosition: (CGPoint) center {
    [screenMarkerView setImagePosition: center];
}

+ (void) setImageRotation: (NSInteger) angle {
    [screenMarkerView setImageRotation: angle];
}

/**
 *  ScreenMarker 내 Image의 Tile모드 활성화
 *  @param   image Text의 내용. 'Nil'일 경우 "initScreenMarker"의 {userInfo} 값으로 적용
 *  @param   angle Font 및 FontSize 정보. 'Nil' 일경우 시스템 폰트 12pt 사용
 *  @param   horizontalMargin ARGB의 32비트 색상String값.
 *  @param   verticalMargin TextView의 회전각(Degree)
 *
 *  Margin 값의 경우 {angle}값에 관계없이 상하좌우에 적용된다.
 */
+ (void) setImageTileMode:  (UIImage*) image
                            angle: (NSInteger) angle
                            horizontalMargin: (NSInteger) horizontalMargin
                            verticalMargin: (NSInteger) verticalMargin

{
    [screenMarkerView setImageTileMode:image angle:angle horizontalMargin:horizontalMargin verticalMargin:verticalMargin];
}

+ (void) setImageTileModeWithText:  (UIImage*) image
            text: (nullable NSString*) text
            font: (nullable UIFont*) font
     colorString: (NSString*) colorString
           angle: (NSInteger) angle
horizontalMargin: (NSInteger) horizontalMargin
                   verticalMargin: (NSInteger) verticalMargin
{
    [screenMarkerView setImageTileModeWithText:image text:text font:font colorString:colorString angle:angle horizontalMargin:horizontalMargin verticalMargin:verticalMargin];
}

/**
 *  ScreenMarker 내 Image의 Tile모드 해제.
 *  해제 시, 마지막 단일 Image모드의 상태로 전환
 */
+ (void) unsetImageTileMode {
    
    [screenMarkerView unsetImageTileMode];
}



@end

extern "C" 
{
    void _InitScreenMarker(const char* userInfo)
    {
        NSString* userInfoString = [NSString stringWithUTF8String:userInfo];
        [ScreenMarker initScreenMarker: userInfoString];
    }

    void _ShowScreenMarker()
    {
        [ScreenMarker showScreenMarker];
    }

    void _HideScreenMarker()
    {
        [ScreenMarker hideScreenMarker];
    }

    void _SetScreenMarkerAlpha(float alpha)
    {
        [ScreenMarker setScreenMarkerAlpha: alpha];
    }

    void _SetImageSource(Byte* byteData, int length)
    {
        NSData *pictureData = [NSData dataWithBytes:byteData length:length];
        UIImage *image = [UIImage imageWithData:pictureData];
        [ScreenMarker setImageSource: image];
    }

    void _SetTextTileMode(const char* text, const char* fontName, float fontSize, const char* colorString, int angle, int horizontalMargin, int verticalMargin)
    {
        NSString* textString = [NSString stringWithUTF8String:text];
        UIFont* font = nil;
        if (fontName != nil)
        {
            NSString* fontNameString = [NSString stringWithUTF8String:fontName];
            font = [UIFont fontWithName:fontNameString size:fontSize];
        }
        NSString* colorStringString = [NSString stringWithUTF8String:colorString];
        [ScreenMarker setTextTileMode: textString font: font colorString: colorStringString angle: angle horizontalMargin: horizontalMargin verticalMargin: verticalMargin];
    }

    void _SetImageTileMode(const char* imageFilePath, int angle, int horizontalMargin, int verticalMargin)
    {
        NSString* imageFilePathString = [NSString stringWithUTF8String:imageFilePath];
        UIImage* image = [UIImage imageWithContentsOfFile:imageFilePathString];
        [ScreenMarker setImageTileMode: image angle: angle horizontalMargin: horizontalMargin verticalMargin: verticalMargin];
    }
}
