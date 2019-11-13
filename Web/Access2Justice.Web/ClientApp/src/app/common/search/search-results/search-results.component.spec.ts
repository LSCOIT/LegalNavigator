import { APP_BASE_HREF } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { BsDropdownModule } from 'ngx-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';

import { Global } from '../../../global';
import { PersonalizedPlanService } from '../../../guided-assistant/personalized-plan/personalized-plan.service';
import { PaginationComponent } from '../../pagination/pagination.component';
import { MapService } from '../../map/map.service';
import { PaginationService } from '../../pagination/pagination.service';
import { ResourceCardComponent } from '../../resource/resource-card/resource-card.component';
import { SaveButtonComponent } from '../../resource/user-action/save-button/save-button.component';
import { ShareButtonComponent } from '../../resource/user-action/share-button/share-button.component';
import { ArrayUtilityService } from '../../services/array-utility.service';
import { NavigateDataService } from '../../services/navigate-data.service';
import { StateCodeService } from '../../services/state-code.service';
import { GuidedAssistantSidebarComponent } from '../../sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../../sidebars/service-org-sidebar/service-org-sidebar.component';
import { ShowMoreService } from '../../sidebars/show-more/show-more.service';
import { SearchFilterComponent } from '../search-filter/search-filter.component';
import { SearchService } from '../search.service';
import { SearchResultsComponent } from './search-results.component';
import { WebResourceComponent } from './web-resource/web-resource.component';
import { SharedModule } from '../../../shared/shared.module';

describe('SearchResultsComponent', () => {
  let component: SearchResultsComponent;
  let fixture: ComponentFixture<SearchResultsComponent>;
  let mockPageNumber: number = 0;
  let mockResourceName = 'Videos';
  let mockResourceNameUndifined = 'NoVideos';
  let mockResourceTypeFilter = [
    {
      ResourceName: 'Videos',
      ResourceCount: 1
    },
    {
      ResourceName: 'Organizations',
      ResourceCount: 2
    },
    {
      ResourceName: 'Forms',
      ResourceCount: 1
    }
  ];
  let mockResourceTypeFilterTemp = [
    {
      ResourceName: 'test',
      ResourceCount: 1
    },
    {
      ResourceName: 'test',
      ResourceCount: 2
    },
    {
      ResourceName: 'test',
      ResourceCount: 1
    }
  ];
  let mockResourceListFilterAll = [
    {
      ResourceCount: 10,
      ResourceList: [
        {
          continuationToken: {},
          resources: [
            {
              address: 'Houston County, Texas, United States',
              name: 'Tenant Action Plan for Eviction'
            },
            {
              address: 'Houston County, Texas, United States',
              name: 'Landlord Action Plan'
            }
          ]
        }
      ],
      ResourceName: 'All'
    }
  ];
  let mockResourceTypeFilterWithResourceList = [
    {
      ResourceList: [
        {
          continuationToken: [
            {
              test: '2'
            }
          ],
          resources: [
            {
              address: 'Houston County, Texas, United States',
              name: 'Tenant Action Plan for Eviction'
            },
            {
              address: 'Houston County, Texas, United States',
              name: 'Landlord Action Plan'
            }
          ]
        }
      ]
    },
    {
      ResourceName: 'Organizations',
      ResourceCount: 2
    },
    {
      ResourceName: 'Forms',
      ResourceCount: 1
    }
  ];
  let mockResourceTypeFilterWithResourceListUndefined = [
    {
      ResourceList: undefined
    },
    {
      ResourceName: 'Organizations',
      ResourceCount: 2
    },
    {
      ResourceName: 'Forms',
      ResourceCount: 1
    }
  ];
  let mockResourceTypeFilter2 = [
    {
      ResourceName: 'Videos',
      ResourceCount: 1,
      ResourceList: [
        {
          continuationToken: [
            {
              test: '2'
            }
          ],
          resources: [
            {
              address: 'Houston County, Texas, United States',
              name: 'Tenant Action Plan for Eviction'
            },
            {
              address: 'Houston County, Texas, United States',
              name: 'Landlord Action Plan'
            }
          ]
        }
      ]
    },
    {
      ResourceName: 'Organizations',
      ResourceCount: 2
    },
    {
      ResourceName: 'Forms',
      ResourceCount: 1
    }
  ];
  let mockSearchText = 'eviction';
  let mockSearchResults2 = [
    {
      continuationToken: [
        {
          test: '2'
        }
      ],
      resources: [
        {
          address: 'Houston County, Texas, United States',
          name: 'Tenant Action Plan for Eviction'
        },
        {
          address: 'Houston County, Texas, United States',
          name: 'Landlord Action Plan'
        }
      ]
    }
  ];
  let mockToastr;
  let msalService;
  let mockMapLocation: any = {
    state: 'Sample State',
    city: 'Sample City',
    county: 'Sample County',
    zipCode: '1009203'
  };
  let mockPaginationService;
  let mockGlobal;
  beforeEach(async(() => {
    mockToastr = jasmine.createSpyObj(['success']);
    mockPaginationService = jasmine.createSpyObj([
      'getPagedResources',
      'searchByOffset'
    ]);
    TestBed.configureTestingModule({
      declarations: [
        SearchResultsComponent,
        SearchFilterComponent,
        ResourceCardComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        WebResourceComponent,
        PaginationComponent
      ],
      imports: [
        RouterModule.forRoot([
          {
            path: 'search',
            component: SearchResultsComponent
          }
        ]),
        HttpClientModule,
        SharedModule,
        BsDropdownModule.forRoot()
      ],
      providers: [
        NavigateDataService,
        SearchService,
        ShowMoreService,
        MapService,
        PersonalizedPlanService,
        ArrayUtilityService,
        NgxSpinnerService,
        SearchResultsComponent,
        StateCodeService,
        {
          provide: PaginationService,
          useValue: mockPaginationService
        },
        {
          provide: APP_BASE_HREF,
          useValue: '/'
        },
        {
          provide: ToastrService,
          useValue: mockToastr
        },
        {
          provide: Global,
          useValue: mockGlobal
        },
        {
          provide: MsalService,
          useValue: msalService
        }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
});
