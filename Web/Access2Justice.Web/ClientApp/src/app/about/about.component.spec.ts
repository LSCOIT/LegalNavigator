import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AboutComponent } from './about.component';
import { StaticResourceService } from '../shared/services/static-resource.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Global } from '../global';

describe('AboutComponent', () => {
  let component: AboutComponent;
  let fixture: ComponentFixture<AboutComponent>;
  let mockStaticResourceService;
  let aboutContent;
  let mockGlobal;
  let globalData;

  beforeEach(async(() => {
    aboutContent = {
      name: "AboutPage",
      location: [
        { state: "Default" }
      ]
    };
    globalData = [
      {
        name: "AboutPage",
        location: [
          { state: "Default" }
        ]
      },
      {
        name: "HomePage",
        helpText: {}
      }
    ];
    mockStaticResourceService = jasmine.createSpyObj(
      [
        'getLocation',
        'getStaticContents'
      ]
    );
    mockGlobal = jasmine.createSpyObj(['getData']);
    mockGlobal.getData.and.returnValue(globalData);
    
    TestBed.configureTestingModule({
      declarations: [ AboutComponent ],
      providers: [ 
        {
          provide: StaticResourceService,
          useValue: mockStaticResourceService
        },
        {
          provide: Global,
          useValue: mockGlobal
        }
      ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set about content to static resource about content if it exists', () => {
    mockStaticResourceService.getLocation.and.returnValue('Default');
    mockStaticResourceService.aboutContent = aboutContent;
    component.getAboutPageContent();
    expect(component.aboutContent).toEqual(mockStaticResourceService.aboutContent);
  });
});
