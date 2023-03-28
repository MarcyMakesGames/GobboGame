using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PawnPhysicalStatus
{
    [SerializeField] private int health;
    [SerializeField] private List<PhysicalStatusEffect> statusEffects;

    public int PhysicalHealth { get => health; set => health = value; }
    public List<PhysicalStatusEffect> PhysicalStatusEffects { get => statusEffects; set => statusEffects = value; }

    public PawnPhysicalStatus(PawnPhysicalStatusContainer statusContainer = null)
    {
        if(statusContainer == null)
        {
            health = 100;
            statusEffects = new List<PhysicalStatusEffect>();
        }
        else
        {
            health = statusContainer.health;
            statusEffects = new List<PhysicalStatusEffect>();

            foreach(PhysicalStatusEffect effect in statusContainer.statusEffects)
            {
                statusEffects.Add(effect);
            }
        }
    }

    public void UpdatePhysicalStatus(PhysicalStatus newStatus, int magnitude)
    {
        foreach (PhysicalStatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.PhysicalEffect == newStatus)
            {
                statusEffect.ModifyEffectMangitude(magnitude);

                if (statusEffect.EffectMagnitude <= 0)
                    statusEffects.Remove(statusEffect);

                return;
            }
        }

        PhysicalStatusEffect newEffect = new PhysicalStatusEffect(newStatus, magnitude);

        statusEffects.Add(newEffect);
    }

    public void IncrementAllStatuses()
    {
        foreach (PhysicalStatusEffect statusEffect in statusEffects)
        {
            statusEffect.ModifyEffectMangitude(-1);
        }
    }
}
