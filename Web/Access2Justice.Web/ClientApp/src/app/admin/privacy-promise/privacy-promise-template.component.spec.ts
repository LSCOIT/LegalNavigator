import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { PrivacyPromiseTemplateComponent } from './privacy-promise-template.component';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Global } from '../../global';
import { AdminService } from '../admin.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { of } from 'rxjs/observable/of';

describe('PrivacyPromiseTemplateComponent', () => {
  let component: PrivacyPromiseTemplateComponent;
  let fixture: ComponentFixture<PrivacyPromiseTemplateComponent>;
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
  let mockNewPrivacyContent;
  let formValue;

  beforeEach(async(() => {
    formValue = <NgForm>{
      value: {
        description0: "Data may be collected to help improve the usability of the site",
        description1: "We do not collect any personal information.",
        description2: "Need language here",
        pageDescription: "Our privacy policy describes how we collect and use information about you. This policy doesn't apply to other sites you reach through Legal Navigator. You should read the privacy policies of other Web sites to learn how they collect and use information about you.↵↵We do not sell, trade, or rent your personal information to others. We may use the information:↵↵1. to train website specialists↵2. to evaluate help↵3. to report on the project's success↵4. to inform the development of new resources and tools for people in need of legal assistance↵5. to perform other administrative tasks",
        title0: "Data Collection and Use",
        title1: "Personal Information",
        title2: "Security"
      }
    };
    mockToastr = jasmine.createSpyObj(['success']);
    mockStaticResourceService = jasmine.createSpyObj(['getStaticContents']);
    mockActiveRoute = {
      snapshot: {
        queryParams: {
          state: "Alaska"
        }
      }
    }
    nullStaticContent = undefined;
    mockNavigateDataService = jasmine.createSpyObj(['getData']);
    mockNgxSpinnerService = jasmine.createSpyObj(['show', 'hide']);
    mockAdminService = jasmine.createSpyObj(['savePrivacyData']);
    mockStaticContent = [
      {
        "name": "HomePage",
        "location": [{ "state": "Alaska" }],
        "organizationalUnit": "Alaska",
        "hero": {},
        "guidedAssistantOverview": {},
        "topicAndResources": {},
        "carousel": { "slides": [] },
        "sponsorOverview": {},
        "privacy": {},
        "helpText": "Are you safe? Call X-XXX-XXX-XXXX to get help.",
        "id": "9c3019e9-ee2d-11ad-befa-ea7bfd06603f"
      },
      {
        "description":
          "Our privacy policy describes how we collect and use information about you. This policy doesn't apply to other sites you reach through Legal Navigator. You should read the privacy policies of other Web sites to learn how they collect and use information about you.\n\nWe do not sell, trade, or rent your personal information to others. We may use the information:\n\n1. to train website specialists\n2. to evaluate help\n3. to report on the project's success\n4. to inform the development of new resources and tools for people in need of legal assistance\n5. to perform other administrative tasks\n",
        "image": {
          "source": "/static-resource/alaska/assets/images/secondary-illustrations/privacy_promise_dark.svg",
          "altText": "Privacy Promise Page"
        },
        "details": [
          {
            "title": "Data Collection and Use",
            "description": "Data may be collected to help improve the usability of the site"
          },
          {
            "title": "Personal Information",
            "description": "We do not collect any personal information.  "
          },
          { "title": "Security", "description": "Need language here" }
        ],
        "organizationalUnit": "Alaska",
        "id": "",
        "name": "PrivacyPromisePage",
        "location": [
          {
            "state": "Alaska",
            "county": null,
            "city": null,
            "zipCode": null
          }
        ]
      }
    ];

    mockNewPrivacyContent = {
      description: "Our privacy policy describes how we collect and use information about you. This policy doesn't apply to other sites you reach through Legal Navigator. You should read the privacy policies of other Web sites to learn how they collect and use information about you.↵↵We do not sell, trade, or rent your personal information to others. We may use the information:↵↵1. to train website specialists↵2. to evaluate help↵3. to report on the project's success↵4. to inform the development of new resources and tools for people in need of legal assistance↵5. to perform other administrative tasks",
      details: [
        {
          title: 'Data Collection and Use',
          description: 'Data may be collected to help improve the usability of the site'
        },
        {
          title: 'Personal Information',
          description: 'We do not collect any personal information.'
        },
        {
          title: 'Security',
          description: 'Need language here'
        }
      ],
      name: "PrivacyPromisePage",
      location: [{
        state: "Alaska"
      }],
      image: {
        "source": "/static-resource/alaska/assets/images/secondary-illustrations/privacy_promise_dark.svg",
        "altText": "Privacy Promise Page"
      },
      organizationalUnit: "Alaska"
    }
    TestBed.configureTestingModule({
      declarations: [PrivacyPromiseTemplateComponent],
      imports: [FormsModule],
      providers: [
        { provide: StaticResourceService, useValue: mockStaticResourceService },
        { provide: Global, useValue: mockGlobal },
        { provide: AdminService, useValue: mockAdminService },
        { provide: NgxSpinnerService, useValue: mockNgxSpinnerService },
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: mockActiveRoute },
        { provide: NavigateDataService, useValue: mockNavigateDataService },
        { provide: ToastrService, useValue: mockToastr }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivacyPromiseTemplateComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call getStaticContents if no static content is found', () => {
    mockNavigateDataService.getData.and.returnValue(nullStaticContent);
    mockStaticResourceService.getStaticContents.and.returnValue(of(mockStaticContent));
    component.getPrivacyPageContent();
    expect(mockStaticResourceService.getStaticContents).toHaveBeenCalledWith({ state: "Alaska" });
    expect(component.staticContent).toEqual(mockStaticContent);
    expect(component.privacyContent).toEqual(mockStaticContent[1]);
  });

  it('should set staticContent and privacy Content when navigateDataService is defined', () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    component.getPrivacyPageContent();
    expect(mockStaticResourceService.getStaticContents).toHaveBeenCalledTimes(0);
    expect(component.staticContent).toEqual(mockStaticContent);
    expect(component.privacyContent).toEqual(mockStaticContent[1]);
  });

  it('should call mapSectionDescription onSubmit and set detailParams', () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    mockAdminService.savePrivacyData.and.returnValue(of({}));
    spyOn(component, 'mapSectionDescription');
    component.getPrivacyPageContent();
    component.onSubmit(formValue);
    expect(mockNgxSpinnerService.show).toHaveBeenCalled();
    expect(component.mapSectionDescription).toHaveBeenCalledWith(formValue.value);
  });

  it('should set detailParams when mapSectionDescription is called', () => {
    component.mapSectionDescription(formValue.value);
    expect(component.detailParams).toEqual(
      [
        {
          title: 'Data Collection and Use',
          description: 'Data may be collected to help improve the usability of the site'
        },
        {
          title: 'Personal Information',
          description: 'We do not collect any personal information.'
        },
        {
          title: 'Security',
          description: 'Need language here' 
        }
      ]
    );
  });

  it('should set newPrivacyContent and call savePrivacyData on submit', () => {
    mockNavigateDataService.getData.and.returnValue(mockStaticContent);
    mockAdminService.savePrivacyData.and.returnValue(of({}));
    component.getPrivacyPageContent();
    component.onSubmit(formValue);
    expect(component.newPrivacyContent).toEqual(mockNewPrivacyContent);
    expect(mockAdminService.savePrivacyData).toHaveBeenCalledWith(mockNewPrivacyContent);
  });
});
