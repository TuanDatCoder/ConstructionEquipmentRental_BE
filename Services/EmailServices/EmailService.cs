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

      
        // Register and Verify
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
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f9f9f9;
        }}
        .container {{
            max-width: 700px;
            margin: 0 auto;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            background-color: #ff9800; /* Màu cam nhẹ */
            color: #fff;
            padding: 20px;
            text-align: center;
            border-radius: 8px 8px 0 0;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
        }}
        .header h1 {{
            font-size: 36px;
            font-weight: bold;
            margin: 0;
        }}
        .body {{
            padding: 20px;
            background-color: #ffffff;
            border-radius: 0 0 8px 8px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }}
        .body p {{
            font-size: 16px;
            line-height: 1.5;
            color: #333;
        }}
        .btn {{
            display: inline-block;
            background-color: #ff9800; /* Màu cam nhẹ */
            color: #fff;
            padding: 15px 30px;
            font-size: 18px;
            border-radius: 5px;
            text-decoration: none;
            box-shadow: 0 5px 15px rgba(255, 152, 0, 0.4);
            transition: all 0.3s ease;
            margin-top: 20px;
        }}
        .btn:hover {{
            background-color: #fb8c00;
            transform: translateY(-5px);
            box-shadow: 0 10px 20px rgba(255, 140, 0, 0.6);
        }}
        .footer {{
            padding: 10px;
            text-align: center;
            font-size: 14px;
            color: #888;
            background-color: #e0e0e0; /* Màu xám nhẹ */
            border-top: 2px solid #bbb;
            border-radius: 0 0 8px 8px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }}
    </style>
</head>
<body>
    <div class='container'>
        <!-- Header Section -->
        <div class='header'>
            <h1>Welcome to ConstructionEquipmentRental!</h1>
        </div>

        <!-- Body Section -->
        <div class='body'>
            <p>Hi {fullName},</p>
            <p>Thank you for registering with ConstructionEquipmentRental. We're excited to have you on board and ready to help you rent the best construction equipment!</p>
            <p>Please verify your email by clicking the link below:</p>
            <a href='{verificationUrl}' class='btn'>Verify Email</a>
        </div>

        <!-- Footer Section -->
        <div class='footer'>
            <p>Thank you,</p>
            <p>The ConstructionEquipmentRental Team</p>
        </div>
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
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f9f9f9;
                    color: #333;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 700px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 10px;
                    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    background-color: #4caf50; /* Màu xanh nhẹ */
                    color: #fff;
                    padding: 20px;
                    text-align: center;
                    border-radius: 8px 8px 0 0;
                    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
                }}
                .header h1 {{
                    font-size: 36px;
                    font-weight: bold;
                    margin: 0;
                }}
                .body {{
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 0 0 8px 8px;
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                }}
                .body p {{
                    font-size: 16px;
                    line-height: 1.5;
                    color: #333;
                }}
                .footer {{
                    padding: 10px;
                    text-align: center;
                    font-size: 14px;
                    color: #888;
                    background-color: #e0e0e0; /* Màu xám nhẹ */
                    border-top: 2px solid #bbb;
                    border-radius: 0 0 8px 8px;
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Welcome to ConstructionEquipmentRental!</h1>
                </div>
                <div class='body'>
                    <p>Hi {fullName},</p>
                    <p>Thank you for registering with ConstructionEquipmentRental. We're excited to have you on board and ready to help you rent the best construction equipment!</p>
                    <p>We hope you enjoy the experience!</p>
                    <p>Thank you,</p>
                    <p>The ConstructionEquipmentRental Team</p>
                </div>
                <div class='footer'>
                    <p>Thank you for choosing us.</p>
                </div>
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
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f9f9f9;
                    color: #333;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 700px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 10px;
                    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    background-color: #4caf50; /* Màu xanh nhẹ */
                    color: #fff;
                    padding: 20px;
                    text-align: center;
                    border-radius: 8px 8px 0 0;
                    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
                }}
                .header h1 {{
                    font-size: 36px;
                    font-weight: bold;
                    margin: 0;
                }}
                .body {{
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 0 0 8px 8px;
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                }}
                .body p {{
                    font-size: 16px;
                    line-height: 1.5;
                    color: #333;
                }}
                .otp {{
                    font-size: 24px;
                    font-weight: bold;
                    color: #4caf50; /* Màu xanh */
                }}
                .footer {{
                    padding: 10px;
                    text-align: center;
                    font-size: 14px;
                    color: #888;
                    background-color: #e0e0e0; /* Màu xám nhẹ */
                    border-top: 2px solid #bbb;
                    border-radius: 0 0 8px 8px;
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Password Reset Request</h1>
                </div>
                <div class='body'>
                    <p>Hi {fullName},</p>
                    <p>You have requested to reset your password. Please use the following OTP:</p>
                    <p class='otp'>{OTP}</p>
                    <p>This OTP is valid for a limited time. Please use it as soon as possible.</p>
                    <p>If you did not request a password reset, please ignore this email.</p>
                    <p>Thank you,</p>
                    <p>The ConstructionEquipmentRental Team</p>
                </div>
                <div class='footer'>
                    <p>Thank you for choosing us.</p>
                </div>
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
