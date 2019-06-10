import { APP_BASE_HREF } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, RouterModule } from "@angular/router";
import { of } from "rxjs";

import { Global } from "../../global";
import { MapService } from "../../common/map/map.service";
import { PaginationService } from "../../common/pagination/pagination.service";
import { NavigateDataService } from "../../common/services/navigate-data.service";
import { StateCodeService } from "../../common/services/state-code.service";
import { GuidedAssistantSidebarComponent } from "../../common/sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component";
import { ServiceOrgSidebarComponent } from "../../common/sidebars/service-org-sidebar/service-org-sidebar.component";
import { ShowMoreService } from "../../common/sidebars/show-more/show-more.service";
import { TopicService } from "../shared/topic.service";
import { SubtopicsComponent } from "./subtopics.component";

describe("SubtopicsComponent", () => {
  let component: SubtopicsComponent;
  let fixture: ComponentFixture<SubtopicsComponent>;
  let mockactiveTopic = "123";
  let mockDocumentData = [
    {
      name: "Family1",
      parentTopicId: [
        {
          id: "111"
        },
        {
          id: "222"
        }
      ],
      keywords: "HOUSING",
      location: [
        {
          state: "Hawaii",
          county: "Kalawao County",
          city: "Kalawao",
          zipCode: "96742"
        }
      ],
      icon: "../../../assets/images/categories/topic.svg",
      guidedAssistantId: "9a6a6131-657d-467d-b09b-c570b7dad242"
    }
  ];
  let mockSubTopics = [
    {
      id: "333",
      name: "Custody / Visitation",
      overview: "Overview of the Custody/Visitation topic",
      quickLinks: [],
      parentTopicId: [
        {
          id: "123"
        }
      ],
      resourceType: "Topics",
      keywords: "Custody | Child Abuse | Child Custody",
      location: [
        {
          state: "Hawaii",
          city: "Kalawao",
          zipCode: "96761"
        }
      ],
      icon: "",
      guidedAssistantId: ""
    }
  ];
  let mockTopicService;
  let mockNavigateDataService;
  let mockShowMoreService;
  let mockTopic = "bd900039-2236-8c2c-8702-d31855c56b0f";
  let mockResourceType = "Organizations";

  beforeEach(async(() => {
    mockTopicService = jasmine.createSpyObj([
      "getDocumentData",
      "getSubtopics"
    ]);
    mockNavigateDataService = jasmine.createSpyObj(["getData", "setData"]);
    mockShowMoreService = jasmine.createSpyObj(["clickSeeMoreOrganizations"]);
    mockTopicService.getDocumentData.and.returnValue(of(mockDocumentData));
    mockTopicService.getSubtopics.and.returnValue(of(mockSubTopics));

    TestBed.configureTestingModule({
      declarations: [
        SubtopicsComponent,
        ServiceOrgSidebarComponent,
        GuidedAssistantSidebarComponent
      ],
      imports: [
        RouterModule.forRoot([
          {
            path: "topics/:topic",
            component: SubtopicsComponent
          }
        ]),
        HttpClientModule
      ],
      providers: [
        MapService,
        PaginationService,
        Global,
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
          provide: NavigateDataService,
          useValue: mockNavigateDataService
        },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                topic: "bd900039-2236-8c2c-8702-d31855c56b0f"
              }
            },
            url: of([
              {
                path: "subtopics",
                params: {}
              },
              {
                path: "bd900039-2236-8c2c-8702-d31855c56b0f",
                params: {}
              }
            ])
          }
        },
        { provide: ShowMoreService, useValue: mockShowMoreService }
      ],
      schemas: [
        NO_ERRORS_SCHEMA,
        CUSTOM_ELEMENTS_SCHEMA
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubtopicsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create subtopics component", () => {
    expect(component).toBeTruthy();
  });

  it("should call getSubtopics method in ngOnInit", () => {
    spyOn(component, "getSubtopics");
    component.ngOnInit();
    expect(component.getSubtopics).toHaveBeenCalled();
    expect(component.getSubtopics).toHaveBeenCalledTimes(1);
  });

  it("should return document data when getdoucmentdata method of topic service called", () => {
    let mockGuidedInput = {
      activeId: mockTopic,
      name: mockDocumentData[0].name,
      guidedAssistantId: undefined
    };
    component.getSubtopics();
    expect(component.activeTopic).toEqual(mockTopic);
    expect(component.topic).toEqual(mockDocumentData[0]);
    expect(component.icon).toEqual(mockDocumentData[0].icon);
    expect(component.guidedInput).toEqual(mockGuidedInput);
    expect(component.subtopics).toEqual(mockSubTopics);
    expect(mockNavigateDataService.setData).toHaveBeenCalledWith(mockSubTopics);
  });

  it("should return document data when getdoucmentdata method of topic service called", () => {
    let mockGuidedInput = {
      activeId: mockTopic,
      name: mockDocumentData[0].name,
      guidedAssistantId: undefined
      // guidedAssistantId: mockDocumentData[0].guidedAssistantId
    };
    component.getSubtopics();
    expect(component.activeTopic).toEqual(mockTopic);
    expect(component.topic).toEqual(mockDocumentData[0]);
    expect(component.icon).toEqual(mockDocumentData[0].icon);
    expect(component.guidedInput).toEqual(mockGuidedInput);
    expect(component.subtopics).toEqual(mockSubTopics);
    expect(mockNavigateDataService.setData).toHaveBeenCalledWith(mockSubTopics);
  });

  it("should call clickSeeMoreOrganizations method in clickSeeMoreOrganizationsFromSubtopic", () => {
    component.activeTopic = mockactiveTopic;
    component.topicIntent = "test";
    component.clickSeeMoreOrganizationsFromSubtopic(mockResourceType);
    expect(mockShowMoreService.clickSeeMoreOrganizations).toHaveBeenCalledWith(
      mockResourceType,
      mockactiveTopic,
      "test"
    );
  });
});
