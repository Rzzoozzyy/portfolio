using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PortfolioMvc.Models;
using System.Diagnostics;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace PortfolioMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Portfolio()
        {
            var projects = GetProjects();
            return View(projects);
        }

        public IActionResult Projects()
        {
            var projects = GetProjects();
            return View(projects);
        }

        // Helper method to avoid duplication
        private List<ProjectModel> GetProjects()
        {
            return new List<ProjectModel>
            {
                new ProjectModel { Title = "ERP System", Description = "Optimized ERP modules using ASP.NET Core & Azure SQL.", Link = "#" },
                new ProjectModel { Title = "Visitor Management System", Description = "RBAC, check-ins, audit logging.", Link = "#" },
                new ProjectModel { Title = "School Management System", Description = "Student & admin portal improving reporting accuracy, online exams.", Link = "#" }
            };
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Resume()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(model.Email));
                message.To.Add(MailboxAddress.Parse("ruzzaayoub@gmail.com")); // 📩 your inbox email
                message.Subject = $"Portfolio Contact: {model.Name}";
                message.Body = new TextPart("plain")
                {
                    Text = $"From: {model.Name} ({model.Email})\n\n Subject: {model.Subject} \n\n Msg: {model.Message}"
                };

                using var client = new SmtpClient();

                // 🔧 SMTP Settings (example: Gmail)
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;
                bool smtpEnableSsl = true;
                string smtpUser = "ruzzaayoub@gmail.com"; // your sending Gmail
                string smtpPass = "oiro vtpp bqzs rzxy"; // Gmail app password, not your real password

                await client.ConnectAsync(smtpHost, smtpPort, smtpEnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                await client.AuthenticateAsync(smtpUser, smtpPass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                ViewBag.Message = "✅ Message sent successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"❌ Error: {ex.Message}";
            }

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
