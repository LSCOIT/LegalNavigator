import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ArticlesComponent } from './articles.component';
import { UserActionSidebarComponent } from '../../../sidebars/user-action-sidebar/user-action-sidebar.component';
import { GuidedAssistantSidebarComponent } from '../../../sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../../../sidebars/service-org-sidebar/service-org-sidebar.component';
import { NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ShowMoreService } from '../../../sidebars/show-more/show-more.service';
import { HttpClientModule } from '@angular/common/http';
import { NavigateDataService } from '../../../navigate-data.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Global } from '../../../../global';
import { PaginationService } from '../../../pagination/pagination.service';
import { MapService } from '../../../map/map.service';

describe('ArticlesComponent', () => {
  let component: ArticlesComponent;
  let fixture: ComponentFixture<ArticlesComponent>;
  let mockRouter;
  let mockShowMoreService;
  let mockArticleResource = [
    {
      "id": "4f327a5b-9a79-466e-98be-2542eb28bb28",
      "name": "Guardian ad Litem or Custody Investigator?",
      "type": "Legal Info",
      "description":
        "In any case where a Judge has to make a decision about what is best for a child, they can appoint a Guardian ad Litem (GAL) to represent the child's interests in the court, or a Custody Investigator (CI) to provide information to the judge about the family situation. It is the responsibility of the GAL or CI to report to the Court what they believe is in the best interests of the child, regardless of what the parents, grandparents, or children think.",
      "resourceType": "Articles",
      "url": "www.courtrecords.alaska.gov",
      "topicTags": [
        {
          "id": "e1fdbbc6-d66a-4275-9cd2-2be84d303e12"
        }
      ],
      "location": [
        {
          "state": "Alaska",
          "city": "Juneau",
          "zipCode": "96815"
        }
      ],
      "icon": "./assets/images/resources/resource.png",
      "overview":
        "The judge referred you to the Custody Investigator’s Office for a child custody/visitation investigation. You may have questions about the role of the investigator and what you can expect during the investigation. We know this is a very stressful time for you. We hope this introduction helps you understand the nature of our involvement.",
      "headline1": "How much does a custody evaluation cost?",
      "content1":
        "There is no fee for investigations done by the court Custody Investigator’s Office. If out of town travel is involved, you and/or the other parent may be responsible for those expenses."
    }
  ];
  beforeEach(async(() => {
    mockShowMoreService = jasmine.createSpyObj(['clickSeeMoreOrganizations']);
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [
        ArticlesComponent,
        UserActionSidebarComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent
      ],
      providers: [
        NavigateDataService,
        MapService,
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { params: { 'id': '123' } } }
        },
        { provide: Router, useValue: mockRouter },
        { provide: Global, useValue: { role: '', shareRouteUrl: '' } },
        { provide: ShowMoreService, uesValue: mockShowMoreService },
        PaginationService
      ],
      schemas: [
        NO_ERRORS_SCHEMA,
        CUSTOM_ELEMENTS_SCHEMA
      ]
    })
      .compileComponents();
    fixture = TestBed.createComponent(ArticlesComponent);
    component = fixture.componentInstance;
    component.resource = mockArticleResource;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
