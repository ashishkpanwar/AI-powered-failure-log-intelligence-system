using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models
{
    public interface IFailureRecordReader
    {
        Task<IReadOnlyList<FailureRecord>> GetByJobIdsAsync(List<string> jobIds);
    }
}
