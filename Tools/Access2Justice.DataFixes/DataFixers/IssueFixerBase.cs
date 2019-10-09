using System;
using System.IO;

namespace Access2Justice.DataFixes.DataFixers
{
    public abstract class IssueFixerBase
    {
	    private readonly string _fileName;

        protected string LogEntryPrefix => $"\t{IssueId}: ";

        protected abstract string IssueId { get; }

        protected void LogEntry(string entry)
        {
	        if (string.IsNullOrWhiteSpace(_fileName))
	        {
		        Console.WriteLine($"{LogEntryPrefix}{entry}");
	        }
	        else
	        {
				File.AppendAllText(_fileName, entry);
	        }
        }

        protected IssueFixerBase():this(null)
        {
        }

        protected IssueFixerBase(string fileName)
        {
	        if (fileName != null)
	        {
		        if (!File.Exists(fileName))
		        {
			        using (File.Create(fileName))
			        {
				        _fileName = fileName;
			        }
		        }
	        }
        }
    }
}