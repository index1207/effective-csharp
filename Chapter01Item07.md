# 아이템 7: 델리게이트를 이용하여 콜백을 표현하라

## 델리게이트
함수의 형태를 가리키는 타입으로, 함수형 및 이벤트 기반 프로그래밍에서 자주 사용되는 기법
```csharp
delegate void MyAction();

static void Hello() => Console.WriteLine("Hello");

static void World() => Console.WriteLine("World");

MyAction myAction;
myAction += Hello;
myAction += World;
```

## 콜백
특정 이벤트가 발생했을 때 호출되는 함수로, 
이벤트 기반 프로그래밍에서 자주 사용되는 기법
```csharp
Button button = GetComponent<Button>();
button.onClick.AddListener(() => Debug.Log("콜백 함수가 호출됩니다."));
```
## .NET 프레임워크 제공 델리게이트
- `Action<T>`
    - 형식: void(T1, T2, T3)
    - 리턴값이 없어 단순 호출하기 위한 델리게이트
- `Func<T, TResult>`
    - 형식: TResult(T1, T2, ...)
    - 리턴값이 있는 함수 형식을 저장하기 위한 델리게이트
- `Predicate<T>`
    - 형식: bool(T)
    - 조건에 따라 bool을 리턴하는 델리게이트

## 활용
List나, LINQ에는 콜백을 사용하는 메서드가 있습니다.
```csharp
List<int> numbers = Enumerable.Range(1, 200).ToList();

var oddNumbers = numberList.Find(n => n % 2 == 1); // 홀수만 필터링
var test = numbers.TrueForAll(n => n < 50); // 모든 요소가 50미만일 경우 true 리턴

numbers.RemoveAll(n => n % 2 == 0); // 모든 짝수 제거

numbers.ForEach(item => Console.WriteLine(item)); // 모든 요소 순환환
```
혹은 윈폼이나 유니티와 같이 멀티스레드로 작동할 때 UI는 싱글스레드로 동작하기 때문에 콜백을 사용합니다.

## 멀티캐스트
위의 델리게이트 예제 코드에서와 같이 하나의 델리게이트 객체 안에 여러 콜백을 집어 넣을 수 있습니다.
```csharp
MyAction myAction;
myAction += Hello;
myAction += World;
```
void가 아닌 값을 리턴하는 델리게이트에서 멀티캐스트를 사용하면 문제가 발생합니다.
```csharp
public static StopAt(this List<T> list, Predicate<T> pred)
{
    foreach(var elem in list)
    {
        if (!pred()) // 델리게이트에 여러 함수가 등록되어 있다면 마지막 리턴값만 리턴됨
            break;
    }
}
```
이를 해결하기 위해 모든 리턴값을 확인하는 방식으로 작성해야 합니다.
```csharp
public static StopAt(this List<T> list, Predicate<T> pred)
{
    foreach(var elem in list)
    {
        foreach(var pr in pred.GetInvocationList())
            if(!pr()) // 델리게이트에 등록된 모든 함수 호출
                break;
    }
}
```
