using CommandLine;

namespace Access2Justice.DataFixes
{
    public class CommandLineOptions
    {
        [Option('e', "endpoint", Required = true, HelpText = "CosmosDB Endpoint")]
        public string CosmosDbEndpoint { get; set; }

        [Option('k', "apikey", Required = true, HelpText = "CosmosDB Key")]
        public string CosmosDbApiKey { get; set; }

        [Option('d', "dbid", Required = true, HelpText = "CosmosDB Database Id")]
        public string CosmosDbDatabaseId { get; set; }

        [Option('i', "issue", Required = true, HelpText = "Id of issue to be fixed. Fixes available for: #829,953,875,350")]
        public string IssueId { get; set; }

        [Option('a', "additional", Required = false)]
		public string AdditionalPram { get; set; }
    }
}
