#if STATUSVALUE_UNIRX_SUPPORT
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace StatusValues
{
    public class FloatReactiveStatus : ReactiveStatus<float>
    {
        public FloatReactiveStatus(float baseValue) : base(baseValue)
        {}

        protected override float CalculationStatusCore(IReadOnlyList<IStatusElement<float>> elements)
        {
            return FloatStatusValue.CalculationStatus(baseValue, elements);
        }
    }
}
#endif