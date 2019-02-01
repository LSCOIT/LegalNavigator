import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { ModalModule } from "ngx-bootstrap";
import { BsModalService } from "ngx-bootstrap/modal";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { of } from "rxjs";

import { QuestionControlService } from "../question-control.service";
import { IntakeFormComponent } from "./intake-form.component";
import { IntakeQuestionService } from "./intake-question-service/intake-question.service";

describe("IntakeFormComponent", () => {
  let component: IntakeFormComponent;
  let fixture: ComponentFixture<IntakeFormComponent>;
  let mockQuestionControlService;
  let mockIntakeQuestionService;
  let mockToastr;
  let mockNgxSpinnerService;
  let mockActiveRoute;

  beforeEach(async(() => {
    mockIntakeQuestionService = jasmine.createSpyObj(["getIntakeQuestions"]);
    mockQuestionControlService = jasmine.createSpyObj(["toFormGroup"]);
    mockToastr = jasmine.createSpyObj(["success"]);
    mockNgxSpinnerService = jasmine.createSpyObj(["show", "hide"]);
    mockActiveRoute = {
      snapshot: {
        params: {
          id: "123-456-7890"
        }
      }
    };
    TestBed.configureTestingModule({
      declarations: [IntakeFormComponent],
      imports: [FormsModule, ReactiveFormsModule, ModalModule.forRoot()],
      providers: [
        BsModalService,
        {
          provide: QuestionControlService,
          useValue: mockQuestionControlService
        },
        {
          provide: IntakeQuestionService,
          useValue: mockIntakeQuestionService
        },
        {
          provide: ToastrService,
          useValue: mockToastr
        },
        {
          provide: NgxSpinnerService,
          useValue: mockNgxSpinnerService
        },
        {
          provide: ActivatedRoute,
          useValue: mockActiveRoute
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IntakeFormComponent);
    component = fixture.componentInstance;
    spyOn(component, "ngOnInit");
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should set parameters when getIntakeQuestion is called", () => {
    let mockResponse = {
      userFields: [
        {
          name: "First Name",
          value: ""
        },
        {
          name: "Last Name",
          value: ""
        },
        {
          name: "Gender",
          value: ""
        }
      ],
      payloadTemplate:
        "<html><body><h1>The following information was submitted through the Legal Navigator portal:</h1><ul>Here are the details submitted as part of the onboarding process.<li>First Name: @First Name@</li><li>Last Name: @Last Name@</li><li>Gender: @Gender@</li><li>Age: @Age@</li><li>Telephone: @Telephone@</li></ul></body></html>",
      deliveryMethod: 1,
      deliveryDestination: null
    };
    mockIntakeQuestionService.getIntakeQuestions.and.returnValue(
      of(mockResponse)
    );
    mockQuestionControlService.toFormGroup.and.returnValue({});
    component.getIntakeQuestions();
    expect(component.payLoad).toEqual(mockResponse);
    expect(component.intakeQuestions).toEqual(mockResponse.userFields);
  });
});
