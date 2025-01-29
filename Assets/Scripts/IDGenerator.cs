public static class IDGenerator
{
    private static uint _idCounter = 0;

    public static uint GenerateId()
    {
        return _idCounter++;
    }
}