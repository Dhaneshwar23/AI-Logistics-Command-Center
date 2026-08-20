using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.AI.Abstractions
{
    public interface IAiTool
    {
        string Name { get; }
        string Description { get; }

        object GetParametersSchema();

        Task<Object?> ExecuteAsync(IReadOnlyDictionary<string, object> arguments, CancellationToken cancellationToken);
    }
}
