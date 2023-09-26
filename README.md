# StatusValue

RPGやローグライクなど、キャラクターのステータス値を管理するのに役立ちます。

> 例: (元の攻撃力(100) + 攻撃力バフ(20)) * 攻撃力バフ(1.5倍) → 最終的な攻撃力(180)

```csharp

// 元の攻撃力100
var attack = new FloatStatusValue(100);
Debug.Log(attack.BaseValue); // 100
Debug.Log(attack.Value); // 100

// +20のバフを与える
var buff1 = new FloatStatusElement(20, CalculationType.Additive);
attack.AddElement(buff1);
Debug.Log(attack.Value); // 120

// 1.5倍のバフを与える
var buff2 = new FloatStatusElement(1.5f, CalculationType.Multiply);
attack.AddElement(buff2);
Debug.Log(attack.Value); // 180

// 0に固定するデバフ
var debuff = new FloatStatusElement(0, CalculationType.Const);
attack.AddElement(debuff);
Debug.Log(attack.Value); // 0

// デバフを外す
attack.RemoveElement(debuff);
```

## Value
計算された最終的な値を返します。(read-only)

## BaseValue 
もとの値を返します。書き換え可能で `Value` にも反映されます。

## CalculationType
BaseValueの加工方法を選択できます。
```csharp
public enum CalculationType
{
    Additive, Multiply, Const
}
```
- `Additive` BaseValue に値を足します
- `Multiply` BaseValue に値を掛けます
- `Const`    BaseValue の値を固定します


## IStatusElement<TElement>
`BaseValue` を加工するためのインターフェースです。

```csharp
public interface IStatusElement<T>
{
    CalculationType CalculationType{get;}
    T Value{get;}
}
```

`StatusValue` に `AddElement` や `RemoveElement` を呼ぶことで付け外しができます。
```csharp
var status = new IntStatusValue(100);

var element = new FloatStatusElement(20, CalculationType.Additive);

status.AddElement(element);
status.RemoveElement(element);
```

## ReactiveStatus

StatusValueに `UniRx` の `ReactiveProperty` の効果をプラスしたようなオブジェクトです。
`Value` の値を購読できます。
この機能を有効にするには Scripting Define Symbols に `STATUSVALUE_UNIRX_SUPPORT` を追加してください

```csharp
var reactiveStatus = new FloatReactiveStatus(100);

reactiveStatus.Subscribe(v => Debug.Log(v));

reactiveStatus.BaseValue = 200;         // onNext
reactiveStatus.AddElement(element);     // onNext
reactiveStatus.RemoveElement(element);  // onNext

reactiveStatus.Dispose();
```
`IReadOnlyReactiveProperty<T>`を実装しているので読み取り専用の ReactiveProperty として外部に公開することもできます。
```csharp
class PlayerStatus
{
    private readonly IntReactiveProperty hp = new();
    public IReadOnlyReactiveProperty<int> HP => hp;
}
```

## カスタマイズ

標準では `FloatStatusValue` と `IntStatusValue` を提供しています。
標準の計算方法や機能をカスタマイズしたい場合は `StatusValue<T>` または `StatusValue<TValue, TElement>` を実装して独自のクラスを作成できます。
`CalculationStatusCore`メソッドをオーバーライドすれば計算ロジックを自由に記述できます。
`CalculationStatusCore`メソッドの返り値はそのまま `Value` に代入されます。

以下の例は FloatStatusValue に 最小値と最大値を設定できるようにカスタマイズしたクラスです。
```csharp
public class FloatLimitedStatusValue : StatusValue<float>
{
    private readonly float min;
    private readonly float max;
    
    // 最小値と最大値を設定できるようなカスタマイズ
    public FloatLimitedStatusValue(float baseValue, float min, float max) : base(baseValue)
    {
        this.min = min;
        this.max = max;
    }
    
    protected override float CalculationStatusCore(IReadOnlyList<IStatusElement<float>> elements)
    {
        var value = FloatStatusValue.CalculationStatus(baseValue, elements);
        // 値をClampする。
        return Mathf.Clamp(value, min, max);
    }
}
```
`ReactiveStatus` でも同様にカスタマイズできます。

## ライセンス
MIT License