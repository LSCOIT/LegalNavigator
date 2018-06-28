using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Logger.AppEnum
{
	/// <summary>
	/// The categories of event id's within the application
	/// </summary>
	public enum EventDictionary
	{
		General = 100,
		HttpHelper = 109,
		HttpRetry = 110,
		Unauthorized = 120,
		MessageStore = 200,
		MessageFailedValidation = 210,
		MessageIdDuplication = 215,
		AcquireAadToken = 300,
		DataLake = 700,
		Queue = 800,
		EventProcessor = 815,
		DBProcessor = 830,
		CandidacyEventProcessor = 845,
		RegularCandidateProcessor = 860,
		SecureCandidateProcessor = 865,
		Fetcher = 870
	}

}
