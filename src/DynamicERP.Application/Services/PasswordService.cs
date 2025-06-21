using BCrypt.Net;
using DynamicERP.Core.Constants;
using DynamicERP.Core.Interfaces.Services;
using System.Text.RegularExpressions;

namespace DynamicERP.Application.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Şifre boş olamaz", nameof(password));

            // BCrypt ile şifreyi hash'le (work factor: 12)
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        
        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Hash formatı geçersiz
                return false;
            }
        }

        public (bool IsValid, string ErrorMessage) ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password))
                return (false, Messages.GetMessage(MessageCodes.Validation.Required, "Şifre"));

            var errors = new List<string>();

            // Minimum uzunluk kontrolü
            if (password.Length < 6)
                errors.Add(Messages.GetMessage(MessageCodes.Validation.MinLength, "Şifre", 6));

            // Maksimum uzunluk kontrolü
            if (password.Length > 50)
                errors.Add(Messages.GetMessage(MessageCodes.Validation.MaxLength, "Şifre", 50));

            // En az bir büyük harf kontrolü
            if (!Regex.IsMatch(password, @"[A-Z]"))
                errors.Add("Şifre en az bir büyük harf içermelidir");

            // En az bir küçük harf kontrolü
            if (!Regex.IsMatch(password, @"[a-z]"))
                errors.Add("Şifre en az bir küçük harf içermelidir");

            // En az bir rakam kontrolü
            if (!Regex.IsMatch(password, @"\d"))
                errors.Add("Şifre en az bir rakam içermelidir");

            // En az bir özel karakter kontrolü
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
                errors.Add("Şifre en az bir özel karakter içermelidir");

            // Yaygın şifre kontrolü
            var commonPasswords = new[] { "password", "123456", "qwerty", "admin", "letmein" };
            if (commonPasswords.Contains(password.ToLower()))
                errors.Add("Bu şifre çok yaygın, lütfen daha güvenli bir şifre seçin");

            if (errors.Any())
                return (false, string.Join("; ", errors));

            return (true, string.Empty);
        }
    }
} 