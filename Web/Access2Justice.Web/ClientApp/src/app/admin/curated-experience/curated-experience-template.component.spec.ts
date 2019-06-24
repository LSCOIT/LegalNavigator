import { HttpClientModule } from "@angular/common/http";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { FormsModule, NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { CuratedExperienceTemplateComponent } from "./curated-experience-template.component";
import {CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA} from '@angular/core';
describe("CuratedExperienceTemplateComponent", () => {
  let component: CuratedExperienceTemplateComponent;
  let fixture: ComponentFixture<CuratedExperienceTemplateComponent>;
  let mockNgxSpinnerService;
  let mockRouter;
  let mockErrorMessage;

  beforeEach(async(() => {
    mockErrorMessage = "Please provide the required fields.";
    TestBed.configureTestingModule({
      declarations: [CuratedExperienceTemplateComponent],
      imports: [FormsModule, HttpClientModule],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
      providers: [
        {
          provide: NgxSpinnerService,
          useValue: mockNgxSpinnerService
        },
        {
          provide: Router,
          useValue: mockRouter
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CuratedExperienceTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should not submit without required field values", () => {
    let mockUploadForm = <NgForm>{
      value: {
        name: "Divorce",
        description: "Divorce Demo",
        file: null
      }
    };
    component.onSubmit(mockUploadForm);
    expect(component.errorMessage).toEqual(mockErrorMessage);
  });
});
