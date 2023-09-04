namespace StatusValues
{
    public interface IStatusCalculable<T>
    {
        bool RemoveElement(IStatusElement<T> element);
        void AddElement(IStatusElement<T> element);
    }
}
