using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StatusValues
{
    public abstract class StatusValue<TValue, TElement> : IReadOnlyStatusValue<TValue> ,IStatusCalculable<TElement>
    {
        protected TValue baseValue;
        private TValue value;
        private readonly List<IStatusElement<TElement>> elements = new();

        public TValue Value => value;

        public TValue BaseValue
        {
            get => baseValue;
            set
            {
                baseValue = value;
                CalculationStatus();
            }
        }

        public StatusValue(TValue baseValue)
        {
            this.baseValue = baseValue;
            value = this.baseValue;
        }

        private TValue CalculationStatus() => value = CalculationStatusCore(elements);

        protected abstract TValue CalculationStatusCore(IReadOnlyList<IStatusElement<TElement>> elements);
        
        public virtual bool RemoveElement(IStatusElement<TElement> element)
        {
            var result = elements.Remove(element);
            CalculationStatus();
            return result;
        }
        
        public virtual void AddElement(IStatusElement<TElement> element)
        {
            if(elements.Contains(element)) return;
            elements.Add(element);
            CalculationStatus();
        }

        public virtual void ClearElements()
        {
            elements.Clear();
            CalculationStatus();
        }

        public static implicit operator TValue(StatusValue<TValue, TElement> self)
        {
            return self.Value;
        }
    }
    
    public abstract class StatusValue<T> : StatusValue<T,T>
    {
        protected StatusValue(T baseValue) : base(baseValue)
        {}
    }
}
