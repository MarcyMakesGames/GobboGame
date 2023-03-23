[System.Serializable]
public enum PhysicalStatus
{
    None = 0,
    Hungry = 1 << 0,
    Tired = 1 << 1,
    Hurt = 1 << 2,
}

