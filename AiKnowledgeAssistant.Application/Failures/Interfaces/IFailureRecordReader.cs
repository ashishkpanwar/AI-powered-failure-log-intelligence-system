using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AiKnowledgeAssistant.Application.Failures.Models;

namespace AiKnowledgeAssistant.Application.Failures.Interfaces
{
    public interface IFailureRecordReader
    {
        Task<IReadOnlyList<FailureRecord>> GetByJobIdsAsync(List<string> jobIds);
    }
}
