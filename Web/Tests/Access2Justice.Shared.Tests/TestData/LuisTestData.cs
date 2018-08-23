namespace Access2Justice.Shared.Tests.TestData
{
    public static class LuisTestData
    {
        public static string Query
        {
            get
            {
                return "eviction";
            }
        }

        public static string ProperLuisResponse
        {
            get
            {
                return "{\r\n  \"query\": \"child abuse\",\r\n  \"topScoringIntent\": {\r\n    " +
                       "\"intent\": \"eviction\",\r\n    \"score\": 0.919329442\r\n  },\r\n  \"intents\": [\r\n    {\r\n     " +
                       " \"intent\": \"eviction\",\r\n      \"score\": 0.239329442\r\n    },\r\n    {\r\n      " +
                       "\"intent\": \"child abuse\",\r\n      \"score\": 0.09217278\r\n    },\r\n    {\r\n      " +
                       "\"intent\": \"child\",\r\n      \"score\": 0.06267241\r\n    },\r\n    {\r\n      " +
                       "\"intent\": \"divorce\",\r\n      \"score\": 0.00997853652\r\n    },\r\n    {\r\n     " +
                       " \"intent\": \"None\",\r\n      \"score\": 0.00248154555\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}";
            }
        }

        public static string NoneLuisResponse
        {
            get
            {
                return "{\r\n  \"query\": \"good bye\",\r\n  \"topScoringIntent\": {\r\n    " +
                   "\"intent\": \"None\",\r\n    \"score\": 0.7257252\r\n  },\r\n  " +
                   "\"intents\": [\r\n    {\r\n      \"intent\": \"None\",\r\n      " +
                   "\"score\": 0.06429157\r\n    },\r\n    {\r\n      \"intent\": \"Divorce\",\r\n      " +
                   "\"score\": 0.05946025\r\n    },\r\n    {\r\n      \"intent\": \"Eviction\",\r\n     " +
                   "\"score\": 4.371685E-05\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}";
            }
        }
    }
}
