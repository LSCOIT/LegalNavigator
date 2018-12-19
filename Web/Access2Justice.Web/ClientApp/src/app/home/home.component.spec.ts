import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HomeComponent } from './home.component';
import { StaticResourceService } from '../shared/static-resource.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Global } from '../global';
import { MapService } from '../shared/map/map.service';
import { StateCodeService } from '../shared/state-code.service';
import { HttpClientModule } from '@angular/common/http';
import { PipeModule } from '../shared/pipe/pipe.module';

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
      ],
      carousel: {
        slides: [
          {
            quote: "This project will help people understand that the legal system is a public service to use and employ that may help them.",
            author: "- Alaska community member",
            location: "",
            image: {
              source: "test image",
              altText: "Sample Image"
            }
          }
        ]
      }
    }

    globalData = [
      {
        name: "HomePage",
        location: [
          { state: "Default" }
        ],
        carousel: {
          slides: [
            {
              quote: "This project will help people understand that the legal system is a public service to use and employ that may help them.",
              author: "- Alaska community member",
              location: "",
              image: {
                source: "test image",
                altText: "Sample Image"
              }
            }
          ]
        }
      }
    ];
    mockStaticResourceService = jasmine.createSpyObj(['getLocation', 'getStaticContents']);
    mockGlobal = jasmine.createSpyObj(['getData']);
    mockGlobal.getData.and.returnValue(globalData);
    
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        PipeModule.forRoot()
        ],
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
    component.slides = [];
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set home content to static resource content if it exists', () => {
    mockStaticResourceService.getLocation.and.returnValue('Default');
    mockStaticResourceService.homeContent = homeContent;
    spyOn(component, 'filterHomeContent');
    component.getHomePageContent();
    expect(component.homeContent).toEqual(mockStaticResourceService.homeContent);
    expect(component.filterHomeContent).toHaveBeenCalledWith(mockStaticResourceService.homeContent);
  });
});
