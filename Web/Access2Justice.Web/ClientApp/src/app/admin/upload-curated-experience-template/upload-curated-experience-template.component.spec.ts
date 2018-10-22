import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadCuratedExperienceTemplateComponent } from './upload-curated-experience-template.component';

describe('UploadCuratedExperienceTemplateComponent', () => {
  let component: UploadCuratedExperienceTemplateComponent;
  let fixture: ComponentFixture<UploadCuratedExperienceTemplateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadCuratedExperienceTemplateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadCuratedExperienceTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
