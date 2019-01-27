using System.Collections.Generic;
using CoreNG.Common.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoreNG.Persistence
{
    public static class DbSeeder
    {
        public static void Seed(IDbContext context)
        {
            if (!context.Categories.Any())
            {
                var lst = new List<string>()
                {
                    "Utilities",
                    "Phone",
                    "Mobile",
                    "Internet",
                    "Power",
                    "Water",
                    "Education",
                    "Insurance",
                    "Fuel",
                    "Tyres",
                    "Service"
                };

                foreach (var category in lst)
                {
                    var c = new Category();
                    c.Name = category;
                    context.Categories.Add(c);
                }

                context.SaveChanges();

            }
        }
    }
}