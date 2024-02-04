ScreenMarker IOS 플러그인 빌드 가이드
=============
# 1. ScreenMarkerIOS.mm 제작하기
ScreenMarker와 관련된 모든 파일의 내용을 ScreenMarkerIOS.mm 파일에 복사/붙여넣기합니다. 헤더 -> 구현 파일 순으로 복사/붙여넣기를 진행합니다. 이때, 파일 의존 관계에 주의하여 utils와 같이 더 먼저 import 되어야하는 코드를 먼저 붙여넣어야 합니다.


# 2. IOS 네이티브 플러그인 추가하기.
유니티의 Plugins/IOS 폴더에 앞서만든 ScreenMarkerIOS.mm 파일을 추가하면 별도의 설정없이 해당 파일의 내용을 사용할 수 있습니다.

# 3. 유니티 연동
## 3.1. 연동을 위한 함수 extern "C" 추가하기
ScreenMarkerIOS.mm 파일에 아래 예시와 같이 외부에 노출시킬 함수는 extern "C"로 노출시킵니다. 문법은 objective-c 그대로입니다.
```c
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
}
```

## 3.2. 유니티 C# 스크립트 작성
```csharp
class ScreenMarkerIOSPlugin : MonoBehaviour, IScreenMarker
{
    [DllImport("__Internal")]
    private static extern void _InitScreenMarker(string userInfo);
    [DllImport("__Internal")]
    private static extern void _ShowScreenMarker();
    [DllImport("__Internal")]
    private static extern void _HideScreenMarker();
}
```
위 코드와 같이 DllImport를 통해 외부 함수를 호출할 수 있습니다. 이때, extern "C"로 노출시킨 함수명과 파라미터는 동일해야합니다. 다만, 여기서는 c#의 문법을 사용하므로 const char* 대신 string으로 파라미터를 받는 등, C#의 문법으로 함수 시그니처를 작성합니다.
