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
import { HomeTemplateComponent } from "./home-template.component";

describe("HomeTemplateComponent", () => {
  let component: HomeTemplateComponent;
  let fixture: ComponentFixture<HomeTemplateComponent>;
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
  let formValue;

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
    mockAdminService = jasmine.createSpyObj(["saveHomeData"]);
    formValue = <NgForm>{
      value: {
        heroDescriptionText: "hero Description",
        guidedAssistantSteps1: "step 1 description",
        slideQuote0: "A2J Website is Awesome"
      }
    };
    mockStaticContent = [
      {
        name: "HomePage",
        location: [
          {
            state: "Alaska"
          }
        ],
        organizationalUnit: "Alaska",
        hero: {
          heading: "Get help with your legal questions in",
          description: {
            text: "We will always protect your privacy. Read our",
            textWithLink: {
              urlText: "Privacy Promise",
              url: "privacy"
            }
          },
          image: {
            source:
              "/static-resource/hawaii/assets/images/primary-illustrations/hawaii.svg",
            altText: "Illustration of Hawaii"
          }
        },
        guidedAssistantOverview: {
          heading:
            "Need a plan of action for your legal issue? Our Guided Assistant can help.",
          description: {
            steps: [
              {
                order: "1",
                description:
                  "Answer a few questions to help us understand your needs."
              },
              {
                order: "2",
                description:
                  "We'll create a personalized plan with steps and resources."
              },
              {
                order: "3",
                description:
                  "Share, print, or create a profile to save your plan."
              }
            ],
            text:
              "Free and no account necessary. We never share any information you provide.",
            textWithLink: {
              urlText: "Privacy Promise",
              url: "privacy"
            }
          },
          button: {
            buttonText: "Start the Guided Assistant",
            buttonAltText: "Start the Guided Assistant",
            buttonLink: "guidedassistant"
          },
          image: {
            source:
              "/static-resource/hawaii/assets/images/secondary-illustrations/guided_assistant.svg",
            altText: "Guided Assistant Illustration"
          }
        },
        topicAndResources: {
          heading: "More Information, Videos, and Links to Resources by Topic",
          button: {
            buttonText: "See More Topics",
            buttonAltText: "Alt text",
            buttonLink: "topics"
          }
        },
        carousel: {
          slides: [
            {
              quote:
                "Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel..",
              author: "Robbie",
              location: "Anchorage, AK",
              image: {
                source:
                  "/static-resource/alaska/assets/images/sample-images/trees_0.JPG",
                altText: "Image not available"
              }
            },
            {
              quote:
                "Ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat.",
              author: "Robbie",
              location: "Anchorage, AK",
              image: {
                source:
                  "/static-resource/alaska/assets/images/sample-images/trees_1.JPG",
                altText: "Image not available"
              }
            },
            {
              quote:
                "Ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor.",
              author: "Rob",
              location: "Anchorage",
              image: {
                source:
                  "/static-resource/alaska/assets/images/sample-images/trees_1.JPG",
                altText: "Image not available"
              }
            }
          ]
        },
        sponsorOverview: {
          heading: "Information you can trust",
          description:
            "This site is here to provide equal access to legal information and resources for everyone. Legal disclaimer lorem ipsum dolor amet. It is brought to you by partnership of these nonproffit organizations",
          sponsors: [
            {
              source: "",
              altText: ""
            },
            {
              source: "",
              altText: ""
            }
          ],
          button: {
            buttonText: "Learn More",
            buttonAltText: "Learn More",
            buttonLink: "about"
          }
        },
        privacy: {
          heading: "Our Privacy Promise",
          description:
            "We value your privacy and will never share your personal information with anyone without your lorem ipsum dolor sit amet.",
          button: {
            buttonText: "Learn More",
            buttonAltText: "Alt text",
            buttonLink: "privacy"
          },
          image: {
            source:
              "/static-resource/hawaii/assets/images/secondary-illustrations/privacy_promise.svg",
            altText: "Privacy Illustration"
          }
        },
        helpText: {
          beginningText: "Are you safe? Call",
          phoneNumber: "X-XXX-XXX-XXXX",
          endingText: "to get help."
        }
      }
    ];

    TestBed.configureTestingModule({
      declarations: [HomeTemplateComponent],
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
    fixture = TestBed.createComponent(HomeTemplateComponent);
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
    component.getHomePageContent();
    expect(mockStaticResourceService.getStaticContents).toHaveBeenCalledWith({
      state: "Alaska"
    });
    expect(component.staticContent).toEqual(mockStaticContent);
    expect(component.homeContent).toEqual(mockStaticContent[0]);
  });

  it("should set staticContent and home Content when navigateDataService is defined", () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    component.getHomePageContent();
    expect(mockStaticResourceService.getStaticContents).toHaveBeenCalledTimes(
      0
    );
    expect(component.staticContent).toEqual(mockStaticContent);
    expect(component.homeContent).toEqual(mockStaticContent[0]);
  });

  it("should call createHomeParams onSubmit", () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    mockAdminService.saveHomeData.and.returnValue(of({}));
    spyOn(component, "createHomeParams");
    component.getHomePageContent();
    component.onSubmit(formValue);
    expect(component.createHomeParams).toHaveBeenCalledWith(formValue);
  });

  it("should set newHomeContent and call saveHomeData on submit", () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    mockAdminService.saveHomeData.and.returnValue(of({}));
    component.getHomePageContent();
    component.onSubmit(formValue);
    expect(component.newHomeContent.hero.description.text).toEqual(
      "hero Description"
    );
    expect(
      component.newHomeContent.guidedAssistantOverview.description.steps[0]
        .description
    ).toEqual("step 1 description");
    expect(mockAdminService.saveHomeData).toHaveBeenCalled();
  });
});
