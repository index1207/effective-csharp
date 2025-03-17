# 아이템3: 캐스트보다 is, as가 좋다

## 캐스팅의 필요성
.NET Framework에서 object 타입의 인수를 요구하는 인터페이스가 많아 특정 클래스 또는 타입으로 캐스팅 해야 할 상황이 발생합니다. 이러한 상황일 때 is와 as를 사용해야 하는 이유를 설명합니다.
```csharp
class ClassA
{
    // object 타입을 요구하는 메서드(Equals, CompareTo 등)
    public bool Equals(object other)
    {
        ...
    }
}
```


## C-Sytle 캐스팅 VS as 캐스팅
- as를 사용해야 하는 이유: <br>
사용해 캐스팅하게 된다면 캐스팅 실패시 null을 리턴해 더 안전하고, 런타임에 더 효율적으로 작동합니다. 반면 c-style 캐스팅을 하게 된다면 캐스팅에 실패하면 `InvalidCastException`이 발생해 크래시가 발생할 수 있습니다.

- 그럼에도 c-style 캐스팅을 사용해야 할 상황: <br>
as를 사용한다면 사용자 정의 캐스팅을 사용할 수 없습니다.
    ```csharp
    class PP_Amount
    {
        ...
        // is, as 사용 불가
        public static explicit operator PP_Amount(long value) => new PP_Amount(value);
    }
    ```

## 캐스팅 실패 처리
- 아래는 2가지 방식 캐스팅의 비교 입니다.
    ```csharp
    /* as를 사용한 캐스팅 */
    object obj = Factory.GetObject(); // 팩토리에서 객체 생성

    MyType t = obj as MyType;
    if (t != null) // obj가 null이면 t도 null
    {
        // 성공했을 경우
    }
    else
    {
        // 실패했을 경우
    }
    ```
    ```csharp
    /* c-style 캐스팅을 사용한 예시 */
    object obj = Factory.GetObject(); // 팩토리에서 객체 생성

    try
    {
        // obj가 null인지 확인 필요
        MyType t = (MyType)obj;
    }
    catch(InvalidCastException)
    {
        // 실패했을 경우
    }
    ```
- as를 사용했을 때의 장점은 코드 가독성이 좋고, try-catch가 없어 성능도 비교적 좋습니다.
    - throw가 되면 콜스택을 추적하는 **스택 언와인딩(stack unwinding)**이 발생해 if를 사용하는것보다 훨씬 큰 오버헤드가 발생합니다.
- c-style 캐스팅은 try에서 성공했더라도 `obj`가 null이여도 성공하게 되어 추가로 null체크를 해야 합니다.

## 캐스팅의 제약
- is, as를 사용해 객체를 다른 타입으로 캐스팅하기 위해서는 상속한 타입이거나 지정한 타입이어야 합니다.
    - 그 외의 경우는 실패처리가 됩니다.
- c-style 캐스팅의 경우 사용자 정의 형변환 연산자가 개입될 수도 있습니다.
```csharp
class MyType {}

class SecondType
{
    private MyType _value;

    public static implicit operator MyType(SecondType t) => t._value;
}

object obj = new SecondType();

MyType t = obj as MyType; // 결과: 실패
if (t != null)
{
    System.Console.WriteLine("Success");
}
else
{
    System.Console.WriteLine("Fail");
}

try
{
    t = (MyType)obj; // 결과: 실패
    System.Console.WriteLine("Success");
}
catch(InvalidCastException)
{
    System.Console.WriteLine("Fail");
}
```
- 위 코드에서 두 번째 방식은 성공해야 할 것처럼 보이지만 실패해 throw됩니다.
    - 컴파일러는 컴파일 타임에 객체가 어떤 타입으로 선언되었는지만 추적합니다.
    - 런타임에 `obj`가 `object`타입으로 판단하고, 사용자 정의 형변환 연산자가 없기 때문에 실패하게 됩니다.

## as를 사용해야 하는 이유
```csharp
// 1. c-style 캐스팅
var t = (MyType)obj;

// 2. as 캐스팅
var t = obj as MyType;
```
- 첫 번째 방식의 경우 사용자 정의 형변환 연산자가 정의 되어 있다면, `obj`의 타입에 따라 결과는 다르게 나올 수 있습니다.
- 두 번째 방식의 경우 `obj`가 어떤 타입으로 선언되든 결과는 항상 같습니다.
- as를 사용하면 코드의 일관성이 높아 c-style 캐스팅보다 as를 사용하는 것이 좋습니다.

## as를 사용할 수 없는 경우
```csharp
object obj = new Factory.GetValue();
int i = obj as int;
```
- 위의 경우 int는 값 타입이기 때문에 null을 리턴하는 as연산에서 컴파일에 실패하게 됩니다.
```csharp
object obj = new Factory.GetValue();
int? i = obj as int?;
if (i != null)
    Console.WriteLine(i.Value)
```
- 위와 같이 nullable타입을 사용해 null인지 확인한 후 처리하는 방식이 있습니다.

## foreach에서의 캐스팅
- foreach는 `IEnumerable`인터페이스를 사용하고, 구현하는 과정에서 캐스팅이 사용됩니다.
    - 타입 안정성을 위해 `IEnumerable<T>`를 사용할 수도 있겠지만, 하위 호환성을 위해 late binding을 사용하는 `IEnumerable`도 지원합니다.
- 값타입과 참조 타입에 대한 캐스팅을 지원해야 하는데 c-style 캐스팅을 사용하면 타입을 구분할 필요가 없어집니다.
    - 대신 캐스팅이 실패하면 `InvalidCastException`이 발생합니다.
- 아래는 foreach의 작동 방식에 대한 코드입니다.
    ```csharp
    IEnumerable iter = collection.GetEnumerator();
    while (iter.MoveNext())
    {
        MyType t = (MyType)iter.Current; // Current는 System.Object입니다.
        ...
    }
    ```
- foreach에서 잘못된 타입으로 순환하는 경우에도 `InvalidCastException`이 발생합니다.
    ```csharp
    IEnumerable iter = new List<int>() { 1, 2, 3, };
    foreach (double d in iter)
    {
        // throw됨
    }
    ```
- 다형성 규칙을 준수해 아래와 같은 코드도 동작합니다.
    ```csharp
    class MyType {}
    class NewType : MyType {}

    IEnumerable iter = new List<NewType>() {...}
    foreach (MyType t in iter)
    {
        // 정상적으로 작동함
    }
    ```
# LINQ
- LINQ에는 특정 타입으로 캐스팅하는 메서드 `Cast<T>`가 있습니다.
    - as는 캐스팅에 제약이 있기 때문에 `Cast<T>`에서는 c-style 캐스팅을 사용합니다.
    - 따라서 제네릭 타입으로 사용자 정의 타입을 넣는다면 캐스팅에 문제가 없는지 확인이 필요합니다.
- 제네릭 컬렉션에서는 `Cast<T>`를 호출할 수 없습니다.
    - 아래는 List<int>에서 Cast<double>을 호출했을 때 발생하는 경고입니다.
    ![](image/CastT.png)