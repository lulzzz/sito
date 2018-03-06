﻿using System.Threading.Tasks;

namespace Maddalena.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}