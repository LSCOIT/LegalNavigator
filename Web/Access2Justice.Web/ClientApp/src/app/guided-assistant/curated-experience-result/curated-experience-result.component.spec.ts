import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CuratedExperienceResultComponent } from './curated-experience-result.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { ToastrService } from 'ngx-toastr';
import { By } from '@angular/platform-browser';

describe('CuratedExperienceResultComponent', () => {
  let component: CuratedExperienceResultComponent;
  let fixture: ComponentFixture<CuratedExperienceResultComponent>;
  let mockToastr;
  let mockNavigateDataService;
  let mockGuidedAssistantResults;

  beforeEach(async(() => { 
    mockNavigateDataService = jasmine.createSpyObj(['getData']);
    mockGuidedAssistantResults = {
      "topIntent": "Eviction",
      "relevantIntents": [
        "Domestic Violence",
        "Tenant's rights",
        "None"
      ]
    }
    mockNavigateDataService.getData.and.returnValue(mockGuidedAssistantResults);
    TestBed.configureTestingModule({
      declarations: [ CuratedExperienceResultComponent ],
      schemas: [ NO_ERRORS_SCHEMA ],
      providers: [ 
        { provide: NavigateDataService, useValue: mockNavigateDataService},
        { provide: ToastrService, useValue: mockToastr }
      ]
    })
    .compileComponents();
  }));
  
  beforeEach(() => {
    fixture = TestBed.createComponent(CuratedExperienceResultComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    component.guidedAssistantResults = mockGuidedAssistantResults;
  });
  
  it('should create', () => {
    expect(component).toBeTruthy();
  });
  
  it('should filter out relevant intents that are None', () => {
    component.filterIntent();
    expect(component.relevantIntents).toEqual(
      [
        "Domestic Violence",
        "Tenant's rights"
      ]
    )
  });
  
  // it('should call toastr when Save for Later button is clicked', () => {
  //   mockToastr = jasmine.createSpyObj(['success']);
  //   component.relevantIntents = [
  //     "Domestic Violence",
  //     "Tenant's rights"
  //   ];
  //   fixture.detectChanges();
  //   fixture.debugElement
  //     .query(By.css('.btn-secondary'))
  //     .triggerEventHandler('click', {stopPropogration: () => {}});
  //   // const button = fixture.debugElement.query(By.css('.btn'));
  //   // button.triggerEventHandler('click', {stopPropogration: () => {}});

  //   // expect(mockToastr.success).toHaveBeenCalled();
  // })
});
