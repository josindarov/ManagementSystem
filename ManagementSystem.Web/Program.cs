using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ManagementSystem.Web;
using ManagementSystem.Web.Brokers.Apis;
using ManagementSystem.Web.Brokers.DateTimes;
using ManagementSystem.Web.Brokers.Loggings;
using ManagementSystem.Web.Services.Foundations.Assingments;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// brokers
builder.Services.AddTransient<IApiBroker, ApiBroker>();
builder.Services.AddTransient<IDateTimeBroker, DateTimeBroker>();
builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();

// Services
builder.Services.AddTransient<IAssignmentService, AssignmentService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();