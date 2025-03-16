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
as를 사용한다면사 사용자 정의 캐스팅을 사용할 수 없습니다.
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

