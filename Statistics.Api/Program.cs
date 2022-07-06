using System.Text.Json.Serialization;
using IdentityServer4.AccessTokenValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Refit;
using Statistics.Api.Consumers;
using Statistics.Api.Converters;
using Statistics.Api.Extensions;
using Statistics.Api.Handlers;
using Statistics.Data.EFCore;
using Statistics.Data.EFCore.Abstracts;
using Statistics.Data.Refit;
using Statistics.Services;
using Statistics.Services.Abstracts;
using Statistics.Services.AutoMapper.Profiles;
using SurveyMe.AnswersApi.Models.Queue;
using SurveyMe.Common.Logging;
using SurveyMe.QueueModels;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logBuilder =>
{
    logBuilder.AddLogger();
    logBuilder.AddFile(builder.Configuration.GetSection("Serilog:FileLogging"));
});

builder.Services.AddDbContext<StatisticsDbContext>(o 
    => o.UseNpgsql(builder.Configuration
        .GetConnectionString("DefaultConnection")));
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        o.JsonSerializerOptions.Converters.Add(new QuestionStatisticsConverter());
    });
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<AuthorizeHandler>();

builder.Services.AddRefitClient<ISurveyPersonOptionsApi>().ConfigureHttpClient(config =>
{
    var stringUrl = builder.Configuration.GetConnectionString("SurveyPersonOptionsApi");
    config.BaseAddress = new Uri(stringUrl);
}).AddHttpMessageHandler<AuthorizeHandler>();

builder.Services.AddRefitClient<IPersonsApi>().ConfigureHttpClient(config =>
{
    var stringUrl = builder.Configuration.GetConnectionString("PersonsApi");
    config.BaseAddress = new Uri(stringUrl);
}).AddHttpMessageHandler<AuthorizeHandler>();

builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IStatisticsUnitOfWork, StatisticsUnitOfWork>();

builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddMaps(typeof(Program).Assembly);
    configuration.AddProfile(new PersonalityProfile());
});

builder.Services.AddMassTransit(c =>
{
    c.AddConsumer<SurveysConsumer>();
    c.AddConsumer<AnswersConsumer>();
    
    c.UsingRabbitMq((context, config) =>
    {
        config.ConfigureJsonSerializerOptions(options =>
        {
            options.Converters.Add(new AnswerQueueConverter());

            return options;
        });
        
        config.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        config.ReceiveEndpoint("survey-statistics-queue", e =>
        {
            e.Bind<SurveyQueueModel>();
            e.ConfigureConsumer<SurveysConsumer>(context);
        });
        
        config.ReceiveEndpoint("answers-statistics-queue", e =>
        {
            e.Bind<SurveyAnswerQueue>();
            e.ConfigureConsumer<AnswersConsumer>(context);
        });
    });
});

builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(options =>
    {
        options.Authority = "https://localhost:7179";
        options.RequireHttpsMetadata = false;
        options.ApiName = "SurveyMeApi";
        options.ApiSecret = "api_secret";
        options.JwtValidationClockSkew = TimeSpan.FromDays(1);
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.CreateDbIfNotExists();

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();