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
        private readonly Subject<TValue> valueChanged = new();

        public bool HasValue => true;

        public ReactiveStatus(TValue baseValue) : base(baseValue){}
        
        public override bool RemoveElement(IStatusElement<TElement> element)
        {
            var before = Value;
            var result = base.RemoveElement(element);
            var after = Value;
            if(!before.Equals(after) && !isDisposed)
            {
                valueChanged.OnNext(after);
            }

            return result;
        }
        
        public override void AddElement(IStatusElement<TElement> element)
        {
            var before = Value;
            base.AddElement(element);
            var after = Value;
            if(before.Equals(after) || isDisposed) return;
            
            valueChanged.OnNext(after);
        }

        public override void ClearElements()
        {
            var before = Value;
            base.ClearElements();
            var after = Value;
            if (before.Equals(after) || isDisposed) return;

            valueChanged.OnNext(after);
        }

        public void ForceNotify()
        {
            if (isDisposed) return;
            valueChanged.OnNext(Value);
        }
        
        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            if (isDisposed)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }
            // raise latest value on subscribe
            observer.OnNext(Value);
            return valueChanged.Subscribe(observer);
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
            valueChanged?.Dispose();
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