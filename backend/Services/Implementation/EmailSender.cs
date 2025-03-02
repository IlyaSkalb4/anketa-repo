using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace backend.Services.Implementation
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration configuration;

		public EmailSender(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var smtpServer = configuration["EmailSettings:SmtpServer"];
			var smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
			var smtpUsername = configuration["EmailSettings:SmtpUsername"];
			var smtpPassword = configuration["EmailSettings:SmtpPassword"];
			var fromEmail = configuration["EmailSettings:FromEmail"];
			var fromName = configuration["EmailSettings:FromName"];

			using (var client = new SmtpClient(smtpServer, smtpPort))
			{
				client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
				client.EnableSsl = true; 

				var mailMessage = new MailMessage
				{
					From = new MailAddress(fromEmail, fromName),
					Subject = subject,
					Body = htmlMessage,
					IsBodyHtml = true
				};
				mailMessage.To.Add(email);

				await client.SendMailAsync(mailMessage);
			}
		}
	}
}