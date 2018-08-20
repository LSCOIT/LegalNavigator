import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CuratedExperienceResultComponent } from './curated-experience-result.component';

describe('CuratedExperienceResultComponent', () => {
  let component: CuratedExperienceResultComponent;
  let fixture: ComponentFixture<CuratedExperienceResultComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CuratedExperienceResultComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CuratedExperienceResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
