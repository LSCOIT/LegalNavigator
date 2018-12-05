import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HomeComponent } from './home.component';
import { StaticResourceService } from '../shared/static-resource.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Global } from '../global';
import { MapService } from '../shared/map/map.service';
import { StateCodeService } from '../shared/state-code.service';
import { HttpClientModule } from '@angular/common/http';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let mockStaticResourceService;
  let homeContent;
  let mockGlobal;
  let globalData;

  beforeEach(async(() => {
    homeContent = {
      name: "HomePage",
      location: [
        { state: "Default" }
      ]
    },
    globalData = [{
      name: "HomePage",
      location: [
        { state: "Default" }
      ]
    }]
    mockStaticResourceService = jasmine.createSpyObj(['getLocation', 'getStaticContents']);
    mockGlobal = jasmine.createSpyObj(['getData']);
    mockGlobal.getData.and.returnValue(globalData);
    
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [ HomeComponent ],
      providers: [ 
        { provide: StaticResourceService, useValue: mockStaticResourceService },
        { provide: Global, useValue: mockGlobal },
        MapService,
        StateCodeService
      ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set help and faq content to static resource content if it exists', () => {
    mockStaticResourceService.getLocation.and.returnValue('Default');
    mockStaticResourceService.helpAndFaqsContent = homeContent;
    spyOn(component, 'filterHomeContent');
    component.getHomePageContent();
    expect(component.homeContent).toEqual(mockStaticResourceService.helpAndFaqsContent);
    expect(component.filterHomeContent).toHaveBeenCalledWith(mockStaticResourceService.helpAndFaqsContent);
  });
});
