using System;

namespace FateExplorer.Shop;


public enum LaborClass { Cheap = 1, Simple, Qualified, HighlyQualified }

public static class MoneyToWage
{
    private const int DaysPerMonth = 30;
    private enum TimePeriod { Day = 1, Week = 7, Month = 30, Year = 365 }

    public static decimal Wage(LaborClass labor, int days) =>
        labor switch
        {
            LaborClass.Cheap => days * 0.5M,
            LaborClass.Simple => days * (1M + 4M) / 2,
            LaborClass.Qualified => days * (5M + 9M) / 2,
            LaborClass.HighlyQualified => days * (10M + 50M) / 2,
            _ => throw new InvalidOperationException()
        };

    public static double ValueInMonthlyWages(LaborClass labor, decimal wage) =>
        (double)(wage / Wage(labor, DaysPerMonth));


}
