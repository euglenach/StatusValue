﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StatusValues
{
    public class FloatStatusValue : StatusValue<float>
    {
        public FloatStatusValue(float baseValue) : base(baseValue)
        {}

        protected override float CalculationStatusCore(IReadOnlyList<IStatusElement<float>> elements)
        {
            return CalculationStatus(baseValue, elements);
        }

        public static float CalculationStatus(float baseValue, IReadOnlyList<IStatusElement<float>> elements)
        {
            if(!elements.Any()) return baseValue;
            
            // 定数があったらそのまま返す
            var constValue = elements.FirstOrDefault(e => e.CalculationType is CalculationType.Const);
            if(constValue is not null) return constValue.Value;
            
            // 乗算部分だけで足し算しておく
            var multiSum = elements.Where(e => e.CalculationType is CalculationType.Multiply)
                                   .Sum(e => e.Value);
            if(!elements.Any(e => e.CalculationType is CalculationType.Multiply))
            {
                multiSum = 1;
            }
            
            // 加算部分と足す
            var addSum = elements.Where(e => e.CalculationType is CalculationType.Additive)
                                 .Sum(e => e.Value);

            return (baseValue + addSum) * multiSum;
        }
    }
}
