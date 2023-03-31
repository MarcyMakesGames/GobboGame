using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PawnNeedsStatus
{
    [SerializeField] public int hunger = 10;
    [SerializeField] public int sleep = 10;
    [SerializeField] public int entertainment = 10;
    [SerializeField] public int happiness = 10;
    [SerializeField] public int statMax = 10;

    public PawnNeedsStatus(PawnNeedsStatusContainer container = null)
    {
        if(container == null)
        {
            hunger = statMax;
            sleep = statMax;
            entertainment = statMax;
            happiness = statMax;
        }
        else
        {
            hunger = container.hunger;
            sleep = container.sleep;
            entertainment = container.entertainment;
            happiness = container.happiness;
        }
    }

    public void UpdateNeedsStatus(NeedStatus need, int magnitude)
    {
        switch (need)
        {
            case NeedStatus.None:
                break;
            case NeedStatus.Hunger:
                hunger = Mathf.Clamp(hunger + magnitude, 0, statMax);
                if (magnitude >= 1 && happiness < 4)
                    happiness++;
                break;
            case NeedStatus.Sleep:
                sleep = Mathf.Clamp(sleep + magnitude, 0, statMax);
                if (magnitude >= 1 && happiness < 4)
                    happiness++;
                break;
            case NeedStatus.Entertainment:
                entertainment = Mathf.Clamp(entertainment + magnitude, 0, statMax);
                if (magnitude >= 1 && happiness < 4)
                    happiness++;
                break;
            case NeedStatus.Happiness:
                happiness = Mathf.Clamp(happiness + magnitude, 0, statMax);
                if (magnitude >= 1 && happiness < 4)
                    happiness++;
                break;
        }

        //Need some kind of way to update the UI for this.
    }

    public void IncrementAllStatuses()
    {
        hunger = Mathf.Clamp(hunger - 1, 0, statMax);
        sleep = Mathf.Clamp(sleep - 1, 0, statMax);
        entertainment = Mathf.Clamp(entertainment - 1, 0, statMax);

        if(hunger <= 4 )
            happiness = Mathf.Clamp(happiness - 1, 0, statMax);
        if (hunger >= 7)
            happiness = Mathf.Clamp(happiness + 1, 0, statMax);

        if (sleep <= 4)
            happiness = Mathf.Clamp(happiness - 1, 0, statMax);
        if (sleep >= 7)
            happiness = Mathf.Clamp(happiness + 1, 0, statMax);

        if (entertainment <= 4)
            happiness = Mathf.Clamp(happiness - 1, 0, statMax);
        if (entertainment >= 7)
            happiness = Mathf.Clamp(happiness + 1, 0, statMax);
    }
}
