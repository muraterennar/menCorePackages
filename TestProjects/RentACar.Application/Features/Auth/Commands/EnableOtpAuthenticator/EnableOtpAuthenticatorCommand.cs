﻿using MediatR;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.AuthenticatorServices;
using RentACar.Application.Services.Repositories;
using RentACar.Application.Services.UserServices;

namespace RentACar.Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorCommand : IRequest<EnabledOtpAuthenticatorResponse>
{
    public int UserId { get; set; }

    // Otp doğrulayıcıyı etkinleştirmek için komut işleyicisini uygular
    public class
        EnableOtpAuthenticatorCommandHandler : IRequestHandler<EnableOtpAuthenticatorCommand,
        EnabledOtpAuthenticatorResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IOtpAuthenticatorRepository _otpAuthenticatorRepository;
        private readonly IUserService _userService;

        // Bağımlılıkları enjekte ederek EnableOtpAuthenticatorCommandHandler sınıfını oluşturur
        public EnableOtpAuthenticatorCommandHandler(AuthBusinessRules authBusinessRules,
            IAuthenticatorService authenticatorService, IOtpAuthenticatorRepository otpAuthenticatorRepository,
            IUserService userService)
        {
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
            _otpAuthenticatorRepository = otpAuthenticatorRepository;
            _userService = userService;
        }

        // Otp doğrulayıcıyı etkinleştirir
        public async Task<EnabledOtpAuthenticatorResponse> Handle(EnableOtpAuthenticatorCommand request,
            CancellationToken cancellationToken)
        {
            // Kullanıcıyı kimlik numarasına göre alır
            var user = await _userService.GetByIdAsync(request.UserId);

            // Kullanıcının varlığını kontrol eder
            await _authBusinessRules.UserShouldBeExists(user);

            // Kullanıcının daha önce doğrulayıcıya sahip olmadığını kontrol eder
            await _authBusinessRules.UserShouldNotBeHaveAuthenticator(user);

            // Kullanıcının zaten doğrulanmış bir Otp doğrulayıcısının olmadığını kontrol eder
            var isExistsOtpAuthenticator = await _otpAuthenticatorRepository.GetAsync(o => o.UserId == request.UserId);
            await _authBusinessRules.OtpAuthenticatorThatVerifiedShouldNotBeExists(isExistsOtpAuthenticator);

            // Eğer mevcut bir Otp doğrulayıcı varsa, silinir
            if (isExistsOtpAuthenticator is not null)
                await _otpAuthenticatorRepository.DeleteAsync(isExistsOtpAuthenticator);

            // Yeni bir Otp doğrulayıcı oluşturur
            var newOtpAuthenticator = await _authenticatorService.CreateOtpAuthenticator(user);
            var addedAuthenticator = await _otpAuthenticatorRepository.AddAsync(newOtpAuthenticator);

            // Otp doğrulayıcının gizli anahtarını dizeye dönüştürür
            var secretKey = await _authenticatorService.ConvertSecretKeyToString(addedAuthenticator.SecretKey);
            var otp = await _authenticatorService.CreateOtpCode(addedAuthenticator.SecretKey);
            var otpQrCode =
                await _authenticatorService.GenerateOtpQrCode(addedAuthenticator.SecretKey, user.Email, "RentACar",
                    $"otps/otp-{Guid.NewGuid()}.png");

            // Etkinleştirilmiş Otp doğrulayıcı yanıtını oluşturur ve döndürür
            EnabledOtpAuthenticatorResponse enabledOtpAuthenticatorDto = new()
            {
                Otp = otp,
                OtpQrCode = otpQrCode
            };

            return enabledOtpAuthenticatorDto;
        }
    }

    public static string GetRoot(string folderName)
    {
        // var path = Path.Combine(Directory.GetCurrentDirectory(), "~/TestProjects/RentACar.WebAPI/wwwroot/Templates")
        var path = Path.Combine(Directory.GetCurrentDirectory(),
            $"{folderName}");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }
}