using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Validations
{
    public class Accumulator
    {
        private readonly List<Issue> _issues = [];
        public IReadOnlyList<Issue> Issues { get { return _issues; } }

        public void Append(Issue issue) => _issues.Add(issue);

        public void Append(IEnumerable<Issue> issues) => _issues.AddRange(issues);
    }
}
