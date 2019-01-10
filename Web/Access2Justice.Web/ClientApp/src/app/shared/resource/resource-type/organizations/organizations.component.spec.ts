import { HttpClientModule, HttpHandler } from "@angular/common/http";
import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { MsalService } from "@azure/msal-angular";
import { BsModalService } from "ngx-bootstrap";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { Global } from "../../../../global";
import { PersonalizedPlanService } from "../../../../guided-assistant/personalized-plan/personalized-plan.service";
import { ArrayUtilityService } from "../../../services/array-utility.service";
import { NavigateDataService } from "../../../services/navigate-data.service";
import { MapResultsComponent } from "../../../sidebars/map-results/map-results.component";
import { MapResultsService } from "../../../sidebars/map-results/map-results.service";
import { UserActionSidebarComponent } from "../../../sidebars/user-action-sidebar/user-action-sidebar.component";
import { DownloadButtonComponent } from "../../user-action/download-button/download-button.component";
import { PrintButtonComponent } from "../../user-action/print-button/print-button.component";
import { SaveButtonComponent } from "../../user-action/save-button/save-button.component";
import { SaveButtonService } from "../../user-action/save-button/save-button.service";
import { SettingButtonComponent } from "../../user-action/setting-button/setting-button.component";
import { ShareButtonComponent } from "../../user-action/share-button/share-button.component";
import { ShareService } from "../../user-action/share-button/share.service";
import { OrganizationsComponent } from "./organizations.component";

describe("OrganizationsComponent", () => {
  let component: OrganizationsComponent;
  let fixture: ComponentFixture<OrganizationsComponent>;
  let mockBsModalService;
  let mockMapResultsService;
  let msalService;
  let mockRouter;
  let mockResource = {
    id: "19a02209-ca38-4b74-bd67-6ea941d41518",
    name: "Alaska Law Help",
    resourceCategory: "Civil Legal Services",
    description: "",
    url: "https://alaskalawhelp.org/",
    topicTags: [
      {
        id: "e1fdbbc6-d66a-4275-9cd2-2be84d303e12"
      }
    ],
    location: [
      {
        state: "Hawaii",
        city: "Kalawao",
        zipCode: "96761"
      }
    ],
    icon: "./assets/images/resources/resource.png",
    address: "2900 E Parks Hwy Wasilla, AK 99654",
    telephone: "907-279-2457",
    reviewer: []
  };
  let mockUrl = "https://www.microsoft.com/en-in/windows";
  let mockToastr;
  let mockSaveButtonService;

  beforeEach(async(() => {
    msalService = jasmine.createSpyObj(["getUser"]);
    mockBsModalService = jasmine.createSpyObj(["show"]);
    mockMapResultsService = jasmine.createSpyObj([
      "getLocationDetails",
      "displayMapResults",
      "getAddress"
    ]);
    mockToastr = jasmine.createSpyObj(["success"]);

    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [
        OrganizationsComponent,
        UserActionSidebarComponent,
        MapResultsComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        PrintButtonComponent,
        DownloadButtonComponent,
        SettingButtonComponent
      ],
      providers: [
        ArrayUtilityService,
        ShareService,
        HttpClientModule,
        HttpHandler,
        PersonalizedPlanService,
        NavigateDataService,
        NgxSpinnerService,
        {
          provide: MapResultsService,
          useValue: mockMapResultsService
        },
        {
          provide: BsModalService,
          useValue: mockBsModalService
        },
        {
          provide: Global,
          useValue: {
            role: "",
            shareRouteUrl: ""
          }
        },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                id: "123"
              }
            }
          }
        },
        {
          provide: ToastrService,
          useValue: mockToastr
        },
        {
          provide: MsalService,
          useValue: msalService
        },
        {
          provide: SaveButtonService,
          useValue: mockSaveButtonService
        },
        {
          provide: Router,
          useValue: mockRouter
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(OrganizationsComponent);
    component = fixture.componentInstance;
    component.resource = mockResource;
    component.urlOrigin = mockUrl;
    spyOn(component, "ngOnInit");
    fixture.detectChanges();
  }));

  it("should create", () => {
    expect(component).toBeDefined();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
