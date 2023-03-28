using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MentalStatusEffect
{
    public MentalStatus MentalEffect { get; private set; }
    public int EffectMagnitude { get; private set; }

    public void ModifyEffectMangitude(int amountToModify)
    {
        EffectMagnitude += amountToModify;
    }

    public MentalStatusEffect() { }

    public MentalStatusEffect(MentalStatus newMentalEffect, int newEffectMagnitude)
    {
        MentalEffect = newMentalEffect;
        EffectMagnitude = newEffectMagnitude;
    }
}