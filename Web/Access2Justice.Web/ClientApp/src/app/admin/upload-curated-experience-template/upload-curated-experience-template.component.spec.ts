import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { UploadCuratedExperienceTemplateComponent } from './upload-curated-experience-template.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';

describe('UploadCuratedExperienceTemplateComponent', () => {
  let component: UploadCuratedExperienceTemplateComponent;
  let fixture: ComponentFixture<UploadCuratedExperienceTemplateComponent>;
  let mockNgxSpinnerService;
  let mockRouter;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UploadCuratedExperienceTemplateComponent],
      imports: [FormsModule, HttpClientModule],
      providers: [
        { provide: NgxSpinnerService, useValue: mockNgxSpinnerService },
        { provide: Router, useValue: mockRouter }
      ]
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
