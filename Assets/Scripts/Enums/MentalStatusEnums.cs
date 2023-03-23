[System.Serializable]
public enum MentalStatus
{
    None = 0,
    Hungry = 1 << 0,
    Tired = 1 << 1,
    Headache = 1 << 2,
}