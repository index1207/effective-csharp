using System.Collections;

public class CastTest
{
    public void CastListIntToDouble()
    {
        var list = new List<int>() { 1, 2, 3 };
        list.Cast<double>();
    }
}