using System;

namespace Access2Justice.DataFixes.DataFixers
{
    public abstract class IssueFixerBase
    {
        protected string LogEntryPrefix => $"\t{IssueId}: ";

        protected abstract string IssueId { get; }

        protected void LogEntry(string entry)
        {
            Console.WriteLine($"{LogEntryPrefix}{entry}");
        }
    }
}