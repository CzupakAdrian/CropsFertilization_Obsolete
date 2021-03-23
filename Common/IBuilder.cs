namespace Common
{
    public interface IBuilder<BuiltType>
    {
        BuiltType Build();
    }
}
