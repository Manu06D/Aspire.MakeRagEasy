using Aspire.MakeRagEasy.AiApiService;
using Aspire.MakeRagEasy.AiApiService.Enpoints;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddScoped<OllamaService>();

string? ollamaOpenApiEndpoint = builder.Configuration["OllamaOpenApiEndpointUri"];
if (!string.IsNullOrEmpty(ollamaOpenApiEndpoint))
{
    // Microsoft.SemanticKernel NuGet 
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    builder.Services.AddKernel().AddOpenAIChatCompletion("phi", new Uri(ollamaOpenApiEndpoint), null);
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.RegisterOllamaEndpoints();

app.Run();
