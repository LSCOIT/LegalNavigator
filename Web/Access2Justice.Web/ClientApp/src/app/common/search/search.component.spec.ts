import { APP_BASE_HREF } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { Router, RouterModule } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { BsDropdownModule } from 'ngx-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { of } from 'rxjs';

import { PrintButtonComponent } from '../resource/user-action/print-button/print-button.component';
import { SaveButtonComponent } from '../resource/user-action/save-button/save-button.component';
import { ShareButtonComponent } from '../resource/user-action/share-button/share-button.component';
import { ResourceCardComponent } from '../resource/resource-card/resource-card.component';
import { NavigateDataService } from '../services/navigate-data.service';
import { SearchFilterComponent } from './search-filter/search-filter.component';
import { SearchResultsComponent } from './search-results/search-results.component';
import { SearchComponent } from './search.component';
import { SearchService } from './search.service';
import { SharedModule } from '../../shared/shared.module';

describe('SearchComponent', () => {
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;
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
    mockRouter = {
      navigate: jasmine.createSpy('navigate'),
      navigateByUrl: jasmine.createSpy('navigateByUrl')
    };
    mockResponse = {
      webResources: {
        _type: 'SearchResponse',
        instrumentation: {
          _type: 'ResponseInstrumentation',
          pingUrlBase: 'https://www.bingapis.com',
          pageLoadPingUrl: 'https://www.bingapis.com/api/ping'
        },
        queryContext: {
          originalQuery: 'family'
        },
        webPages: {
          webSearchUrl: 'https://www.bing.com/search?q=family',
          webSearchUrlPingSuffix: 'DevEx,5442.1',
          totalEstimatedMatches: 257000,
          value: [
            {
              id: 'https://api.cognitive.microsoft.com/api/v7/#WebPages.0',
              name: 'Family Law - Pro Bono Net',
              url: 'https://www.probono.net/dc/family/',
              urlPingSuffix: 'DevEx,5099.1',
              isFamilyFriendly: true,
              displayUrl: 'https://www.probono.net/dc/family',
              snippet: 'family snippet',
              deepLinks: [
                {
                  name: 'Family Justice/DV',
                  url: 'https://www.probono.net/ny/family/library/',
                  urlPingSuffix: 'DevEx,5087.1',
                  snippet: 'test snippet'
                }
              ],
              dateLastCrawled: '2018-09-03T13:26:00Z',
              fixedPosition: false,
              language: 'en',
              isNavigational: true
            }
          ]
        },
        rankingResponse: {
          mainline: {
            items: [
              {
                answerType: 'WebPages',
                resultIndex: 0,
                value: {
                  id: 'https://api.cognitive.microsoft.com/api/v7/#WebPages.0'
                }
              }
            ]
          }
        }
      }
    };
    mockMapLocation = {
      location: {
        state: 'Alaska'
      }
    };
    mockMapLocationParsed = {
      state: undefined,
      county: undefined,
      city: undefined,
      zipCode: undefined
    };
    store = {};
    mockSessionStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      },
      removeItem: (key: string) => {
        delete store[key];
      }
    };

    spyOn(sessionStorage, 'removeItem').and.callFake(
      mockSessionStorage.removeItem
    );
    TestBed.configureTestingModule({
      declarations: [
        SearchComponent,
        SearchResultsComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        PrintButtonComponent,
        SearchFilterComponent,
        ResourceCardComponent
      ],
      imports: [
        RouterModule.forRoot([
          {
            path: 'search',
            component: SearchResultsComponent
          }
        ]),
        HttpClientModule,
        FormsModule,
        SharedModule,
        BsDropdownModule.forRoot()
      ],
      providers: [
        NgxSpinnerService,
        {
          provide: APP_BASE_HREF,
          useValue: '/'
        },
        {
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: SearchService,
          useValue: mockSearchService
        },
        {
          provide: NavigateDataService,
          useValue: mockNavigateDataService
        },
        {
          provide: MsalService,
          useValue: msalService
        }
      ],
      schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create', () => {
    expect(component).toBeDefined();
  });

  it('should assign respose to searchresult property when search method of search service called', () => {
    let test = mockRouter.navigate('/search');
    mockRouter.navigateByUrl.and.returnValue(Promise.resolve(test));
    let mocklocationchange = Object({skipLocationChange: true});
    let mockSearchForm = <NgForm>{
      value: {
        inputText: 'family'
      }
    };
    spyOn(sessionStorage, 'getItem').and.returnValue(
      JSON.stringify(mockMapLocation)
    );
    mockSearchService.search.and.returnValue(of(mockResponse));
    component.onSubmit(mockSearchForm);
    expect(component.luisInput['Sentence']).toEqual('family');
    expect(component.luisInput['Location']).toEqual(mockMapLocationParsed);
    expect(mockNavigateDataService.setData).toHaveBeenCalled();
    expect(component.searchResults).toEqual(mockResponse);
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith(
      '/searchRefresh',
      mocklocationchange
    );
    expect(mockRouter.navigate).toHaveBeenCalledWith('/search');
  });

  it('should be respose be null  when search method of search service called', () => {
    let mockSearchForm = <NgForm>{
      value: {
        inputText: 'family'
      }
    };
    mockRouter.navigateByUrl.and.returnValue(Promise.resolve(undefined));
    mockSearchService.search.and.returnValue(of(undefined));
    component.onSubmit(mockSearchForm);
    expect(component.searchResults).not.toEqual(mockResponse);
  });
});
