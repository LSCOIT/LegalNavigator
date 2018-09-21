import { APP_BASE_HREF } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NavigateDataService } from '../navigate-data.service';
import { Observable } from 'rxjs';
import { PrintButtonComponent } from '../../shared/resource/user-action/print-button/print-button.component';
import { ResourceCardComponent } from '../resource/resource-card/resource-card.component';
import { RouterModule, Router } from '@angular/router';
import { SaveButtonComponent } from '../../shared/resource/user-action/save-button/save-button.component';
import { SearchComponent } from './search.component';
import { SearchFilterComponent } from './search-filter/search-filter.component';
import { SearchFilterPipe } from './search-filter.pipe';
import { SearchResultsComponent } from './search-results/search-results.component';
import { SearchService } from './search.service';
import { ShareButtonComponent } from '../../shared/resource/user-action/share-button/share-button.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { of } from 'rxjs/observable/of';
import { By } from '@angular/platform-browser';
import { ILuisInput } from './search-results/search-results.model';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { Input } from '@angular/core/src/metadata/directives';
import { MsalService } from '@azure/msal-angular';

describe('SearchComponent', () => {
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;
  let router: RouterModule;
  let searchService: SearchService;
  let mockNavigateDataService;
  let mockSearchService;
  let mockRouter;
  let mockResponse;
  let mockSessionStorage;
  let mockMapLocationParsed;
  let mockMapLocation;
  let store;
  let msalService;

  beforeEach(async(() => {
   
    mockNavigateDataService = jasmine.createSpyObj(['setData']);
    mockSearchService = jasmine.createSpyObj(['search']);  
    mockRouter =
      {
        navigate: jasmine.createSpy('navigate'),
        navigateByUrl: jasmine.createSpy('navigateByUrl')
      };
 
   mockResponse =
      {
        "webResources": {
          "_type": "SearchResponse",
          "instrumentation": {
            "_type": "ResponseInstrumentation",
            "pingUrlBase": "https://www.bingapis.com",
            "pageLoadPingUrl": "https://www.bingapis.com/api/ping"
          },
          "queryContext": {
            "originalQuery": "family"
          },
          "webPages": {
            "webSearchUrl": "https://www.bing.com/search?q=family",
            "webSearchUrlPingSuffix": "DevEx,5442.1",
            "totalEstimatedMatches": 257000,
            "value": [
              {
                "id": "https://api.cognitive.microsoft.com/api/v7/#WebPages.0",
                "name": "Family Law - Pro Bono Net",
                "url": "https://www.probono.net/dc/family/",
                "urlPingSuffix": "DevEx,5099.1",
                "isFamilyFriendly": true,
                "displayUrl": "https://www.probono.net/dc/family",
                "snippet": "family snippet",
                "deepLinks": [
                  {
                    "name": "Family Justice/DV",
                    "url": "https://www.probono.net/ny/family/library/",
                    "urlPingSuffix": "DevEx,5087.1",
                    "snippet": "test snippet"
                  }
                ],
                "dateLastCrawled": "2018-09-03T13:26:00Z",
                "fixedPosition": false,
                "language": "en",
                "isNavigational": true
              }
            ]
          },
          "rankingResponse": {
            "mainline": {
              "items": [
                {
                  "answerType": "WebPages",
                  "resultIndex": 0,
                  "value": {
                    "id": "https://api.cognitive.microsoft.com/api/v7/#WebPages.0"
                  }
                }
              ]
            }
          }
        }
      }
    mockMapLocation = `{
      "state":"Alaska",
      "locality":"Alaska",
      "address":"Alaska"
    }`
    mockMapLocationParsed = {
      state: "Alaska",
      locality: "Alaska",
      address: "Alaska"
    }
    store = {};
    mockSessionStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      },
      removeItem: (key: string) => {
        delete store[key];
      }
    };
    spyOn(sessionStorage, 'getItem')
      .and.callFake(mockSessionStorage.getItem);
    spyOn(sessionStorage, 'removeItem')
      .and.callFake(mockSessionStorage.removeItem);
    TestBed.configureTestingModule({
      declarations: [
        SearchComponent, 
        SearchResultsComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        PrintButtonComponent,
        SearchFilterComponent, 
        ResourceCardComponent,
        SearchFilterPipe
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'search', component: SearchResultsComponent }
        ]),
        HttpClientModule, FormsModule],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        { provide: Router, useValue: mockRouter },
        { provide: SearchService, useValue: mockSearchService },
        { provide: NavigateDataService, useValue: mockNavigateDataService },
        { provide: MsalService, useValue: msalService },
        NgxSpinnerService
      ],
      schemas: [NO_ERRORS_SCHEMA,CUSTOM_ELEMENTS_SCHEMA]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should assign respose to searchresult property when search method of search service called', () => {
    let test = mockRouter.navigate('/search');
    mockRouter.navigateByUrl.and.returnValue(Promise.resolve(test));
    let mocklocationchange = Object({ skipLocationChange: true });
    let mockSearchForm = <NgForm>{
      value: {
        inputText: "family"
      }
    }
    store.globalMapLocation = mockMapLocation;
    mockSearchService.search.and.returnValue(of(mockResponse));
    component.onSubmit(mockSearchForm);
    expect(component.luisInput["Sentence"]).toEqual("family");
    expect(component.luisInput["Location"]).toEqual(mockMapLocationParsed);
    expect(mockNavigateDataService.setData).toHaveBeenCalled();
    expect(component.searchResults).toEqual(mockResponse);
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/searchRefresh', mocklocationchange );
    expect(mockRouter.navigate).toHaveBeenCalledWith('/search');
  });
});
