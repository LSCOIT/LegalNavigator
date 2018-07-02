import { SearchService } from './search.service';
import { Observable } from 'rxjs/Rx';
import { api } from '../../../api/api';
import { ILuisInput } from '../search/search-results/search-results.model';

describe('SearchService', () => {
  let service: SearchService;
  const httpSpy = jasmine.createSpyObj('http', ['get']);
  let luisInput: ILuisInput = { Sentence: '', Location: '', TranslateFrom: '', TranslateTo: '' };  

  beforeEach(() => {
    service = new SearchService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('should return list of internal resources', (done) => {
    let resources = {
      topics: [
        {
          "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
          "name": "Family",
          "parentTopicID": "",
          "keywords": "EVICTION",
          "location": [
            {
              "state": "Hawaii",
              "county": "Kalawao County",
              "city": "Kalawao",
              "zipCode": "96742"
            } ],
          "jsonContent": "",
          "icon": "./assets/images/topics/topic14.png",
          "createdBy": "",
          "createdTimeStamp": "",
          "modifiedBy": "",
          "modifiedTimeStamp": ""
        }
      ],
      resources: [
        {
          "id": "77d301e7-6df2-612e-4704-c04edf271806",
          "name": "Tenant Action Plan for Eviction",
          "description": "This action plan is for tenants who are facing eviction and have experienced the following:",
          "resourceType": "Action",
          "externalUrl": "",
          "url": "",
          "topicTags": [
            {
              "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba"
            } ],
          "location": "Hawaii, Honolulu, 96812 | Alaska",
          "icon": "./assets/images/resources/resource.png",
          "createdBy": "",
          "createdTimeStamp": "",
          "modifiedBy": "",
          "modifiedTimeStamp": ""
        }]
    }

    const mockResponse = Observable.of(resources);

    httpSpy.get.and.returnValue(mockResponse);

    service.search(luisInput).subscribe(searchResponse => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${api.searchUrl}`);
      expect(searchResponse).toEqual(resources);
      done();
    });
  });

  it('should return luis response', (done) => {
    let luisResponse = {
      query: 'eviction',
      topScoringIntent: {
        intent: 'Eviction',
        score: 0.828598337
      },
      intents: [
        {
          intent: 'Eviction',
          score : 0.828598337
        },
        {
          intent: 'None',
          score : 0.3924698
        },
        {
          intent: 'Small Claims Court',
          score : 0.374458015
        }
      ],
      'entities': []
    }

    const mockResponse = Observable.of(luisResponse);

    httpSpy.get.and.returnValue(mockResponse);

    service.search(luisInput).subscribe(searchResponse => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${api.searchUrl}`);
      expect(searchResponse).toEqual(luisResponse);
      done();
    });
  });

  it('should return list of the web resources', (done) => {
    let webResources = {
      "webResources": {
        "_type": "SearchResponse",
        "instrumentation": {
          "_type": "ResponseInstrumentation",
      "pingUrlBase": "https://www.bingapis.com/api/ping",
      "pageLoadPingUrl": "https://www.bingapis.com/api"
        },
        "queryContext": {
          "originalQuery": "getting kicked out"
        },
        "webPages": {
        "webSearchUrl": "https://www.bing.com/search?q=getting+kicked+out",
        "webSearchUrlPingSuffix": "DevEx, 5388.1",
      "totalEstimatedMatches": 6,
        "value": [
          {
            "id": "https://api.cognitive.microsoft.com/api/v7/#WebPages.0",
            "name": "Mukesh and Another v State for NCT of Delhi and Others - Lawnotes.in",
            "url": "http://www.lawnotes.in/Mukesh_and_Another_v_State_for_NCT_of_Delhi_and_Others", 
          "urlPingSuffix": "DevEx,5076.1",
            "isFamilyFriendly": true,
            "displayUrl": " www.lawnotes.in / Mukesh_and_Another_v_State_for_NCT_of_Delhi_and_Others",
              "snippet": "Mukesh and Another v State ...",
            "deepLinks": [{}],
            "dateLastCrawled": "2018-05-19T08:31:00Z",
            "fixedPosition": false,
            "language": "en"
          }
        ]
      },
      "rankingResponse": {
        
    }
  }
}

    const mockResponse = Observable.of(webResources);

    httpSpy.get.and.returnValue(mockResponse);

    service.search(luisInput).subscribe(searchResponse => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${api.searchUrl}`);
      expect(searchResponse).toEqual(webResources);
      done();
    });

  });

});
