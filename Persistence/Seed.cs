using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (context.Categories.Any()) return;

            var categories = new List<Category>
            {
                new Category
                {
                    Description = "Еда"
                },
                new Category
                {
                    Description = "Вкусности"
                },
                new Category
                {
                    Description = "Вода"
                }
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            if (context.Products.Any()) return;

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Селедка",
                    CategoryId = 1,
                    Description = "Селедка соленая",
                    Cost = 10000,
                    GeneralNote = "Акция",                    
                    SpecialNote = "Пересоленая"                   
                },
                new Product
                {
                    Name = "Тушенка",
                    CategoryId = 1,
                    Description = "Тушенка Говяжая",
                    Cost = 20000,
                    GeneralNote = "Вкусная",                    
                    SpecialNote = "Жилы",                   
                },
                new Product
                {
                    Name = "Сгущенка",
                    CategoryId = 2,
                    Description = "В банках",
                    Cost = 30000,
                    GeneralNote = "С ключом",                    
                    SpecialNote = "Вкусная"                   
                },
                new Product
                {
                    Name = "Квас",
                    CategoryId = 3,
                    Description = "В бутылках",
                    Cost = 15000,
                    GeneralNote = "Вятский",                    
                    SpecialNote = "Теплый"                   
                }
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            if(roleManager.Roles.Any()) return;

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "user"
                },
                new IdentityRole
                {
                    Name = "advanced"
                },
                new IdentityRole
                {
                    Name = "admin"
                }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            if (userManager.Users.Any()) return;

            var user = new AppUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
            };

            await userManager.CreateAsync(user, "Admin123@");
            await userManager.AddToRoleAsync(user, "admin");

        }
    }
}