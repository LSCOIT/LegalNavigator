using System;
using System.Linq;

namespace Access2Justice.Shared
{
    public class USState
    {
        public static readonly USState Alabama = new USState("Alabama", "AL");
        public static readonly USState Alaska = new USState("Alaska", "AK");
        public static readonly USState Arizona = new USState("Arizona", "AZ");
        public static readonly USState Arkansas = new USState("Arkansas", "AR");
        public static readonly USState California = new USState("California", "CA");
        public static readonly USState Colorado = new USState("Colorado", "CO");
        public static readonly USState Connecticut = new USState("Connecticut", "CT");
        public static readonly USState Delaware = new USState("Delaware", "DE");
        public static readonly USState Florida = new USState("Florida", "FL");
        public static readonly USState Georgia = new USState("Georgia", "GA");
        public static readonly USState Hawaii = new USState("Hawaii", "HI");
        public static readonly USState Idaho = new USState("Idaho", "ID");
        public static readonly USState Illinois = new USState("Illinois", "IL");
        public static readonly USState Indiana = new USState("Indiana", "IN");
        public static readonly USState Iowa = new USState("Iowa", "IA");
        public static readonly USState Kansas = new USState("Kansas", "KS");
        public static readonly USState Kentucky = new USState("Kentucky", "KY");
        public static readonly USState Louisiana = new USState("Louisiana", "LA");
        public static readonly USState Maine = new USState("Maine", "ME");
        public static readonly USState Maryland = new USState("Maryland", "MD");
        public static readonly USState Massachusetts = new USState("Massachusetts", "MA");
        public static readonly USState Michigan = new USState("Michigan", "MI");
        public static readonly USState Minnesota = new USState("Minnesota", "MN");
        public static readonly USState Mississippi = new USState("Mississippi", "MS");
        public static readonly USState Missouri = new USState("Missouri", "MO");
        public static readonly USState Montana = new USState("Montana", "MT");
        public static readonly USState Nebraska = new USState("Nebraska", "NE");
        public static readonly USState Nevada = new USState("Nevada", "NV");
        public static readonly USState NewHampshire = new USState("New Hampshire", "NH");
        public static readonly USState NewJersey = new USState("New Jersey", "NJ");
        public static readonly USState NewMexico = new USState("New Mexico", "NM");
        public static readonly USState NewYork = new USState("New York", "NY");
        public static readonly USState NorthCarolina = new USState("North Carolina", "NC");
        public static readonly USState NorthDakota = new USState("North Dakota", "ND");
        public static readonly USState Ohio = new USState("Ohio", "OH");
        public static readonly USState Oklahoma = new USState("Oklahoma", "OK");
        public static readonly USState Oregon = new USState("Oregon", "OR");
        public static readonly USState Pennsylvania = new USState("Pennsylvania", "PA");
        public static readonly USState RhodeIsland = new USState("Rhode Island", "RI");
        public static readonly USState SouthCarolina = new USState("South Carolina", "SC");
        public static readonly USState SouthDakota = new USState("South Dakota", "SD");
        public static readonly USState Tennessee = new USState("Tennessee", "TN");
        public static readonly USState Texas = new USState("Texas", "TX");
        public static readonly USState Utah = new USState("Utah", "UT");
        public static readonly USState Vermont = new USState("Vermont", "VT");
        public static readonly USState Virginia = new USState("Virginia", "VA");
        public static readonly USState Washington = new USState("Washington", "WA");
        public static readonly USState WestVirginia = new USState("West Virginia", "WV");
        public static readonly USState Wisconsin = new USState("Wisconsin", "WI");
        public static readonly USState Wyoming = new USState("Wyoming", "WY");

        public static readonly USState[] AllStates = new USState[]
        {
            Alabama, Alaska, Arizona, Arkansas, California, Colorado,
            Connecticut, Delaware, Florida, Georgia, Hawaii, Idaho, Illinois,
            Indiana, Iowa, Kansas, Kentucky, Louisiana, Maine, Maryland,
            Massachusetts, Michigan, Minnesota, Mississippi, Missouri, Montana,
            Nebraska, Nevada, NewHampshire, NewJersey, NewMexico, NewYork,
            NorthCarolina, NorthDakota, Ohio, Oklahoma, Oregon, Pennsylvania,
            RhodeIsland, SouthCarolina, SouthDakota, Tennessee, Texas, Utah,
            Vermont, Virginia, Washington, WestVirginia, Wisconsin, Wyoming
        };

        private USState(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }

        public string Name { get; private set; }
        public string Abbreviation { get; private set; }

        public static USState GetByName(string name)
        {
            return AllStates.
                FirstOrDefault(
                    s => string.Compare(s.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public static USState GetByAbbreviation(string abbreviation)
        {
            return AllStates.
                FirstOrDefault(
                    s => string.Compare(s.Abbreviation, abbreviation, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
