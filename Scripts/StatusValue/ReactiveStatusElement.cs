namespace StatusValues
{
    public enum CalculationType
    {
        Additive,Multiply,Const
    }

    public interface IStatusElement<T>
    {
        CalculationType CalculationType{get;}
        T Value{get;}
    }
    
    public class FloatStatusElement : IStatusElement<float>
    {
        public CalculationType CalculationType{get;}
        public float Value{get;}

        public FloatStatusElement(float value,CalculationType calculationType)
        {
            CalculationType = calculationType;
            Value = value;
        }
    }

    public class IntStatusElement : IStatusElement<int>
    {
        public CalculationType CalculationType { get; }
        public int Value { get; }

        public IntStatusElement(int value, CalculationType calculationType)
        {
            CalculationType = calculationType;
            Value = value;
        }
    }
}