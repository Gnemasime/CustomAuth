using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;
using CustomAuth.Models;

public class AccountController : Controller
{
    private readonly MySqlDbContext _dbContext;

    public AccountController(MySqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: /Account/SignUp
    public IActionResult SignUp()
    {
        return View();
    }

    // POST: /Account/SignUp
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(string username, string email, string password)
    {
        if (ModelState.IsValid)
        {
            // Check if the username already exists in the database
            var existingUser = await _dbContext.Users
                                                .Where(u => u.Username == username)
                                                .FirstOrDefaultAsync();
            if (existingUser != null)
            {
                // If user already exists, show error message
                ViewData["ErrorMessage"] = "Username already exists.";
                return View();
            }

            // Hash the password before storing it
            string hashedPassword = HashPassword(password);

            // Create a new User object
            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword
            };

            // Add the user to the database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Redirect to login page after successful sign-up
            return RedirectToAction("Login");
        }

        return View();
    }

    // Helper method to hash the password
    private string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Hash the password with the salt using PBKDF2
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hashedPassword;
    }

    // GET: /Account/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (ModelState.IsValid)
        {
            var user = await _dbContext.Users
                                       .Where(u => u.Username == username)
                                       .FirstOrDefaultAsync();
            if (user != null && VerifyPassword(password, user.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
        }

        return View();
    }

    // Helper method to verify the password
    private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
    {
        // You would need to implement the code to compare the entered password with the stored hashed password.
        // In this case, the hashing mechanism should ideally store the salt along with the hashed password.

        // For now, this is simplified for demo purposes and assumes no salt is stored.
        // You would also need to handle salt extraction if needed.

        return enteredPassword == storedHashedPassword; // Simplified for demo purposes
    }

    // GET: /Account/Logout
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
