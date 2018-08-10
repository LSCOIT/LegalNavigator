import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { SubtopicsComponent } from './subtopics.component';
import { ServiceOrgSidebarComponent } from '../../shared/sidebars/service-org-sidebar.component';
import { GuidedAssistantSidebarComponent } from '../../shared/sidebars/guided-assistant-sidebar.component';
import { TopicService } from '../shared/topic.service';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { BreadcrumbService } from '../shared/breadcrumb.service';
import { ShowMoreService } from '../../shared/sidebars/show-more.service';
import { MapService } from '../../shared/map/map.service';
describe('SubtopicsComponent', () => {
  let component: SubtopicsComponent;
  let fixture: ComponentFixture<SubtopicsComponent>;
  let topicService: TopicService;
  let navigateDataService: NavigateDataService;
  let mockactiveTopic = "123";
  let mockDocumentData = [
    {
      "name": "Family1",
      "parentTopicId": [
        {
          "id": "111"
        },
        {
          "id": "222"
        }
      ],
      "keywords": "HOUSING",
      "location": [
        {
          "state": "Hawaii",
          "county": "Kalawao County",
          "city": "Kalawao",
          "zipCode": "96742"
        }

      ],
      "icon": "",
    }
  ];

  let mockSubTopics = [
    {
      "id": "333",
      "name": "Custody / Visitation",
      "overview": "Overview of the Custody/Visitation topic",
      "quickLinks": [],
      "parentTopicId": [
        {
          "id": "123"
        }
      ],
      "resourceType": "Topics",
      "keywords": "Custody | Child Abuse | Child Custody",
      "location": [
        {
          "state": "Hawaii",
          "city": "Kalawao",
          "zipCode": "96761"
        }
      ],
      "icon": ""
    }
  ];

  beforeEach(async(() => {   
    TestBed.configureTestingModule({
      declarations: [
        SubtopicsComponent,
        ServiceOrgSidebarComponent,
        GuidedAssistantSidebarComponent,
        BreadcrumbComponent
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'topics/:topic', component: SubtopicsComponent }
        ]),
        HttpClientModule
      ],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        TopicService,
        NavigateDataService,
        BreadcrumbService,
        ShowMoreService,
        MapService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubtopicsComponent);
    component = fixture.componentInstance;
    topicService = TestBed.get(TopicService);
    navigateDataService = TestBed.get(NavigateDataService);
    fixture.detectChanges();
  });

  it('should create subtopics component', () => {
    expect(component).toBeTruthy();
  });

  it('should define subtopics component', () => {
    expect(component).toBeTruthy();
  });

  it("should call getDocumentData of topicService service when getTopics of component is called", () => {
    spyOn(topicService, 'getDocumentData').and.returnValue(Observable.of(mockDocumentData));
    topicService.getDocumentData(mockactiveTopic);
    component.getSubtopics();
    expect(topicService.getDocumentData).toHaveBeenCalled();
  });

  it("should call getSubtopics of topicService service when getTopics of component is called", () => {
    spyOn(topicService, 'getSubtopics').and.returnValue(Observable.of(mockSubTopics));
    topicService.getSubtopics(mockactiveTopic);
    component.getSubtopics();
    expect(topicService.getSubtopics).toHaveBeenCalled();
  });

  it("should call setData of navigateData service when subtopics data available in getsubtopics of component is called", () => {  
    spyOn(navigateDataService, 'setData');
    navigateDataService.setData(mockSubTopics);
    component.getSubtopics();
    expect(navigateDataService.setData).toHaveBeenCalled();
  });
});
