using ActiveClub.Shared.DTOs.Kontakt;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace ActiveClub.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendKontaktAsync(KontaktFormDto dto)
        {
            await SendToCompanyAsync(dto);
            await SendConfirmationToUserAsync(dto);
        }

        private async Task SendToCompanyAsync(KontaktFormDto dto)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                "ActiveClub Formularz",
                _config["Mail:From"]));

            message.To.Add(new MailboxAddress(
                "ActiveClub",
                _config["Mail:To"]));

            message.ReplyTo.Add(new MailboxAddress(
                dto.ImieNazwisko,
                dto.Email));

            message.Subject = $"Formularz kontaktowy: {dto.Temat}";

            message.Body = new TextPart("plain")
            {
                Text =
$"""
Nowa wiadomość z formularza kontaktowego ActiveClub

Imię i nazwisko: {dto.ImieNazwisko}
Email użytkownika: {dto.Email}
Temat: {dto.Temat}

Wiadomość:
{dto.Wiadomosc}
"""
            };

            await SendAsync(message);
        }

        private async Task SendConfirmationToUserAsync(KontaktFormDto dto)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                "ActiveClub",
                _config["Mail:From"]));

            message.To.Add(new MailboxAddress(
                dto.ImieNazwisko,
                dto.Email));

            message.Subject = "Potwierdzenie wysłania wiadomości - ActiveClub";

            message.Body = new TextPart("plain")
            {
                Text =
$"""
Dzień dobry {dto.ImieNazwisko},

Dziękujemy za kontakt z ActiveClub.
Otrzymaliśmy Twoją wiadomość i odpowiemy najszybciej jak to możliwe.

Kopia Twojej wiadomości:

Temat: {dto.Temat}

Wiadomość:
{dto.Wiadomosc}

Pozdrawiamy,
Zespół ActiveClub
"""
            };

            await SendAsync(message);
        }

        private async Task SendAsync(MimeMessage message)
        {
            using var client = new SmtpClient();

            await client.ConnectAsync(
                _config["Mail:SmtpHost"],
                int.Parse(_config["Mail:SmtpPort"]!),
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                _config["Mail:Username"],
                _config["Mail:Password"]);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}