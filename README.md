ScreenMarker Unity Package 사용가이드
===============================

# 1. ScreenMarker Unity Package 불러오기
유니티 프로젝트를 열고, 제공된 ScreenMarker.unitypackage 파일을 더블클릭하여 import 한다.

# 1.1 폴더구조
```
Plugins
└── Android
    ├── screenmarker.aar
    ├── assets
    │   └── fonts
└── IOS
    └── ScreenMarkerIos.mm

Scripts
└── IScreenMarker.cs
└── ScreenMarkerAndroidPlugin.cs
└── ScreenMarkerIOSPlugin.cs
└── ScreenMarkerPlugin.cs
└── ScreenMarkerPluginManager.cs
```

# 1.2 주요파일
## 1.2.1 Scripts
- IScreenMarker.cs : ScreenMarker를 사용하기 위한 인터페이스
- ScreenMarkerAndroidPlugin.cs : Android용 IScreenMarker 구현체
- ScreenMarkerIOSPlugin.cs : IOS용 IScreenMarker 구현체
- ScreenMarkerPlugin.cs : IScreenMarker를 생성하기 위한 클래스. GetScreenMarker()를 통해 IScreenMarker를 생성한다.
- ScreenMarkerPluginManager.cs : ScreenMarkerPlugin을 사용하는 예제 코드. Prefab으로 제공된 ScreenMarkerManager에 추가되어 있음.

## 1.2.2 Plugins
- Android : Android용 ScreenMarker 라이브러리. aar 파일이 위치함.
- Android.assets : Android용 ScreenMarker 라이브러리에 필요한 리소스. 커스텀 폰트를 사용할 경우 assets/fonts 폴더에 추가한다.
- IOS : IOS용 ScreenMarker 라이브러리.
  
## 1.2.3 기타
- Scenes : ScreenMarker를 사용하는 예제 씬이 위치함.
- Prefabs : ScreenMarkerManager 예제 프리팹이 위치함.
- Sprites : ScreenMarkerManager 예제에 사용되는 스프라이트가 위치함.

# 2. ScreenMarker 사용하기
ScreenMarker를 사용하기 위해서는 ScreenMarkerPlugin.cs를 통해 IScreenMarker를 생성하고, 해당 인터페이스를 통해 ScreenMarker를 사용한다. ScreenMarkerPluginManager.cs에는 ScreenMarkerPlugin을 사용하는 예제 코드가 포함되어 있다.


# 3. 사용시 주의사항
- IOS에서 커스텀 폰트 사용을 원하는 경우, 빌드 후 생성되는 Xcode 프로젝트에 Fonts 폴더를 추가하고 원하는 폰트 파일을 추가해야 한다. 이후, Info.plist에 해당 폰트를 추가해야 한다. 필요한 경우 https://tngusmiso.tistory.com/86 를 참고한다.
- IScreenMarker 인스턴스를 ScreenMarkerPlugin.GetScreenMarker()를 통해 생성하고 난 후에는 반드시 InitScreenMarker를 호출하여 초기화한다. Start()에서 호출하는 것을 권장한다.
- 안드로이드와 IOS에서 서로 다른 동작을 원하는 경우 #if UNITY_ANDROID, #if UNITY_IOS를 사용하여 분기처리한다.