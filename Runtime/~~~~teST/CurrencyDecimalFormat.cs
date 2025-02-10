public class CurrencyDecimalFormat : ScoreFormat
{
    public override string GetValueStringFormatted(float value, bool hasBeenSet)
    {
        return !hasBeenSet ? "$0.00" : $"${value:F2}";
    }
}