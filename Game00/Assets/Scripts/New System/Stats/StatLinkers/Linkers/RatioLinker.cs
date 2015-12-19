using UnityEngine;
using System.Collections;
using System;

public class RatioLinker : StatLinker
{
    private float ratio;

    public override int Value
    {
        get { return (int)(LinkedStat.BaseValue*ratio); }
    }

    public RatioLinker(BaseStat stat, float ratio) : base(stat)
    {
        this.ratio = ratio;
    }
}
