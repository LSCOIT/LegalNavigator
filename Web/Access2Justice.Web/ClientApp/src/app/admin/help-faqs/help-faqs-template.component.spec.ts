import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { FormsModule, NgForm, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { of } from "rxjs";

import { Global } from "../../global";
import { NavigateDataService } from "../../shared/services/navigate-data.service";
import { StaticResourceService } from "../../shared/services/static-resource.service";
import { AdminService } from "../admin.service";
import { HelpFaqsTemplateComponent } from "./help-faqs-template.component";

describe("HelpFaqsTemplateComponent", () => {
  let component: HelpFaqsTemplateComponent;
  let fixture: ComponentFixture<HelpFaqsTemplateComponent>;
  let mockStaticResourceService;
  let mockGlobal;
  let mockAdminService;
  let mockNgxSpinnerService;
  let mockRouter;
  let nullStaticContent;
  let mockActiveRoute;
  let mockNavigateDataService;
  let mockToastr;
  let mockStaticContent;
  let mockNewHelpAndFaqsContent;
  let formValue;
  let mockFaqForm;

  beforeEach(async(() => {
    mockToastr = jasmine.createSpyObj(["success"]);
    mockStaticResourceService = jasmine.createSpyObj(["getStaticContents"]);
    mockActiveRoute = {
      snapshot: {
        queryParams: {
          state: "Alaska"
        }
      }
    };
    nullStaticContent = undefined;
    mockNavigateDataService = jasmine.createSpyObj(["getData"]);
    mockNgxSpinnerService = jasmine.createSpyObj(["show", "hide"]);
    mockAdminService = jasmine.createSpyObj(["saveHelpAndFaqData"]);
    mockStaticContent = [
      {
        name: "HelpAndFAQPage",
        location: [
          {
            state: "Hawaii"
          }
        ],
        organizationalUnit: "Hawaii",
        description:
          "Lorem ipsum, dolor sit amet consectetur adipisicing elit. A non excepturi ullam magnam similique, nobis officiis cupiditate aperiam consectetur at labore commodi! Soluta, expedita fuga in quod corporis voluptatem reiciendis. ",
        image: {
          source:
            "/static-resource/alaska/assets/images/secondary-illustrations/help_faqs.svg",
          altText: "Help and FAQs Page"
        },
        faqs: [
          {
            question:
              "Lorem ipsum dolor sit amet consectetur adipisicing elit. Alias quo expedita, ab vero repellat voluptates molestiae? Ipsam, ducimus nam. Ducimus.",
            answer:
              "Lorem ipsum dolor sit amet consectetur adipisicing elit. Repellat reprehenderit laborum dicta architecto iusto at sed iste fugit necessitatibus rem, fuga ad minima provident neque est autem dolore officiis, ab nihil? Consectetur, nostrum nam eligendi praesentium illo dignissimos magni maxime."
          },
          {
            question:
              "Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae nam corporis laboriosam voluptate, neque eligendi?",
            answer:
              "Lorem ipsum dolor sit amet consectetur adipisicing elit. Enim iure cupiditate eius fugit eveniet sit deserunt velit esse repellendus totam quia, accusamus ipsum doloribus. Adipisci aliquam nostrum quibusdam similique alias itaque optio dolorum magni et eveniet architecto debitis quos inventore sit, quam non."
          }
        ]
      }
    ];
    formValue = <NgForm>{
      value: {
        helpPageDescription: "help page Description",
        faq1: "old faq",
        answer1: "old answer"
      }
    };
    mockFaqForm = {
      value: {
        faqs: [
          {
            question: "question 1",
            answer: "answer 1"
          }
        ]
      }
    };

    TestBed.configureTestingModule({
      declarations: [HelpFaqsTemplateComponent],
      imports: [FormsModule, ReactiveFormsModule],
      providers: [
        {
          provide: StaticResourceService,
          useValue: mockStaticResourceService
        },
        {
          provide: Global,
          useValue: mockGlobal
        },
        {
          provide: AdminService,
          useValue: mockAdminService
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
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HelpFaqsTemplateComponent);
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
    component.getHelpFaqPageContent();
    expect(mockStaticResourceService.getStaticContents).toHaveBeenCalledWith({
      state: "Alaska"
    });
    expect(component.staticContent).toEqual(mockStaticContent);
    expect(component.helpAndFaqsContent).toEqual(mockStaticContent[0]);
  });

  it("should set staticContent and helpAndFaqsContent when navigateDataService is defined", () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    component.getHelpFaqPageContent();
    expect(mockStaticResourceService.getStaticContents).toHaveBeenCalledTimes(
      0
    );
    expect(component.staticContent).toEqual(mockStaticContent);
    expect(component.helpAndFaqsContent).toEqual(mockStaticContent[0]);
  });

  it("should call createHelpAndFaqsParams and mapSectionDescription onSubmit", () => {
    component.faqForm = mockFaqForm;
    component.faqParams = [];
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    mockAdminService.saveHelpAndFaqData.and.returnValue(of({}));
    spyOn(component, "createHelpAndFaqsParams");
    spyOn(component, "mapSectionDescription");
    component.onSubmit(formValue);
    expect(component.mapSectionDescription).toHaveBeenCalledWith(
      formValue.value
    );
    expect(component.createHelpAndFaqsParams).toHaveBeenCalledWith(formValue);
  });

  it("should set newHelpAndFaqsContent and call saveHelpAndFaqData on submit", () => {
    component.faqForm = mockFaqForm;
    component.faqParams = [];
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    mockAdminService.saveHelpAndFaqData.and.returnValue(of({}));
    spyOn(component, "createHelpAndFaqsParams");
    component.onSubmit(formValue);
    expect(mockAdminService.saveHelpAndFaqData).toHaveBeenCalled();
  });
});
