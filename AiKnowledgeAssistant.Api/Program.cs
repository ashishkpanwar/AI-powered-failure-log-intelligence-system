using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Infrastructure.AI;
using Azure;
using Azure.AI.OpenAI;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(sp =>
{
    var endpoint = new Uri(builder.Configuration["AzureOpenAI:Endpoint"]!);
    var key = builder.Configuration["AzureOpenAI:ApiKey"]!;
    return new AzureOpenAIClient(endpoint, new AzureKeyCredential(key));
});


builder.Services.AddSingleton<IAiClient>(sp =>
{
    var azureClient = sp.GetRequiredService<AzureOpenAIClient>();
    var deployment = builder.Configuration["AzureOpenAI:ChatDeployment"]!;
    return new AzureOpenAiClient(azureClient,deployment);
});


var app = builder.Build();
app.UseDeveloperExceptionPage();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();