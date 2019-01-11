import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { FormsModule, NgForm, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { of } from "rxjs/observable/of";
import { Global } from "../../global";
import { NavigateDataService } from "../../shared/services/navigate-data.service";
import { StaticResourceService } from "../../shared/services/static-resource.service";
import { AdminService } from "../admin.service";
import { PersonalizedPlanTemplateComponent } from "./personalized-plan-template.component";

describe("PersonalizedPlanTemplateComponent", () => {
  let component: PersonalizedPlanTemplateComponent;
  let fixture: ComponentFixture<PersonalizedPlanTemplateComponent>;
  let mockStaticResource;
  let mockGlobal;
  let mockNgxSpinnerService;
  let mockActiveRoute;
  let mockRouter;
  let mockNavigateDataService;
  let mockToastr;
  let mockAdminService;
  let nullStaticContent;
  let mockStaticResourceService;
  let mockStaticContent;
  let formValue;

  beforeEach(async(() => {
    mockActiveRoute = {
      snapshot: {
        queryParams: {
          state: "Alaska"
        }
      }
    };
    mockNavigateDataService = jasmine.createSpyObj(["getData"]);
    mockToastr = jasmine.createSpyObj(["success"]);
    mockAdminService = jasmine.createSpyObj(["savePersonalizedPlanData"]);
    mockStaticResourceService = jasmine.createSpyObj(["getStaticContents"]);
    mockNgxSpinnerService = jasmine.createSpyObj(["show", "hide"]);
    nullStaticContent = undefined;
    mockStaticContent = [
      {
        description: "orginal description",
        organizationalUnit: "Alaska",
        id: "123456789",
        name: "PersonalizedActionPlanPage",
        location: [
          {
            state: "Alaska",
            county: null,
            city: null,
            zipCode: null
          }
        ],
        sponsors: [
          {
            source: "",
            altText: "sponsor1"
          },
          {
            source: "",
            altText: "sponsor2"
          }
        ]
      }
    ];
    formValue = <NgForm>{
      value: {
        personalizedPlanDescription: "test description"
      }
    };

    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule],
      declarations: [PersonalizedPlanTemplateComponent],
      providers: [
        {
          provide: StaticResourceService,
          useValue: mockStaticResource
        },
        {
          provide: Global,
          useValue: mockGlobal
        },
        {
          provide: NgxSpinnerService,
          useValue: mockNgxSpinnerService
        },
        {
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: ActivatedRoute,
          useValue: mockActiveRoute
        },
        {
          provide: NavigateDataService,
          useValue: mockNavigateDataService
        },
        {
          provide: ToastrService,
          useValue: mockToastr
        },
        {
          provide: AdminService,
          useValue: mockAdminService
        },
        {
          provide: StaticResourceService,
          useValue: mockStaticResourceService
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonalizedPlanTemplateComponent);
    component = fixture.componentInstance;
    spyOn(component, "ngOnInit");
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should call getStaticContents if no static content is found", () => {
    mockNavigateDataService.getData.and.returnValue(nullStaticContent);
    mockStaticResourceService.getStaticContents.and.returnValue(
      of(mockStaticContent)
    );
    spyOn(component, "createForm");
    component.getPersonalizedPlanPageContent();
    expect(mockStaticResourceService.getStaticContents).toHaveBeenCalledWith({
      state: "Alaska"
    });
    expect(component.staticContent).toEqual(mockStaticContent);
    expect(component.personalizedPlanContent).toEqual(mockStaticContent[0]);
    expect(component.createForm).toHaveBeenCalled();
  });

  it("should assign values to newPersonalizedPlanContent onSubmit - description", () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    mockAdminService.savePersonalizedPlanData.and.returnValue(of({}));
    component.getPersonalizedPlanPageContent();
    component.onSubmit(formValue);
    expect(component.newPersonalizedPlanContent.description).toEqual(
      formValue.value.personalizedPlanDescription
    );
  });

  it("should take original personalized plan description if none was provided ", () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    mockAdminService.savePersonalizedPlanData.and.returnValue(of({}));
    component.getPersonalizedPlanPageContent();
    component.onSubmit(formValue);
    expect(component.newPersonalizedPlanContent.description).toEqual(
      "test description"
    );
  });
});
