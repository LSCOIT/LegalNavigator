using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.DataFixes.DataAccess;
using Access2Justice.DataFixes.DataFixers;
using Access2Justice.DataFixes.Helpers;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;

namespace Access2Justice.DataFixes.DataFixers
{
    public class ResourcesRemoval : IssueFixerBase, IDataFixer
    {
        protected override string IssueId => "#__removeResources";

        public async Task ApplyFixAsync(
            CosmosDbSettings cosmosDbSettings,
            CosmosDbService cosmosDbService)
        {
            var namesList = new List<string>()
            {
"Child Support Information                                                                                                                  ",
"Mga Kaso ng Diborsyo at Pangangalaga: Impormasyon Para sa mga Kumakatawan sa Kanilang Sarili (Alaska Divorce & Custody Videos - Tagalog)   ",
"Casos de divorcio y custodia: Información para quienes se representan a sí mismo (Alaska Divorce & Custody Videos - Spanish)               ",
"Alaska Divorce &Custody Case Videos (18)                                                                                                   ",
"Appeals Process - Step by Step & Timeline                                                                                                  ",
"Marital Property & Debt Division Agreement [PDF]                                ",
"Marital Property & Debt Division Agreement [Word]                               ",
"Answer & Counterclaim to Legal Separation With Children                         ",
"Answer & Counterclaim to Legal Separation Without Children                      ",
"Motion & Affidavit to Convert Legal Separation to Divorce                       ",
"Marital Property and Debt Division Agreement & Order [Word]                     ",
"Marital Property and Debt Division Agreement & Order [PDF]                      ",
"Motion & Affidavit to Modify Custody, Visitation and/or Child Support [Word]    ",
"Motion & Affidavit to Modify Custody, Visitation and/or Child Support [PDF]     ",
"Appeal Affidavit & Memorandum                                                   ",
"Answer & Counterclaim to a Custody Complaint [Word]                             ",
"Answer & Counterclaim to a Custody Complaint [PDF]                              ",
"Answer & Counterclaim Packet to Custody Packet (for unmarried parents)          ",
"Answer & Counterclaim Packet to Divorce With Minor Children Packet              ",
"Answer & Counterclaim Packet to Divorce Without Minor Children Packet           ",
"Motions: Requesting and Order from the Court & Opposing the Request             ",
"After You Get the Final Order & Judgment                                        ",
"Rules of Appellate Procedure                                                    ",
"Rules of Appellate Procedure                                                    ",
"Motion to Modify Packet                                                         ",
"Order on Motion [Word]                                                          ",
"Order on Motion [PDF]                                                           ",
"Alaska Domestic Violence Resources by Community                                 ",
"Getting Ready for Hearing or Trial                                              ",
"Motions: Requesting an Order from the Court; Opposing a Motion                  ",
"Domestic Relations Trials - Understanding the Two Options                       "
            };

            namesList = namesList.Select(x => x.Trim()).ToList();

            List<Resource> resources =
                JsonHelper.Deserialize<List<Resource>>(
                    await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.ResourcesCollectionId));

            List<Resource> resourcesToDelete = resources.Where(x => namesList.Contains(x.Name)).ToList();

            foreach (var resource in resourcesToDelete)
            {
                await cosmosDbService.DeleteItemAsync(resource.ResourceId, resource.OrganizationalUnit, cosmosDbSettings.ResourcesCollectionId);
            }
        }
    }
}
