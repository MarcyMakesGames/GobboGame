using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PawnStatusContainer
{
    public string FirstName { get; set; }
    public string NickName { get; set; }
    public string LastName { get; set; }
    public PawnMentalStatusContainer MentalStatusContainer { get; set; }
    public PawnPhysicalStatusContainer PhysicalStatusContainer { get; set; }
    public PawnNeedsStatusContainer NeedsStatusContainer { get; set;}

    public PawnStatusContainer () { }
}

[System.Serializable]
public class PawnMentalStatusContainer
{
    public int health { get; set; }
    public List<MentalStatusEffect> statusEffects { get; set; }

    public PawnMentalStatusContainer() { }
}

[System.Serializable]
public class PawnPhysicalStatusContainer
{
    public int health { get; set; }
    public List<PhysicalStatusEffect> statusEffects { get; set; }

    public PawnPhysicalStatusContainer() { }
}

[System.Serializable]
public class PawnNeedsStatusContainer
{
    public int hunger { get; set; }
    public int sleep { get; set; }
    public int entertainment { get; set; }
    public int happiness { get; set; }

    public PawnNeedsStatusContainer() { }
}
