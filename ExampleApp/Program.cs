// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;

public class Program
{
    const string DbName = "enrollment.db";

    public static async Task Main()
    {
        var dbContext = await DataLayer.QuickDataBuilder.BuildData.GetDbContext(DbName);
        var manger = new Manager(dbContext);
        manger.Entry();
    }
}