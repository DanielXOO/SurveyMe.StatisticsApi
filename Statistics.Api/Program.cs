using MassTransit;
using Statistics.Api.Consumers;
using Statistics.Api.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(c =>
{
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
        
        config.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();