using Access2Justice.Tools.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Access2Justice.Tools.BusinessLogic
{
    class InsertResources
    {
        public Resources CreateJsonFromCSV()
        {
            // path to the schema file to be converted. Look at the included 
            // sample file in this same project 'SampleFiles/Resource_Data_tab'.
            string path = "";
            string textFilePath = path;
            const Int32 BufferSize = 128;
            int lineCount = File.ReadLines(path).Count();
            List<Resource> ResourcesList = new List<Resource>();
            Resources Resources = new Resources();
            

            using (var fileStream = File.OpenRead(textFilePath))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line1, line2;
                line1 = streamReader.ReadLine();
                string[] parts = line1.Split('\t');
                string val;

                while ((line2 = streamReader.ReadLine()) != null)
                {
                    List<string> value = new List<string>();
                    string[] partsb = line2.Split('\t');
                    Conditions[] conditions = null;
                    ReferenceTag[] referenceTags = null;
                    List<Locations> locations = new List<Locations>();

                    for (int iterationCounter = 0; iterationCounter < partsb.Length; iterationCounter++)
                    {
                        val = parts[iterationCounter];
                        if (val.EndsWith("referenceTags", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string referenceId = partsb[iterationCounter];
                            referenceTags = GetReferenceTags(referenceId);
                        }

                        else if (val.EndsWith("location", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string locationId = partsb[iterationCounter];
                            locations = GetLocations(locationId);
                        }

                        else if (val.EndsWith("conditions", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string conditionId = partsb[iterationCounter];
                            conditions = GetConditions(conditionId);
                        }

                        else
                        {
                            value.Add(partsb[iterationCounter]);
                        }
                    }
                    var newResourceId = Guid.NewGuid();
                    ResourcesList.Add(new Resource()
                    {
                        Name = value[1],
                        Description = value[2],
                        ResourceType = value[3],
                        ExternalUrls = value[4],
                        Urls = value[5],
                        ReferenceTags = referenceTags,
                        Location = locations,
                        Icon = value[6],
                        Condition = conditions,
                        Overview = value[7],
                        HeadLine1 = value[8],
                        HeadLine2 = value[9],
                        HeadLine3 = value[10],
                        IsRecommended = value[11],
                        SubService = value[12],
                        Street = value[13],
                        City = value[14],
                        State = value[15],
                        ZipCode = value[16],
                        Telephone = value[17],
                        EligibilityInformation = value[18],
                        ReviewedByCommunityMember = value[19],
                        ReviewerFullName = value[20],
                        ReviewerTitle = value[21],
                        ReviewerImage = value[22],
                        FullDescription = value[23],
                        CreatedBy = value[24],
                        ModifiedBy = value[25]
                    });
                }
            }
            Resources.ResourcesList = ResourcesList.ToList();
            return Resources;
        }

        public dynamic GetReferenceTags(string referenceId)
        {
            ReferenceTag[] referenceTags = null;
            string[] referencesb = null;
            referencesb = referenceId.Split('|');
            referenceTags = new ReferenceTag[referencesb.Length];
            for (int referenceTagIterator = 0; referenceTagIterator < referencesb.Length; referenceTagIterator++)
            {
                referenceTags[referenceTagIterator] = new ReferenceTag()
                {
                    ReferenceTags = referencesb[referenceTagIterator],
                };
            }
            return referenceTags;
        }

        public dynamic GetLocations(string locationId)
        {
            List<Locations> locations = new List<Locations>();
            string[] locsb = null;
            locsb = locationId.Split('|');
            for (int locationIterator = 0; locationIterator < locsb.Length; locationIterator++)
            {
                string tempLocationsId = locsb[locationIterator];
                string[] locationsb = null;
                locationsb = tempLocationsId.Split(';');
                if (locationsb.Length == 4)
                {
                    int position = 0;
                    var specificLocations = new Locations();
                    string state = string.Empty, county = string.Empty, city = string.Empty, zipCode = string.Empty;
                    foreach (var subLocations in tempLocationsId.Split(';'))
                    {
                        state = position == 0 && string.IsNullOrEmpty(state) ? subLocations : state;
                        county = position == 1 && string.IsNullOrEmpty(county) ? subLocations : county;
                        city = position == 2 && string.IsNullOrEmpty(city) ? subLocations : city;
                        zipCode = position == 3 && string.IsNullOrEmpty(zipCode) ? subLocations : zipCode;

                        if (position == 3)
                        {
                            specificLocations = new Locations()
                            {
                                State = state,
                                County = county,
                                City = city,
                                ZipCode = zipCode,
                            };
                        }
                        position++;
                    }
                    locations.Add(specificLocations);
                }

            }
            return locations;
        }

        public dynamic GetConditions(string conditionId)
        {
            Conditions[] conditions = null;
            string[] conditionsb = null;
            conditionsb = conditionId.Split('|');
            conditions = new Conditions[conditionsb.Length];
            for (int conditionIterator = 0; conditionIterator < conditionsb.Length; conditionIterator++)
            {
                conditions[conditionIterator] = new Conditions()
                {
                    Condition = conditionsb[conditionIterator],
                };
            }
            return conditions;
        }
    }
}