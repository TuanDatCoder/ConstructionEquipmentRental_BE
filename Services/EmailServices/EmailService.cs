using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        // Gửi lúc đăng ký thành công
        public async Task SendRegistrationEmail(string fullName, string userEmail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "[ConstructionEquipmentRental Application] - Welcome to ConstructionEquipmentRental!";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Welcome to ConstructionEquipmentRental</title>
                </head>
                <body style='font-family: Arial, sans-serif; background-color: #dcf343; color: #ffffff;'>
                    <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4949e9;'>
                        <h1>Welcome to ConstructionEquipmentRental!</h1>
                        <p>Hi {fullName},</p>
                        <p>Thank you for registering with ConstructionEquipmentRental. We're excited to have you on board!</p>
                        <p>We hope you enjoy the experience!</p>
                        <p>Thank you,</p>
                        <p>The ConstructionEquipmentRental Team</p>
                    </div>
                </body>
                </html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }





        public async Task SendRegistrationEmail(string fullName, string userEmail, string verificationUrl)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "[ConstructionEquipmentRental Application] - Welcome to ConstructionEquipmentRental!";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Welcome to ConstructionEquipmentRental</title>
        </head>
        <body style='font-family: Arial, sans-serif; background-color: #dcf343; color: #ffffff;'>
            <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4949e9;'>
                <h1>Welcome to ConstructionEquipmentRental!</h1>
                <p>Hi {fullName},</p>
                <p>Thank you for registering with ConstructionEquipmentRental. We're excited to have you on board!</p>
                <p>Please verify your email by clicking the link below:</p>
                <a href='{verificationUrl}' style='color: #fff; text-decoration: underline;'>Verify Email</a>
                <p>Thank you,</p>
                <p>The ConstructionEquipmentRental Team</p>
            </div>
        </body>
        </html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }




        public async Task SendAccountResetPassword(string fullName, string userEmail, string OTP)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "[ConstructionEquipmentRental Application] - Password Reset Request";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Password Reset</title>
                </head>
                <body style='font-family: Arial, sans-serif; background-color: #dcf343; color: #ffffff;'>
                    <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4949e9;'>
                        <h1>Password Reset Request</h1>
                        <p>Hi {fullName},</p>
                        <p>You have requested to reset your password. Please use the following OTP:</p>
                        <p style='font-size: 24px; font-weight: bold;'>{OTP}</p>
                        <p>This OTP is valid for a limited time. Please use it as soon as possible.</p>
                        <p>If you did not request a password reset, please ignore this email.</p>
                        <p>Thank you,</p>
                        <p>The ConstructionEquipmentRental Team</p>
                    </div>
                </body>
                </html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
