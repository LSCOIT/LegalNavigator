using Access2Justice.DataImportTool.BusinessLogic;
using System;
using System.Collections.Generic;
using Xunit;

namespace Access2Justice.DataImportTool.Tests
{
    public class BusinessLogicTests
    {
        [Fact]
        public void Get_Ranking_Should_Return_1_Key_Value()
        {
            string ranking = "1: Dealing with: tolerance, etc";

            Dictionary<string, int> actualResult = InsertResources.GetRanking(ranking);

            Assert.True(actualResult.ContainsKey("Dealing with: tolerance, etc"));
            Assert.Equal(1, actualResult.GetValueOrDefault("Dealing with: tolerance, etc"));
        }

        [Fact]
        public void Get_Ranking_Should_Return_Number()
        {
            string ranking = "1";

            dynamic actualResult = InsertResources.GetRanking(ranking);

            Assert.Equal(1, (int)actualResult);
        }
        [Fact]
        public void Get_Ranking_Should_Return_2_Key_Values()
        {
            string ranking = "2: Domestic Violence and Abuse|5: Child Custody and Parenting Plans";

            Dictionary<string, int> actualResult = InsertResources.GetRanking(ranking);

            Assert.True(actualResult.ContainsKey("Domestic Violence and Abuse"));
            Assert.Equal(2, actualResult.GetValueOrDefault("Domestic Violence and Abuse"));

            Assert.True(actualResult.ContainsKey("Child Custody and Parenting Plans"));
            Assert.Equal(5, actualResult.GetValueOrDefault("Child Custody and Parenting Plans"));
        }

        [Fact]
        public void Get_Ranking_Should_Return_Multiple_Key_Values()
        {
            string ranking = "1: Eviction from a Home|2: Problems with Living Conditions|5: Child Support|5: Debt and Lending Money issues|5: Domestic Violence and Abuse|5: Paternity|5: Renting or Leasing a Home|4: Guardianship and Conservatorship|3: Going to court and dealing with procedure|3: Information help resources to deal with legal issues|3: Representing Oneself as Pro Se|3: Small Claims Actions|2: Child Custody and Parenting Plans|3: Legal Research|5: Name Change|2: Problems with Living Conditions";

            Dictionary<string, int> actualResult = InsertResources.GetRanking(ranking);

            Assert.True(actualResult.ContainsKey("Eviction from a Home"));
            Assert.Equal(1, actualResult.GetValueOrDefault("Eviction from a Home"));

            Assert.True(actualResult.ContainsKey("Problems with Living Conditions"));
            Assert.Equal(2, actualResult.GetValueOrDefault("Problems with Living Conditions"));

            Assert.Equal(5, actualResult.GetValueOrDefault("Child Support"));
        }   
    }
}
