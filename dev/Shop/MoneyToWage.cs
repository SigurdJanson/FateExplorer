using System;

namespace FateExplorer.Shop;


public enum LaborClass { Cheap = 1, Simple, Qualified, HighlyQualified }

public static class MoneyToWage
{
    private enum TimePeriod { Day = 1, Week = 7, Month = 30, Year = 365 }


    /// <summary>
    /// Determines the average wage a laborer of the specified labor class earns in the given time period.
    /// </summary>
    /// <param name="labor">The labor class.</param>
    /// <param name="days">The time period a person works in days.</param>
    /// <returns>The expected wage in Silverthalers.</returns>
    /// <exception cref="InvalidOperationException">In case <see cref="LaborClass"/> is not set.</exception>
    public static decimal Wage(LaborClass labor, int days) =>
        labor switch
        {
            LaborClass.Cheap => days * 0.5M,
            LaborClass.Simple => days * (1M + 4M) / 2,
            LaborClass.Qualified => days * (5M + 9M) / 2,
            LaborClass.HighlyQualified => days * (10M + 50M) / 2,
            _ => throw new InvalidOperationException()
        };

    /// <summary>
    /// Expresses the value of an amount of money in terms of the time a laborer has to work for it.
    /// </summary>
    /// <param name="labor">The labor class of the laborer to earn the money.</param>
    /// <param name="wage">The wage in Silverthalers.</param>
    /// <returns>The average in months that it takes to earn the given amount of money.</returns>
    public static double ValueInMonthlyWages(LaborClass labor, decimal wage) =>
        (double)(wage / Wage(labor, (int)TimePeriod.Month));


    /// <summary>
    /// The number of days a laborer needs to earn the given amount of money.
    /// </summary>
    /// <param name="labor">The labor class of the laborer to earn the money.</param>
    /// <param name="wage">The wage in Silverthalers.</param>
    /// <returns></returns>
    public static int DaysToEarn(LaborClass labor, decimal wage) =>
        (int)Math.Ceiling(wage / Wage(labor, 1));


    /// <summary>
    /// The time period a laborer needs to earn the given amount of money.
    /// </summary>
    /// <param name="labor">The labor class of the laborer to earn the money.</param>
    /// <param name="wage">The wage in Silverthalers.</param>
    /// <returns>The time period needed to make that money (in days, months, years).</returns>
    public static (int years, int months, int days) TimeToEarn(LaborClass labor, decimal wage)
    {
        int days = DaysToEarn(labor, wage);
        int years = days / (int)TimePeriod.Year;
        days -= years * (int)TimePeriod.Year;
        int months = days / (int)TimePeriod.Month;
        days -= months * (int)TimePeriod.Month;
        return (years, months, days);
    }
}
