import { APP_BASE_HREF } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { RouterModule } from "@angular/router";
import { of } from "rxjs";

import { Global } from "../global";
import { MapService } from "../common/map/map.service";
import { PaginationService } from "../common/pagination/pagination.service";
import { NavigateDataService } from "../common/services/navigate-data.service";
import { StateCodeService } from "../common/services/state-code.service";
import { GuidedAssistantSidebarComponent } from "../common/sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component";
import { ServiceOrgSidebarComponent } from "../common/sidebars/service-org-sidebar/service-org-sidebar.component";
import { ShowMoreService } from "../common/sidebars/show-more/show-more.service";
import { TopicService } from "./shared/topic.service";
import { TopicsComponent } from "./topic/topics.component";
import { TopicsResourcesComponent } from "./topics-resources.component";

describe("TopicsResourcesComponent", () => {
  let component: TopicsResourcesComponent;
  let fixture: ComponentFixture<TopicsResourcesComponent>;
  let mockTopicService;
  let mockShowMoreService: ShowMoreService;
  let mockTopics = [
    {
      id: "e3bdf5d8-8755-46d9-b13b-e28546fcd27e",
      name: "Abuse & Harassment",
      parentTopicId: [],
      resourceType: "Topics",
      keywords: null,
      location: [
        {
          state: "Hawaii",
          city: "Kalawao",
          zipCode: "96761"
        }
      ],
      icon: ""
    }
  ];
  beforeEach(async(() => {
    mockTopicService = jasmine.createSpyObj(["getTopics"]);
    mockTopicService.getTopics.and.returnValue(of(mockTopics));
    mockShowMoreService = jasmine.createSpyObj(["clickSeeMoreOrganizations"]);
    TestBed.configureTestingModule({
      declarations: [
        TopicsResourcesComponent,
        TopicsComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent
      ],
      imports: [
        RouterModule.forRoot([
          {
            path: "topics/:topic",
            component: TopicsComponent
          }
        ]),
        HttpClientModule
      ],
      providers: [
        Global,
        MapService,
        NavigateDataService,
        PaginationService,
        StateCodeService,
        {
          provide: APP_BASE_HREF,
          useValue: "/"
        },
        {
          provide: TopicService,
          useValue: mockTopicService
        },
        {
          provide: ShowMoreService,
          useValue: mockShowMoreService
        }
      ],
      schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicsResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should call clickSeeMoreOrganizationsFromTopic", () => {
    let resourceType = "test";
    spyOn(component, "clickSeeMoreOrganizationsFromTopic");
    component.clickSeeMoreOrganizationsFromTopic(resourceType);
    expect(component.clickSeeMoreOrganizationsFromTopic).toHaveBeenCalled();
  });
});
