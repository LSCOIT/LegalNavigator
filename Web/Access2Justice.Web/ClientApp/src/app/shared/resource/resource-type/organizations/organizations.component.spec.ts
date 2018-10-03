import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { OrganizationsComponent } from './organizations.component';
import { UserActionSidebarComponent } from '../../../sidebars/user-action-sidebar/user-action-sidebar.component';
import { MapResultsComponent } from '../../../sidebars/map-results/map-results.component';
import { SaveButtonComponent } from '../../user-action/save-button/save-button.component';
import { ShareButtonComponent } from '../../user-action/share-button/share-button.component';
import { PrintButtonComponent } from '../../user-action/print-button/print-button.component';
import { DownloadButtonComponent } from '../../user-action/download-button/download-button.component';
import { SettingButtonComponent } from '../../user-action/setting-button/setting-button.component';
import { BsModalService } from 'ngx-bootstrap';
import { Global } from '../../../../global';
import { MapResultsService } from '../../../sidebars/map-results/map-results.service';
import { HttpClientModule, HttpHandler } from '@angular/common/http';
import { ArrayUtilityService } from '../../../array-utility.service';
import { ShareService } from '../../user-action/share-button/share.service';
import { ActivatedRoute } from '@angular/router';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { ToastrService } from 'ngx-toastr';

describe('OrganizationsComponent', () => {
  let component: OrganizationsComponent;
  let fixture: ComponentFixture<OrganizationsComponent>;
  let mockBsModalService;
  let mockMapResultsService;
  let mockResource = {
    resources: [
      {
        "id": "19a02209-ca38-4b74-bd67-6ea941d41518",
        "name": "Alaska Law Help",
        "type": "Civil Legal Services",
        "description": "",
        "url": "https://alaskalawhelp.org/",
        "topicTags": [
          {
            "id": "e1fdbbc6-d66a-4275-9cd2-2be84d303e12"
          }
        ],
        "location": [
          {
            "state": "Hawaii",
            "city": "Kalawao",
            "zipCode": "96761"
          }
        ],
        "icon": "./assets/images/resources/resource.png",
        "address": "2900 E Parks Hwy Wasilla, AK 99654",
        "telephone": "907-279-2457"
      }
    ]
  };
  let mockToastr;
  beforeEach(async(() => {
    mockBsModalService = jasmine.createSpyObj(['show']);
    mockMapResultsService = jasmine.createSpyObj(['getMap']);
    mockToastr = jasmine.createSpyObj(['success']);
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
        { provide: MapResultsService, useValue: mockMapResultsService },
        HttpClientModule,
        HttpHandler,
        { provide: BsModalService, useValue: mockBsModalService },
        { provide: Global, useValue: { role: '', shareRouteUrl: '' } },
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { params: { 'id': '123' } } }
        },
        PersonalizedPlanService,
        { provide: ToastrService, useValue: mockToastr }
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(OrganizationsComponent);
    component = fixture.componentInstance;
    component.resource = mockResource;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeDefined();
  });
});

