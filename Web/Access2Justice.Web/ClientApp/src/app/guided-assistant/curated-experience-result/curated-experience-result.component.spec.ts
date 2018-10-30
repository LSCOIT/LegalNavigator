import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CuratedExperienceResultComponent } from './curated-experience-result.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { ToastrService } from 'ngx-toastr';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { MapService } from '../../shared/map/map.service';

describe('CuratedExperienceResultComponent', () => {
  let component: CuratedExperienceResultComponent;
  let fixture: ComponentFixture<CuratedExperienceResultComponent>;
  let mockToastr;
  let mockNavigateDataService;
  let mockGuidedAssistantResults;
  let mockRouter;
  beforeEach(async(() => { 
    mockNavigateDataService = jasmine.createSpyObj(['getData']);
    mockToastr = jasmine.createSpyObj(['success']);
    mockRouter = jasmine.createSpyObj(['navigateByUrl']);
    mockGuidedAssistantResults = {
      "topIntent": "Divorce",
      "relevantIntents": [
        "Domestic Violence",
        "Tenant's rights",
        "None"
      ],
      "topicIds":[  
        "e1fdbbc6-d66a-4275-9cd2-2be84d303e12"
     ],
     "guidedAssistantId":"9a6a6131-657d-467d-b09b-c570b7dad242"
    }
    TestBed.configureTestingModule({
      declarations: [ CuratedExperienceResultComponent ],
      schemas: [ NO_ERRORS_SCHEMA ],
      providers: [ 
        { provide: NavigateDataService, useValue: mockNavigateDataService},
        { provide: ToastrService, useValue: mockToastr },
        { provide: Router, useValue: mockRouter }, MapService
      ]
    })
    .compileComponents();
  }));
  
  beforeEach(() => {
    fixture = TestBed.createComponent(CuratedExperienceResultComponent);
    component = fixture.componentInstance;
    mockNavigateDataService.getData.and.returnValue(mockGuidedAssistantResults);
    component.ngOnInit();
    fixture.detectChanges();
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
  
  it('should call toastr when Save for Later button is clicked', () => {
    component.relevantIntents = [
      "Domestic Violence",
      "Tenant's rights"
    ];
    fixture.debugElement
      .query(By.css('.btn-secondary'))
      .triggerEventHandler('click', {stopPropogration: () => {}});
    const button = fixture.debugElement.query(By.css('.btn-secondary'));
    button.triggerEventHandler('click', {stopPropogration: () => {}});
    expect(mockToastr.success).toHaveBeenCalled();
  });
});
