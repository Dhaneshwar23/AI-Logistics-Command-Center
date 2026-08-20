using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.AI.Groq
{
    public sealed class GroqOptions
    {
        public const string SectionName = "Groq";
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Model {  get; set; } = string.Empty;
    }
}
