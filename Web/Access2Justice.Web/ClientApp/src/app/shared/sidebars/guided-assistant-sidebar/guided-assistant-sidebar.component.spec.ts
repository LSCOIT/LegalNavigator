import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { GuidedAssistantSidebarComponent } from './guided-assistant-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { ModalModule, BsModalService } from 'ngx-bootstrap';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MapService } from '../../map/map.service';
import { NavigateDataService } from '../../services/navigate-data.service';
import { PaginationService } from '../../pagination/pagination.service';
import { Observable } from 'rxjs/Observable';
import { MapLocation } from '../../map/map';
import { IResourceFilter } from '../../search/search-results/search-results.model';
import { StateCodeService } from '../../services/state-code.service';

describe('GuidedAssistantSidebarComponent', () => {
  let component: GuidedAssistantSidebarComponent;
  let fixture: ComponentFixture<GuidedAssistantSidebarComponent>;
  let router: Router;
  let activateRouter: ActivatedRoute;
  let navigateDataService: NavigateDataService;
  let paginationService: PaginationService;
  let mockResourceType = 'Guided Assistant';
  let mockActivetopic = "bd900039-2236-8c2c-8702-d31855c56b0f";
  let mockEmpty = "";
  let mockMapLocation: MapLocation = {
    state: 'Sample State',
    city: 'Sample City',
    county: 'Sample County',
    zipCode: '1009203'
  };
  let mockresourceInput: IResourceFilter = {
    ResourceType: mockResourceType,
    ContinuationToken: '',
    TopicIds: [mockActivetopic],
    ResourceIds: [],
    PageNumber: 0,
    Location: mockMapLocation,
    IsResourceCountRequired: false,
    IsOrder: false,
    OrderByField: "",
    OrderBy: ""
  };
  let mockRouter = {
    navigate:jasmine.createSpyObj('Router', ['navigateByUrl'])

  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule, RouterTestingModule, ModalModule.forRoot()],
      declarations: [GuidedAssistantSidebarComponent],
      providers: [
        BsModalService,
        {
          provide: Router,
          useValue: mockRouter
        },        
        { provide: ActivatedRoute, 
          useValue: {
            snapshot: {
              params: {
                 'id': 'bd900039-2236-8c2c-8702-d31855c56b0f'
              }
            },
            url: Observable.of([
              {
                path: 'guidedassistant',
                params: {}
              },
              {
                path: 'bd900039-2236-8c2c-8702-d31855c56b0f',
                params: {}
              }
            ])
          }
        },
        MapService,
        NavigateDataService,
        PaginationService,
        StateCodeService
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GuidedAssistantSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    paginationService = TestBed.get(PaginationService);
    navigateDataService = TestBed.get(NavigateDataService);
    router = TestBed.get(Router);
    activateRouter = TestBed.get(ActivatedRoute);

    let store = {};
    const mockSessionStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      },
      setItem: (key: string, value: string) => {
        store[key] = `${value}`;
      },
      removeItem: (key: string) => {
        delete store[key];
      },
      clear: () => {
        store = {};
      }
    };

    spyOn(sessionStorage, 'setItem')
      .and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, 'removeItem')
      .and.callFake(mockSessionStorage.removeItem);
    spyOn(sessionStorage, 'clear')
      .and.callFake(mockSessionStorage.clear);
  });

  it('should create guided assistant sidebar component', () => {
    expect(component).toBeTruthy();
  });

  it('should assign session storage values in ngOnInit', () => {
    component.guidedAssistantId = undefined;
    let mockLocationDetails = { location: mockMapLocation };
    spyOn(sessionStorage, 'getItem')
      .and.returnValue(JSON.stringify(mockLocationDetails));
    component.activeTopic = mockActivetopic;
    spyOn(component, 'getGuidedAssistantResults');
    component.ngOnInit();
    expect(component.location).toEqual(mockMapLocation);
    expect(component.getGuidedAssistantResults).toHaveBeenCalled();
    sessionStorage.removeItem("globalMapLocation");
  });

  it('should return guidedAssistantResults when getPagedResource method of pagination service called', () => {
    let mockExternalUrl = "www.google.com";
    let mockResponse = {
      "resources": [{ "externalUrl": mockExternalUrl }]
    };
    spyOn(paginationService, 'getPagedResources').and.callFake(() => {
      return Observable.from([mockResponse])
    });
    component.location = mockMapLocation;
    component.activeTopic = mockActivetopic;
    component.getGuidedAssistantResults();
    expect(component.resourceFilter).toEqual(mockresourceInput);
    expect(component.guidedAssistantId).toBe(mockEmpty);
  });

  it('should return guidedAssistantResults when getPagedResource method of pagination service returns no data called', () => {
    let mockExternalUrl = "www.google.com";
    let mockResponse = {
      "resources": [{ "externalUrl": mockExternalUrl }]
    };
    spyOn(paginationService, 'getPagedResources').and.callFake(() => {
      return Observable.from([undefined])
    });
    component.location = mockMapLocation;
    component.activeTopic = mockActivetopic;
    component.getGuidedAssistantResults();
    expect(component.resourceFilter).toEqual(mockresourceInput);
    expect(component.guidedAssistantId).toBe(mockEmpty);
  });

  it('should return guidedAssistantResults when active topic is null', () => {
    component.activeTopic = null;
    component.getGuidedAssistantResults();
    expect(component.guidedAssistantId).toBe(mockEmpty);
  });

  it('should return guidedAssistantLinkResults when getPagedResource method of pagination service called', () => {
    spyOn(navigateDataService, 'setData');
    spyOn(paginationService, 'getPagedResources');
    paginationService.getPagedResources(mockresourceInput);
    expect(paginationService.getPagedResources).toHaveBeenCalled();
    expect(component.guidedAssistantId).toBeUndefined();
  });

});
