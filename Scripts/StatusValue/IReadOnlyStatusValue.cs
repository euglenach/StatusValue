namespace StatusValues
{
    public interface IReadOnlyStatusValue<TValue>
    {
        TValue Value{get;}
    }
}
