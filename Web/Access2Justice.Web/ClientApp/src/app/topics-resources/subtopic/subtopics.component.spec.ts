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
import { ServiceOrgService } from '../../shared/sidebars/service-org.service';
import { LocationService } from '../../shared/location/location.service';
import { Observable } from 'rxjs/Observable';

describe('SubtopicsComponent', () => {
  let component: SubtopicsComponent;
  let fixture: ComponentFixture<SubtopicsComponent>;
  let topicService: TopicService;
  let navigateDataService: NavigateDataService;
  let mockactiveTopic = "e1fdbbc6-d66a-4275-9cd2-2be84d303e12";
  let mockDocumentData = [

    {
      "name": "Family1",
      "parentTopicId": [
        {
          "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba"
        },
        {
          "id": "f102bfae-362d-4659-aaef-956c391f79de"
        }
      ],
      "keywords": "HOUSING",
      "location": [
        {
          "state": "Hawaii",
          "county": "Kalawao County",
          "city": "Kalawao",
          "zipCode": "96742"
        },
        {
          "zipCode": "96742"
        }

      ],
      "jsonContent": "jsonContent",
      "icon": "./assets/images/topics/topic14.png",
      "createdBy": "API",
      "modifiedBy": "API"
    },
    {
      "name": "Family2",
      "parentTopicId": [
        {
          "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba"
        },
        {
          "id": "f102bfae-362d-4659-aaef-956c391f79de"
        }
      ],
      "keywords": "HOUSING",
      "location": [
        {
          "state": "Hawaii",
          "county": "Kalawao County",
          "city": "Kalawao",
          "zipCode": "96742"
        },
        {
          "zipCode": "96742"
        }

      ],
      "jsonContent": "jsonContent",
      "icon": "./assets/images/topics/topic14.png",
      "createdBy": "API",
      "modifiedBy": "API"
    }
  ]
  let mockSubTopics = [
    {
      "id": "bdc07e7a-1f06-4517-88d8-9345bb87c3cf",
      "name": "Custody / Visitation",
      "overview": "Overview of the Custody/Visitation topic",
      "quickLinks": [
        {
          "text": "Child Support",
          "url": "https://alaskalawhelp.org/resource/child-support?ref=sxZRw"
        },
        {
          "text": "Common Questions About Child Custody",
          "url": "https://alaskalawhelp.org/resource/common-questions-about-child-custody?ref=sxZRw"
        }
      ],
      "parentTopicId": [
        {
          "id": "e1fdbbc6-d66a-4275-9cd2-2be84d303e12"
        }
      ],
      "resourceType": "Topics",
      "keywords": "Custody | Child Abuse | Child Custody",
      "location": [
        {
          "state": "Hawaii",
          "city": "Kalawao",
          "zipCode": "96761"
        },
        {
          "state": "Hawaii",
          "city": "Honolulu",
          "zipCode": "96741"
        },
        {
          "state": "Alaska",
          "city": "Juneau",
          "zipCode": "96815"
        },
        {
          "state": "Alaska",
          "city": "Anchorage",
          "zipCode": "99507"
        }
      ],
      "icon": "https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/topics/housing.svg"
    },
    {
      "id": "9c9a59cc-34ac-4a6f-80c4-90ac041abba7",
      "name": "Adoption, Guardianship, and POA",
      "overview": "Overview of the Adoption, Guardianship, and POA topic",
      "quickLinks": [
        {
          "text": "Adoption and Guardianship: Making Permanent Plans for Children",
          "url": "http://dhss.alaska.gov/ocs/Pages/adoptions/default.aspx"
        },
        {
          "text": "Guardianship and Conservatorship -- What You Need to Know",
          "url": "http://courts.alaska.gov/shc/guardian-conservator/index.htm"
        }
      ],
      "parentTopicId": [
        {
          "id": "e1fdbbc6-d66a-4275-9cd2-2be84d303e12"
        }
      ],
      "resourceType": "Topics",
      "keywords": "Adoption | Guardianship | and POA",
      "location": [
        {
          "state": "Hawaii",
          "city": "Kalawao",
          "zipCode": "96761"
        },
        {
          "state": "Hawaii",
          "city": "Honolulu",
          "zipCode": "96741"
        },
        {
          "state": "Alaska",
          "city": "Juneau",
          "zipCode": "96815"
        },
        {
          "state": "Alaska",
          "city": "Anchorage",
          "zipCode": "99507"
        }
      ],
      "icon": "https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/topics/housing.svg"
    }
  ]
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
        ServiceOrgService,
        LocationService
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
