import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { UpperNavComponent } from './upper-nav.component';
import { StaticResourceService } from '../../shared/services/static-resource.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Global } from '../../global';
import { MapService } from '../map/map.service';
import { HttpClientModule } from '@angular/common/http';
import { StateCodeService } from '../../shared/services/state-code.service';

describe('UpperNavComponent', () => {
  let component: UpperNavComponent;
  let fixture: ComponentFixture<UpperNavComponent>;
  let mockStaticResourceService;
  let navigation;
  let mockGlobal;
  let globalData;

  beforeEach(async(() => {
    navigation = {
      name: "Navigation",
      location: [
        { state: "Default" }
      ]
    };
    globalData = [
      {
        name: "Navigation",
        location: [
          {
             state: "Default"
          }
        ]
      }
    ];
    mockStaticResourceService = jasmine.createSpyObj(['getLocation', 'getStaticContents']);
    mockGlobal = jasmine.createSpyObj(['getData']);
    mockGlobal.getData.and.returnValue(globalData);
    
    TestBed.configureTestingModule({
      imports: [ HttpClientModule ],
      declarations: [ UpperNavComponent ],
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
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpperNavComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set about content to static resource about content if it exists', () => {
    mockStaticResourceService.getLocation.and.returnValue('Default');
    mockStaticResourceService.navigation = navigation;
    spyOn(component, 'filterUpperNavigationContent');
    component.getUpperNavigationContent();
    expect(component.navigation).toEqual(mockStaticResourceService.navigation);
    expect(component.filterUpperNavigationContent).toHaveBeenCalledWith(mockStaticResourceService.navigation);
  });
});
