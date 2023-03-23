using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PawnMentalStatus
{
    [SerializeField] int health;
    [SerializeField] private List<MentalStatusEffect> statusEffects;

    public int MentalHealth { get => health; set => health = value; }
    public List<MentalStatusEffect> MentalStatusEffects { get => statusEffects; set => statusEffects = value; }

    public PawnMentalStatus(PawnMentalStatusContainer statusContainer = null)
    {
        if (statusContainer == null)
        {
            health = 100;
            statusEffects = new List<MentalStatusEffect>();
        }
        else
        {
            health = statusContainer.health;
            statusEffects = new List<MentalStatusEffect>();

            foreach (MentalStatusEffect effect in statusContainer.statusEffects)
            {
                statusEffects.Add(effect);
            }
        }
    }

    public void UpdateMentalStatus(MentalStatus newStatus, int magnitude)
    {
        foreach (MentalStatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.MentalEffect == newStatus)
            {
                statusEffect.ModifyEffectMangitude(magnitude);

                if (statusEffect.EffectMagnitude <= 0)
                    statusEffects.Remove(statusEffect);

                return;
            }
        }

        MentalStatusEffect newEffect = new MentalStatusEffect(newStatus, magnitude);

        statusEffects.Add(newEffect);
    }

    public void IncrementAllStatuses()
    {
        foreach (MentalStatusEffect statusEffect in statusEffects)
        {
            statusEffect.ModifyEffectMangitude(-1);
        }
    }
}
