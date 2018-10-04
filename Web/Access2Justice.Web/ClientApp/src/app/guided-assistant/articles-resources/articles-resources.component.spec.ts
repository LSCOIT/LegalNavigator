import { ArticlesResourcesComponent } from './articles-resources.component';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NavigateDataService } from '../../shared/navigate-data.service';

describe('ArticlesResourcesComponent', () => {
  let component: ArticlesResourcesComponent;
  let fixture: ComponentFixture<ArticlesResourcesComponent>;
  let mockNavigateDataService;

  beforeEach(async(() => {
    mockNavigateDataService = jasmine.createSpyObj(['setData', 'getData'])

    TestBed.configureTestingModule({
      declarations: [ ArticlesResourcesComponent ],
      providers: [{ provide: NavigateDataService, useValue: mockNavigateDataService }, ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArticlesResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get guidedAssistantResults on ngOnInit', () => {
    spyOn(component, 'ngOnInit');
    let mockResults = 'testresults';    
    component.guidedAssistantResults = mockResults;
    component.ngOnInit();
    expect(component.ngOnInit).toHaveBeenCalled();
    expect(component.guidedAssistantResults).toEqual(mockResults);
  });

});
