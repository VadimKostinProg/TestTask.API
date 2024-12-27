namespace TestTask.Utils.DTO;

public class PopularityReportLineDto
{
    public int Year { get; set; }

    public string ItemName { get; set; }

    public int MaxDailyPurchase { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not PopularityReportLineDto other) return false;
        return ItemName == other.ItemName &&
               Year == other.Year &&
               MaxDailyPurchase == other.MaxDailyPurchase;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ItemName, Year, MaxDailyPurchase);
    }
}
