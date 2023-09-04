#if UNIRX_REACTIVESTATUS_SUPPORT
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace StatusValues
{
    [System.Serializable]
    public class IntReactiveStatus : ReactiveStatus<int, float>
    {
        protected override int CalculationStatusCore(IReadOnlyList<IStatusElement<float>> elements)
        {
            return IntStatusValue.CalculationStatus(baseValue, elements);
        }

        public IntReactiveStatus(int baseValue) : base(baseValue)
        {}
    }
}
#endif