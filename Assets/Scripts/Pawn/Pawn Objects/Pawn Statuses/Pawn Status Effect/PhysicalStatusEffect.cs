using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysicalStatusEffect
{
    private PhysicalStatus physicalEffect;
    private int effectMagnitude;

    public PhysicalStatus PhysicalEffect { get => physicalEffect; }
    public int EffectMagnitude { get => effectMagnitude; }

    public PhysicalStatusEffect() { }

    public PhysicalStatusEffect(PhysicalStatus newPhysicalEffect, int newEffectMagnitude)
    {
        physicalEffect = newPhysicalEffect;
        effectMagnitude = newEffectMagnitude;
    }

    public void ModifyEffectMangitude(int amountToModify)
    {
        effectMagnitude += amountToModify;
    }
}
