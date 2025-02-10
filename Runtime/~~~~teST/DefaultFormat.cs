public class DefaultFormat : ScoreFormat
{
    public override string GetValueStringFormatted(float value, bool hasBeenSet)
    {
        return !hasBeenSet ? "0" : $"{(int)value}";
    }
}