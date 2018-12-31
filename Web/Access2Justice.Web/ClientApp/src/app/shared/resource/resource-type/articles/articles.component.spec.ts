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
import { StateCodeService } from '../../../state-code.service';

describe('ArticlesComponent', () => {
  let component: ArticlesComponent;
  let fixture: ComponentFixture<ArticlesComponent>;
  let mockRouter;
  let mockShowMoreService;
  let mockGlobal;
  let mockArticleResource = 
    {
      "overview": "This article gives introductory information about registering a custody or child support order from another state with the Alaska courts.  An order from another state is sometimes called a \"foreign order.\"",
      "contents": [
        {
          "headline": "Why would I want to register my order?",
          "content": "If you want the Alaska courts or Alaska police to take action here in Alaska on the court order you have from another state, you generally must register the foreign order in Alaska. Some people register a foreign order simply to be able to enforce it; other people register a foreign order as the first step in a modification procedure."
        },
        {
          "headline": "When can I register an order?",
          "content": "Registration of a valid foreign court order for the purpose of enforcement is allowed at any time, and is a fairly straightforward process. Registration for the purpose of a modification is much more complicated because only a court with jurisdiction can modify an order. The laws controlling jurisdiction are very complicated. If you have any questions at all, you are urged to seek the advice of an attorney."
        },
        {
          "headline": "How long does it take for the registration to become effective?",
          "content": "The out-of-state order is registered when you file it with the Alaska court with the required attachments and pay the filing fee if there is one. Once the court has processed your packet, the clerk's office will serve the documents on the other party. The other party has 20 calendar days from the date they received the notice to request a hearing."
        },
        {
          "headline": "Do I have to register a domestic violence protective order from another state?",
          "content": "You don't have to file special forms at the court to register the protective order from another state. But for the police to enforce the out-of-state protective order, you can bring a certified copy to the court for filing. There is no filing fee. The clerk will send a copy of the filed order to Alaska law enforcement who will enter it into their system. The court or the police will not give the respondent a copy of the protective order or notice that you have filed the order with the Alaska court."
        }
      ],
      "id": "a882a5eb-0c39-44e9-9ecb-85fc486c690f",
      "name": "Introduction to Registering a Custody or Child Support Order from Another State",
      "resourceCategory": null,
      "description": "Introductory information about registering a custody or child support order from another state with the Alaska courts.",
      "resourceType": "Articles",
      "url": null,
      "topicTags": [
        {
          "id": "8cce15a2-3c0e-45a3-b77d-61f1915445fd"
        }
      ],
      "organizationalUnit": "AK",
      "location": [
        {
          "state": "AK",
          "county": "",
          "city": "",
          "zipCode": ""
        }
      ]
    };

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
        { provide: Global, useValue: { mockGlobal, role: '', shareRouteUrl: '', activeSubtopicParam: '123', topIntent: 'Divorce' } },
        { provide: ShowMoreService, useValue: mockShowMoreService },
        PaginationService,
        StateCodeService
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

  it('should call clickSeeMoreOrganizations service method', () => {
    component.clickSeeMoreOrganizationsFromArticles("test");
    expect(mockShowMoreService.clickSeeMoreOrganizations).toHaveBeenCalled();
  });
});
