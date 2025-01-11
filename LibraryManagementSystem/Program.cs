using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace LibraryManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<LibraryDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<LibraryDbContext>();

            var app = builder.Build();



            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Tworzenie ról, je?li nie istniej?
                var roles = new[] { "Administrator", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Tworzenie konta administratora
                var adminEmail = "admin@admin.com";
                var adminPassword = "Admin@123";

                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);
                    if (createAdminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Administrator");
                        Console.WriteLine("Utworzono konto administratora.");
                    }
                    else
                    {
                        Console.WriteLine("B??dy podczas tworzenia administratora:");
                        foreach (var error in createAdminResult.Errors)
                        {
                            Console.WriteLine($"- {error.Description}");
                        }
                    }
                }

                // Tworzenie kont testowych
                var testAccounts = new[]
                {
                    new { Email = "test1@library.com", Password = "Test1@123" },
                    new { Email = "test2@library.com", Password = "Test2@123" }
                };

                foreach (var account in testAccounts)
                {
                    var testUser = await userManager.FindByEmailAsync(account.Email);
                    if (testUser == null)
                    {
                        testUser = new IdentityUser
                        {
                            UserName = account.Email,
                            Email = account.Email,
                            EmailConfirmed = true
                        };

                        var createTestUserResult = await userManager.CreateAsync(testUser, account.Password);
                        if (createTestUserResult.Succeeded)
                        {
                            await userManager.AddToRoleAsync(testUser, "User");
                            Console.WriteLine($"Utworzono konto testowe: {account.Email}");
                        }
                        else
                        {
                            Console.WriteLine($"B??dy podczas tworzenia konta {account.Email}:");
                            foreach (var error in createTestUserResult.Errors)
                            {
                                Console.WriteLine($"- {error.Description}");
                            }
                        }
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapRazorPages();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
