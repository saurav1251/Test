namespace Test.Data.SqlHelper
{
    public interface IOutParam<T>
    {
        T Value { get; }
    }
}
