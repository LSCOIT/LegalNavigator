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
            string path = "C:\\Users\\v-sobhad\\Desktop\\reading-csv\\Resource_Data_tab.txt";
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
                int len = parts.Length;
                string val;
                int j = 0, q = 0;

                while ((line2 = streamReader.ReadLine()) != null)
                {
                    List<string> value = new List<string>();
                    string[] partsb = line2.Split('\t');
                    Conditions[] conditions = null;
                    ReferenceTag[] referenceTags = null;
                    List<string> reference_Id = new List<string>();
                    List<string> condition_Id = new List<string>();
                    List<Locations> locations = new List<Locations>();
                    List<string> loc_Id = new List<string>();
                    List<string> locations_Id = new List<string>();

                    for (int i = 0; i < partsb.Length; i++)
                    {
                        val = parts[i];
                        if (val.EndsWith("referenceTags", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string tempReferenceId = partsb[i];
                            reference_Id.Add(partsb[i]);
                            string[] referencesb = null;
                            referencesb = tempReferenceId.Split('|');
                            referenceTags = new ReferenceTag[referencesb.Length];
                            for (int m = 0; m < referencesb.Length; m++)
                            {
                                referenceTags[m] = new ReferenceTag()
                                {
                                    ReferenceTags = referencesb[m],
                                };
                            }
                        }

                        else if (val.EndsWith("location", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string templocId = partsb[i];
                            loc_Id.Add(partsb[i]);
                            string[] locsb = null;
                            locsb = templocId.Split('|');
                            for (int m = 0; m < locsb.Length; m++)
                            {
                                string tempLocationsId = locsb[m];
                                locations_Id.Add(locsb[m]);
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
                        }

                        else if (val.EndsWith("conditions", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string tempconditionId = partsb[i];
                            condition_Id.Add(partsb[i]);
                            string[] conditionsb = null;
                            conditionsb = tempconditionId.Split('|');
                            conditions = new Conditions[conditionsb.Length];
                            for (int m = 0; m < conditionsb.Length; m++)
                            {
                                conditions[m] = new Conditions()
                                {
                                    Condition = conditionsb[m],
                                };
                            }
                        }

                        else
                        {
                            value.Add(partsb[i]);
                        }
                        j++;
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
                    q++;
                }
            }
            Resources.ResourcesList = ResourcesList.ToList();
            return Resources;
        }
    }
}