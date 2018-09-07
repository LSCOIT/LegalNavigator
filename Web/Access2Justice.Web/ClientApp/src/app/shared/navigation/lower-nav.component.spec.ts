import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LowerNavComponent } from './lower-nav.component';
import { StaticResourceService } from '../../shared/static-resource.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Global } from '../../global';
import { MapService } from '../map/map.service';

describe('LowerNavComponent', () => {
  let component: LowerNavComponent;
  let fixture: ComponentFixture<LowerNavComponent>;
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
    },
    globalData = [{
      name: "Navigation",
      location: [
        { state: "Default" }
      ]
    }]
    mockStaticResourceService = jasmine.createSpyObj(['getLocation', 'getStaticContents']);
    mockGlobal = jasmine.createSpyObj(['getData']);
    mockGlobal.getData.and.returnValue(globalData);
    
    TestBed.configureTestingModule({
      declarations: [ LowerNavComponent ],
      providers: [ 
        { provide: StaticResourceService, useValue: mockStaticResourceService },
        { provide: Global, useValue: mockGlobal },
        MapService
      ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LowerNavComponent);
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
    spyOn(component, 'filterNavigationContent');
    component.getNavigationContent();
    expect(component.navigation).toEqual(mockStaticResourceService.navigation);
    expect(component.filterNavigationContent).toHaveBeenCalledWith(mockStaticResourceService.navigation);
  });
});
