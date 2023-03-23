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

    public PawnNeedsStatus(PawnNeedsStatusContainer container = null)
    {
        if(container == null)
        {
            hunger = 10;
            sleep = 10;
            entertainment = 10;
            happiness = 10;
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
                hunger = Mathf.Clamp(hunger + magnitude, 0, 10);
                break;
            case NeedStatus.Sleep:
                sleep = Mathf.Clamp(sleep + magnitude, 0, 10);
                break;
            case NeedStatus.Entertainment:
                entertainment = Mathf.Clamp(entertainment + magnitude, 0, 10);
                break;
            case NeedStatus.Happiness:
                happiness = Mathf.Clamp(happiness + magnitude, 0, 10);
                break;
        }

        //Need some kind of way to update the UI for this.
    }

    public void IncrementAllStatuses()
    {
        hunger = Mathf.Clamp(hunger - 1, 0, 10);
        sleep = Mathf.Clamp(sleep - 1, 0, 10);
        entertainment = Mathf.Clamp(entertainment - 1, 0, 10);

        if(hunger <= 4 )
            happiness = Mathf.Clamp(happiness - 1, 0, 10);
        if (hunger >= 5)
            happiness = Mathf.Clamp(happiness + 1, 0, 10);

        if (sleep <= 4)
            happiness = Mathf.Clamp(happiness - 1, 0, 10);
        if (sleep >= 5)
            happiness = Mathf.Clamp(happiness + 1, 0, 10);

        if (entertainment <= 4)
            happiness = Mathf.Clamp(happiness - 1, 0, 10);
        if (entertainment >= 5)
            happiness = Mathf.Clamp(happiness + 1, 0, 10);
    }
}
