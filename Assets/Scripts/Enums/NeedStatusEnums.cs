[System.Serializable]
public enum NeedStatus
{
    None = 0,
    Hunger = 1 << 0,
    Sleep = 1 << 1,
    Entertainment = 1 << 2,
    Happiness = 1 << 3,
}