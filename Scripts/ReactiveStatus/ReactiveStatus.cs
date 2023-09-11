#if UNIRX_REACTIVESTATUS_SUPPORT
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace StatusValues
{
    public abstract class ReactiveStatus<TValue, TElement> : StatusValue<TValue, TElement>, IDisposable, IReadOnlyReactiveProperty<TValue>
    {
        private bool isDisposed;
        private readonly ReactiveProperty<TValue> innerProperty = new();

        public bool HasValue => innerProperty.HasValue;

        public ReactiveStatus(TValue baseValue) : base(baseValue)
        {
            innerProperty.Value = baseValue;
        }
        
        public override bool RemoveElement(IStatusElement<TElement> element)
        {
            var result = base.RemoveElement(element);
            innerProperty.Value = Value;
            return result;
        }
        
        public override void AddElement(IStatusElement<TElement> element)
        {
            base.AddElement(element);
            innerProperty.Value = Value;
        }

        public override void ClearElements()
        {
            base.ClearElements();
            innerProperty.Value = Value;
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            return innerProperty.Subscribe(observer);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;
            isDisposed = true;
            innerProperty?.Dispose();
        }

        public static implicit operator TValue(ReactiveStatus<TValue, TElement> self)
        {
            return self.Value;
        }
    }
    
    public abstract class ReactiveStatus<T> : ReactiveStatus<T,T>
    {
        protected ReactiveStatus(T baseValue) : base(baseValue)
        {}
    }
}
#endif