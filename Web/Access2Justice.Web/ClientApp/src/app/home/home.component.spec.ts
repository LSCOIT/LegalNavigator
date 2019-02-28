import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Global } from '../global';
import { MapService } from '../common/map/map.service';
import { StateCodeService } from '../common/services/state-code.service';
import { StaticResourceService } from '../common/services/static-resource.service';
import { HomeComponent } from './home.component';
import { SharedModule } from '../shared/shared.module';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let mockStaticResourceService;
  let homeContent;
  let mockGlobal;
  let globalData;

  beforeEach(async(() => {
    homeContent = {
      name: 'HomePage',
      location: [{state: 'Default'}],
      carousel: {
        slides: [
          {
            quote:
              'This project will help people understand that the legal system is a public service to use and employ that may help them.',
            author: '- Alaska community member',
            location: '',
            image: {
              source: 'test image',
              altText: 'Sample Image'
            }
          }
        ]
      }
    };
    globalData = [
      {
        name: 'HomePage',
        location: [{state: 'Default'}],
        carousel: {
          slides: [
            {
              quote:
                'This project will help people understand that the legal system is a public service to use and employ that may help them.',
              author: '- Alaska community member',
              location: '',
              image: {
                source: 'test image',
                altText: 'Sample Image'
              }
            }
          ]
        }
      }
    ];
    mockStaticResourceService = jasmine.createSpyObj([
      'getLocation',
      'getStaticContents'
    ]);
    mockGlobal = jasmine.createSpyObj(['getData']);
    mockGlobal.getData.and.returnValue(globalData);

    TestBed.configureTestingModule({
      imports: [HttpClientModule, SharedModule],
      declarations: [HomeComponent],
      providers: [
        MapService,
        StateCodeService,
        {
          provide: StaticResourceService,
          useValue: mockStaticResourceService
        },
        {
          provide: Global,
          useValue: mockGlobal
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
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
    expect(component.homeContent).toEqual(
      mockStaticResourceService.homeContent
    );
    expect(component.filterHomeContent).toHaveBeenCalledWith(
      mockStaticResourceService.homeContent
    );
  });
});
