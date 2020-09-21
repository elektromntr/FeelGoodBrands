using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace Logic.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IRepository<Customer> customerRepository,
	        IEmailConfiguration emailConfiguration)
        {
            _customerRepository = customerRepository;
            _emailConfiguration = emailConfiguration;
        }

        public async Task<bool> ContactMe(ContactMeViewModel contact)
        {
            try
            {
                var customer = new Customer
                {
	                CreationDate = DateTime.Now,
	                PhoneNumber = contact.Phone,
	                Email = contact.Email,
	                Name = contact.Name,
	                Content = contact.Content,
	                EditDate = DateTime.Now,
                };
                var tasksList = new List<Task>();
                _customerRepository.Add(customer);
                SendNewCustomerEmail(customer);
                _customerRepository.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return true;
            }
        }

        public async Task SendNewCustomerEmail(Customer customer)
        {
	        try
	        {
		        // create message
		        var message = new MimeMessage();
		        message.From.Add(new MailboxAddress(customer.Name, "noreply@materie.pl"));
		        message.Subject = "Kontakt ze strony";
		        message.Body = new TextPart("html")
		        {
			        Text =
				        $"<p>{customer.Name} prosi o kontakt.<br>Telefon: {customer.PhoneNumber}<br>Email: {customer.Email}<br>Treść: {customer.Content}</p><p>Data: {customer.CreationDate}</p>"
		        };
		        message.To.Add(new MailboxAddress("Michał", "michal@materie.pl"));
		        message.Bcc.Add(new MailboxAddress("Admin", "admin@materie.pl"));
				if (!string.IsNullOrWhiteSpace(customer.Email))
					message.ReplyTo.Add(new MailboxAddress(customer.Name, customer.Email));

		        // send email
		        using var smtp = new SmtpClient();
		        smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
		        await smtp.ConnectAsync(_emailConfiguration.SmtpServer, 
			        _emailConfiguration.SmtpPort, true);
		        await smtp.AuthenticateAsync(_emailConfiguration.SmtpUsername, 
			        _emailConfiguration.SmtpPassword);
		        await smtp.SendAsync(message);
		        await smtp.DisconnectAsync(true);
            }
	        catch (Exception e)
	        {
		        Debug.WriteLine(e);
		        throw;
	        }
        }
    }
}
