using Access2Justice.Api.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Api.Tests.TestData
{
    class StateProvinceTestData
    {
        public static IEnumerable<object[]> GetStateCodesTestData()
        {
            yield return new object[] { JArray.Parse(@"[{
              'id': '28570257-8c35-f041-90f4-7d8884d4432e',
              'stateProvinces': [
                {
                  'code': 'HI',
                  'name': 'Hawaii'
                },
                {
                  'code': 'AK',
                  'name': 'Alaska'
                }
              ],
              'type': 'stateProvince'
            }]"),
              new List<StateCode>{ new StateCode { Code= "HI", Name="Hawaii"}, new StateCode { Code = "AK", Name = "Alaska" } } };
            yield return new object[] { JArray.Parse(@"[]"), null };
        }

        public static IEnumerable<object[]> GetStateNameTestData()
        {
            yield return new object[] { "Hawaii", JArray.Parse(@"[
                {
                  'code': 'HI',
                  'name': 'Hawaii'
                }]"), "HI" };
            yield return new object[] {"", JArray.Parse(@"[]"), null };
        }

        public static IEnumerable<object[]> GetStateCodeTestData()
        {
            yield return new object[] { "HI", JArray.Parse(@"[
                {
                  'code': 'HI',
                  'name': 'Hawaii'
                }]"), "Hawaii" };
            yield return new object[] { "", JArray.Parse(@"[]"), null };
        }
    }
}
