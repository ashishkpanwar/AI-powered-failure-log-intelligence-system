using AiKnowledgeAssistant.Infrastructure.Persistence;
using AiKnowledgeAssistant.Infrastructure.Repositories;
using AiKnowledgeAssistant.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> dbOptions)
    {
        services.AddDbContext<JobExecutionDbContext>(dbOptions);

        services.AddScoped<IJobExecutionReader, JobExecutionReader>();

        return services;
    }
}
