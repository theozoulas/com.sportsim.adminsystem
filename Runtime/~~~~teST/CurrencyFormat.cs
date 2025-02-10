public class CurrencyFormat : ScoreFormat
{
    public override string GetValueStringFormatted(float value, bool hasBeenSet)
    {
        return !hasBeenSet ? "$0" : $"${value}";
    }
}