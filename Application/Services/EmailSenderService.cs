//using Application.Repositories;
using Infrastructure.Abstracts;
//using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    //public class EmailSenderService : BackgroundService
    //{
    //    private readonly ISubscribersRepository subscribersRepository;
    //    private readonly IEmailService emailService;

    //    public EmailSenderService(ISubscribersRepository subscribersRepository, IEmailService emailService)
    //    {
    //        this.subscribersRepository = subscribersRepository;
    //        this.emailService = emailService;
    //    }

    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        var emails = await subscribersRepository.GetAll();
    //        var chunkSize = 80;
    //        var chunks = emails.Chunk(chunkSize);

    //        foreach (var chunk in chunks)
    //        {
    //            await emailService.SendBulkEmailAsync(chunk, "Subject", "Body");
    //            await Task.Delay(1000, stoppingToken); // Optional: Throttle to avoid hitting SMTP limits
    //        }
    //    }
    //}

}
