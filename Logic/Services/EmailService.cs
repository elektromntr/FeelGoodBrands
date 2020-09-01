using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.WebPages;
using Data.Models;
using Data.Repository;
using Logic.DataTransferObjects;
using Logic.Services.Interfaces;

namespace Logic.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRepository<Customer> _customerRepository;

        public EmailService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> ContactMe(ContactMeViewModel contact)
        {
            try
            {
                await _customerRepository.Add(new Customer
                {
                    CreationDate = DateTime.Now,
                    PhoneNumber = contact.Phone,
                    Email = contact.Email,
                    Name = contact.Name,
                    Content = contact.Content,
                    EditDate = DateTime.Now,
                });
                _customerRepository.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }
}
