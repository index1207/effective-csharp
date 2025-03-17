using System.Collections;

class Program
{
    static void Main(String[] args)
    {
        var list = new List<int>() { 1, 2, 3 };
        list.Cast<double>();
    }
}

