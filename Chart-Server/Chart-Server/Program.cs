using Chart.SignalR.Server;
using Microsoft.Extensions.Options;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddScoped<SignalRMessage>();
//builder.Services.AddSingleton<SignalRHub>();

builder.Services.Configure<JobOptions>(builder.Configuration.GetSection("Jobs"));

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.SchedulerName = "Chart schedulers";
});

builder.Services.AddOptions<QuartzOptions>()
    .Configure<IOptions<JobOptions>>((options, jobOptions) =>
    {
        options.AddJob<PieChartServiceJob>(j => j.WithIdentity("pie-chart-service-job"));

        options.AddTrigger(trigger => trigger
            .WithIdentity("cron schedule for pie-chart-service-job")
            .ForJob("pie-chart-service-job")
            .WithCronSchedule(jobOptions.Value.PieChartSchedule));
    });

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// UseCors must be called before MapHub.
app.UseCors(x =>
              x.WithOrigins(
                  "http://localhost:4200")
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod());

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<SignalRHub>("/signalr");
app.Run();
