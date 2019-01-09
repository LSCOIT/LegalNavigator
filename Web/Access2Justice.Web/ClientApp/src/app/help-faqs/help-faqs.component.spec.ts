import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HelpFaqsComponent } from './help-faqs.component';
import { StaticResourceService } from '../shared/services/static-resource.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Global } from '../global';
import { StateCodeService } from '../shared/services/state-code.service';

describe('HelpFaqsComponent', () => {
  let component: HelpFaqsComponent;
  let fixture: ComponentFixture<HelpFaqsComponent>;
  let mockStaticResourceService;
  let helpAndFaqsContent;
  let mockGlobal;
  let globalData;

  beforeEach(async(() => {
    helpAndFaqsContent = {
      name: "HelpAndFAQPage",
      location: [
        { state: "Default" }
      ]
    };
    globalData = [
      {
        name: "HelpAndFAQPage",
        location: [
          { state: "Default" }
        ]
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
      declarations: [ HelpFaqsComponent ],
      providers: [ 
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
    fixture = TestBed.createComponent(HelpFaqsComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set help and faq content to static resource content if it exists', () => {
    mockStaticResourceService.getLocation.and.returnValue('Default');
    mockStaticResourceService.helpAndFaqsContent = helpAndFaqsContent;
    spyOn(component, 'filterHelpAndFaqContent');
    component.getHelpFaqPageContent();
    expect(component.helpAndFaqsContent).toEqual(mockStaticResourceService.helpAndFaqsContent);
    expect(component.filterHelpAndFaqContent).toHaveBeenCalledWith(mockStaticResourceService.helpAndFaqsContent);
  });
});
