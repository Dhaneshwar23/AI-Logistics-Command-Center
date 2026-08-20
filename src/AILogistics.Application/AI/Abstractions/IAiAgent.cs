using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.AI.Abstractions
{
    public interface IAiAgent
    {
        Task<string> AskAsync(string message, CancellationToken cancellationToken = default);
    }
}
