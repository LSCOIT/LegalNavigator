import { ArticlesResourcesComponent } from './articles-resources.component';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NavigateDataService } from '../../shared/navigate-data.service';

describe('ArticlesResourcesComponent', () => {
  let component: ArticlesResourcesComponent;
  let fixture: ComponentFixture<ArticlesResourcesComponent>;
  let mockNavigateDataService;
  let mockGuidedAssistantResults;
  let mockGuidedAssistId = '9a6a6131-657d-467d-b09b-c570b7dad242'

  beforeEach(async(() => {
    mockNavigateDataService = jasmine.createSpyObj(['setData', 'getData'])

    mockGuidedAssistantResults = {
      "topIntent": "Divorce",
      "relevantIntents": [
        "Domestic Violence",
        "Tenant's rights",
        "None"
      ],
      "topicIds": [
        "e1fdbbc6-d66a-4275-9cd2-2be84d303e12"
      ],
      "guidedAssistantId": "9a6a6131-657d-467d-b09b-c570b7dad242"
    }

    TestBed.configureTestingModule({
      declarations: [ ArticlesResourcesComponent ],
      providers: [{ provide: NavigateDataService, useValue: mockNavigateDataService }, ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArticlesResourcesComponent);
    component = fixture.componentInstance;
    mockNavigateDataService.getData.and.returnValue(mockGuidedAssistantResults);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get guidedAssistantResults on ngOnInit', () => {
    spyOn(component, 'ngOnInit');
    component.ngOnInit();
    expect(component.ngOnInit).toHaveBeenCalled();
    expect(component.guidedAssistantResults).toEqual(mockGuidedAssistantResults);
    expect(component.guidedAssistantResults.guidedAssistantId).toEqual(mockGuidedAssistId);
  });

});
