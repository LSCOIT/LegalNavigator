import { NO_ERRORS_SCHEMA } from "@angular/core";
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
import { AboutTemplateComponent } from "./about-template.component";

describe("AboutAdminComponent", () => {
  let component: AboutTemplateComponent;
  let fixture: ComponentFixture<AboutTemplateComponent>;
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
    mockAdminService = jasmine.createSpyObj(["saveAboutData"]);
    mockStaticResourceService = jasmine.createSpyObj(["getStaticContents"]);
    mockNgxSpinnerService = jasmine.createSpyObj(["show", "hide"]);
    nullStaticContent = undefined;
    mockStaticContent = [
      {
        description: "",
        image: {
          source:
            "/static-resource/alaska/assets/images/secondary-illustrations/privacy_promise_dark.svg",
          altText: "Privacy Promise Page"
        },
        details: [
          {
            title: "Data Collection and Use",
            description:
              "Data may be collected to help improve the usability of the site"
          },
          {
            title: "Personal Information",
            description: "We do not collect any personal information.  "
          },
          {
            title: "Security",
            description: "Need language here"
          }
        ],
        organizationalUnit: "Alaska",
        id: "1ea905c9-5d37-5e9e-db15-34b2ffd300bc",
        name: "PrivacyPromisePage",
        location: [
          {
            state: "Alaska",
            county: null,
            city: null,
            zipCode: null
          }
        ]
      },
      {
        aboutImage: {
          source:
            "/static-resource/alaska/assets/images/secondary-illustrations/about.svg",
          altText: "About Page"
        },
        contactUs: {
          title: "Contact Us",
          description: "test contact us description",
          email: ""
        },
        inTheNews: {
          title: "In the News",
          description:
            "Follow project updates and media coverage of Legal Navigator on our project blog, SimplifyingLegalHelp.org.",
          news: [
            {
              title: "news1",
              desscription: "",
              image: {
                source: "",
                altText: ""
              }
            },
            {
              title: "news2",
              desscription: "",
              image: {
                source: "",
                altText: ""
              }
            },
            {
              title: "news3",
              desscription: "",
              image: {
                source: "",
                altText: ""
              }
            }
          ]
        },
        mission: {
          sponsors: [
            {
              source: "",
              altText: "sponsor1"
            },
            {
              source: "",
              altText: "sponsor2"
            }
          ],
          title: "Our Mission",
          description: ""
        },
        service: {
          image: {
            source: "",
            altText: "sample image"
          },
          url: "https://www.travelalaska.com/",
          title: "Lorem ipsum dolor sit amet2",
          description:
            "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter."
        },
        id: "ab0ef241-5778-0147-062c-005e80946d38",
        name: "AboutPage",
        location: [
          {
            state: "Alaska",
            county: null,
            city: null,
            zipCode: null
          }
        ],
        mediaInquiries: {
          title: "Media Inquiries",
          description: "",
          email: ""
        },
        privacyPromise: {
          description: "",
          image: {
            source: "",
            altText: ""
          },
          privacyPromiseButton: {
            buttonText: "View our privacy promise",
            buttonAltText: "View our privacy promise",
            buttonLink: "privacy"
          },
          title: "Our Privacy Promise"
        }
      }
    ];
    formValue = <NgForm>{
      value: {
        inTheNewsDescription: "test description",
        contactUs: {
          description: ""
        }
      }
    };

    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule],
      declarations: [AboutTemplateComponent],
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
    fixture = TestBed.createComponent(AboutTemplateComponent);
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
    component.getAboutPageContent();
    expect(mockStaticResourceService.getStaticContents).toHaveBeenCalledWith({
      state: "Alaska"
    });
    expect(component.staticContent).toEqual(mockStaticContent);
    expect(component.aboutContent).toEqual(mockStaticContent[1]);
    expect(component.createForm).toHaveBeenCalled();
  });

  it("should assign values to newAboutContent onSubmit - inTheNews description", () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    mockAdminService.saveAboutData.and.returnValue(of({}));
    component.getAboutPageContent();
    component.onSubmit(formValue);
    expect(component.newAboutContent.inTheNews.description).toEqual(
      formValue.value.inTheNewsDescription
    );
  });

  it("should take original about content if no new content was provided - contactUs description", () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    mockAdminService.saveAboutData.and.returnValue(of({}));
    component.getAboutPageContent();
    component.onSubmit(formValue);
    expect(component.newAboutContent.contactUs.description).toEqual(
      "test contact us description"
    );
  });
});
