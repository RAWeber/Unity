using UnityEngine;
using System.Collections;

public class TotalPercentMod : StatModifier {


    public override int Order
    {
        get { return 3; }
    }

    public override int ApplyModifier(int statValue, float modValue)
    {
        return (int)(modValue);
    }

    public TotalPercentMod(float value, bool stacks) : base(value, stacks) { }
}
