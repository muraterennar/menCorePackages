﻿using MenCore.Persistence.Repositories; // MenCore içinde bulunan Persistence.Repositories isim alanı kullanılıyor
using MenCore.Security.Enums;

namespace MenCore.Security.Entities
{
    // User sınıfı, Entity<int> sınıfından türetiliyor
    public class User : Entity<int>
    {
        // Kullanıcının adı
        public string FirstName { get; set; }

        // Kullanıcının soyadı
        public string LastName { get; set; }

        // Kullanıcının tam adı (ad + soyad)
        public string FullName => $"{FirstName} {LastName}";

        // Kullanıcının kullanıcı adı (küçük harfe dönüştürülmüş ad ve soyadın birleşimi)
        public string Username { get; set; }

        // Kullanıcının kimlik numarası
        public string IdentityNumber { get; set; }

        // Kullanıcının doğum yılı
        public short BirthYear { get; set; }

        // Kullanıcının e-posta adresi
        public string Email { get; set; }

        // Kullanıcının şifresinin tuzu
        public byte[] PasswordSalt { get; set; }

        // Kullanıcının şifresinin hash değeri
        public byte[] PasswordHash { get; set; }

        // Kullanıcının durumu (aktif veya pasif)
        public bool Status { get; set; }

        // Kullanıcının Authentication tipi
        public AuthenticatorType AuthenticatorType { get; set; }

        // Kullanıcının işlem yetkileri
        public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = null!;

        // Yenileme token'ları
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = null!;

        // OTP kimlik doğrulayıcıları
        public virtual ICollection<OtpAuthenticator> OtpAuthenticators { get; set; } = null!;

        // E-posta doğrulayıcıları
        public virtual ICollection<EmailAuthenticator> EmailAuthenticators { get; set; } = null!;

        // Varsayılan yapılandırıcı, boş bir kullanıcı oluşturur
        public User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            IdentityNumber = string.Empty;
            Email = string.Empty;
            Status = false;
            PasswordHash = Array.Empty<byte>();
            PasswordSalt = Array.Empty<byte>();
        }

        // Parametreleri kullanarak kullanıcı oluşturur
        public User(string firstName, string lastName, string identityNumber, short birthYear, string email, byte[] passwordSalt, byte[] passwordHash, bool status)
        {
            FirstName = firstName;
            LastName = lastName;
            IdentityNumber = identityNumber;
            BirthYear = birthYear;
            Email = email;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
            Status = status;
        }

        // Tüm özelliklerin yanı sıra tarih bilgilerini de içeren parametreleri kullanarak kullanıcı oluşturur
        public User(int id, string firstName, string lastName, string identityNumber, short birthYear, string email, byte[] passwordSalt, byte[] passwordHash, bool status, DateTime createdDate, DateTime updatedDate, DateTime deletedDate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            IdentityNumber = identityNumber;
            BirthYear = birthYear;
            Email = email;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
            Status = status;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            DeletedDate = deletedDate;
        }
    }
}