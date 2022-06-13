using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MVCUser.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcUserContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcUserContext>>()))
                    {
                        // Look for any users.
                        if (context.Users.Any())
                        {
                            return;   // DB has been seeded
                        }

                        context.Users.AddRange(
                            new Users
                            {
                                username = "SogoWhat",
                                password = "A1233456"
                            },

                            new Users
                            {
                                username = "Tosky",
                                password = "B123456"
                            }
                        );
                        context.SaveChanges();
                    }
        }
    }
}