import { HttpClientModule } from "@angular/common/http";
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { of } from "rxjs";

import { Global } from "../../../global";
import { MapService } from "../../map/map.service";
import { PaginationService } from "../../pagination/pagination.service";
import { NavigateDataService } from "../../services/navigate-data.service";
import { StateCodeService } from "../../services/state-code.service";
import { GuidedAssistantSidebarComponent } from "../../sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component";
import { MapResultsComponent } from "../../sidebars/map-results/map-results.component";
import { ServiceOrgSidebarComponent } from "../../sidebars/service-org-sidebar/service-org-sidebar.component";
import { ShowMoreService } from "../../sidebars/show-more/show-more.service";
import { UserActionSidebarComponent } from "../../sidebars/user-action-sidebar/user-action-sidebar.component";
import { ActionPlansComponent } from "../resource-type/action-plan/action-plans.component";
import { ArticlesComponent } from "../resource-type/articles/articles.component";
import { OrganizationsComponent } from "../resource-type/organizations/organizations.component";
import { VideosComponent } from "../resource-type/videos/videos.component";
import { ResourceService } from "../resource.service";
import { RemoveButtonComponent } from "../user-action/remove-button/remove-button.component";
import { ResourceCardDetailComponent } from "./resource-card-detail.component";

describe("ResourceCardDetailComponent", () => {
  let component: ResourceCardDetailComponent;
  let fixture: ComponentFixture<ResourceCardDetailComponent>;
  let mockResourceService;
  let mockResource, mockRouter, mockGlobal;

  beforeEach(async(() => {
    mockResource = [
      {
        id: "38bcfaac-ea4a-4951-bad9-6b173114cfaf",
        name: "Illegal Landlord Self-Help",
        type: "Landlord Tenant Law Basics",
        description: "",
        resourceType: "Articles",
        url: "",
        topicTags: [
          {
            id: "62a93f03-8234-46f1-9c35-b3146a96ca8b"
          }
        ],
        location: [
          {
            state: "Alaska",
            city: "Juneau",
            zipCode: "96815"
          }
        ],
        icon: "./assets/images/resources/resource.png",
        overview:
          "It is also illegal for your landlord to threaten to take legal action that is outside of the actions provided by law. This includes charging or threatening to charge you with criminal trespass in order to remove you from the property without a hearing. If this happens, you should contact an attorney immediately to discuss ways to protect yourself. ",
        headline1:
          "Your landlord cannot do the following things in an attempt to make you move:",
        content1:
          "Shut of utility service; Change the locks; Take your personal property; Take possession of the property by force, without a court hearing"
      }
    ];
    mockResourceService = jasmine.createSpyObj(["getResource"]);
    mockResourceService.getResource.and.returnValue(of(mockResource));
    TestBed.configureTestingModule({
      declarations: [
        ResourceCardDetailComponent,
        ArticlesComponent,
        OrganizationsComponent,
        VideosComponent,
        ActionPlansComponent,
        UserActionSidebarComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent,
        MapResultsComponent,
        RemoveButtonComponent
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
      providers: [
        ShowMoreService,
        NavigateDataService,
        PaginationService,
        MapService,
        StateCodeService,
        {
          provide: ResourceService,
          useValue: mockResourceService
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
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: Global,
          useValue: mockGlobal
        },
        {
          provide: Global,
          useValue: {
            role: "",
            shareRouteUrl: ""
          }
        }
      ],
      imports: [HttpClientModule]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceCardDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should call getResource ", () => {
    spyOn(component, "getResource");
    component.resourceId = component.resource.id;
    mockResourceService.getResource();
    component.getResource();
    expect(component.resourceId).toEqual(mockResource[0].id);
  });
});
