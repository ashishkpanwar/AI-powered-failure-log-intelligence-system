using AiKnowledgeAssistant.Api;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders().AddConsole().AddDebug();

CompositionRoot.ConfigureServices(
    builder.Services,
    builder.Configuration);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


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