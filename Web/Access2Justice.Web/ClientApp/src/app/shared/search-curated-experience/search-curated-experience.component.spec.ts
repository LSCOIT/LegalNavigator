import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { SearchCuratedExperienceComponent } from './search-curated-experience.component';
import { CuratedExperienceService } from './curatedexperience.service';

describe('SearchCuratedExperienceComponent', () => {
  let component: SearchCuratedExperienceComponent;
  let fixture: ComponentFixture<SearchCuratedExperienceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SearchCuratedExperienceComponent],
      providers: [
        CuratedExperienceService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchCuratedExperienceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
