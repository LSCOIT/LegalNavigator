import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CuratedExperienceComponent } from './curated-experience.component';

describe('CuratedExperienceComponent', () => {
  let component: CuratedExperienceComponent;
  let fixture: ComponentFixture<CuratedExperienceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CuratedExperienceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CuratedExperienceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
