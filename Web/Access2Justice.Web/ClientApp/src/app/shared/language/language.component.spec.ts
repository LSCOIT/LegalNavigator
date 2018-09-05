import { async, ComponentFixture, TestBed, tick, fakeAsync } from '@angular/core/testing';
import { LanguageComponent } from './language.component';
import { StaticResourceService } from '../../shared/static-resource.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Global } from '../../global';
import { By } from '@angular/platform-browser';

describe('LanguageComponent', () => {
  let component: LanguageComponent;
  let fixture: ComponentFixture<LanguageComponent>;
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
    globalData = [{
      name: "Navigation",
      location: [
        { state: "Default" }
      ]
    }];
    mockStaticResourceService = jasmine.createSpyObj(['getLocation', 'getStaticContents']);
    mockGlobal = jasmine.createSpyObj(['getData']);
    mockGlobal.getData.and.returnValue(globalData);
    
    TestBed.configureTestingModule({
      declarations: [ LanguageComponent ],
      providers: [ 
        { provide: StaticResourceService, useValue: mockStaticResourceService },
        { provide: Global, useValue: mockGlobal }
      ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LanguageComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set about content to static resource language content if it exists', () => {
    mockStaticResourceService.getLocation.and.returnValue('Default');
    mockStaticResourceService.navigation = navigation;
    spyOn(component, 'filterLanguagueNavigationContent');
    component.getLanguagueNavigationContent();
    expect(component.navigation).toEqual(mockStaticResourceService.navigation);
    expect(component.filterLanguagueNavigationContent).toHaveBeenCalledWith(mockStaticResourceService.navigation);
  });

  it('should call addAttribute after view init', fakeAsync(() => {
    spyOn(component, 'addAttributes');
    component.ngAfterViewInit();
    tick();
    expect(component.addAttributes).toHaveBeenCalled();
  }));

  it('should toggle showLanguage and translator dropdown on click', fakeAsync(() => {
    let event = {
      target: {
        offsetParent: {
          id: "language"
        }
      }
    };
    let translator = document.getElementById('google_translate_element');
    component.showLanguage = true;
    component.onClick(event);
    expect(component.showLanguage).toBe(false);
    expect(translator.style.display).toEqual("none");
  }));
});
