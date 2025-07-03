using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(opt =>
{
    opt.AddFixedWindowLimiter("FixedPolicy", policyOpt =>
    {
        policyOpt.Window = TimeSpan.FromSeconds(10);
        policyOpt.PermitLimit = 5;
        policyOpt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

var app = builder.Build();

app.MapReverseProxy();

app.Run();
