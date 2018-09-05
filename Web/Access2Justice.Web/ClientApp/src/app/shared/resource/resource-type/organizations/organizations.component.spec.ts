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

describe('OrganizationsComponent', () => {
  let component: OrganizationsComponent;
  let fixture: ComponentFixture<OrganizationsComponent>;
  let mockBsModalService;
  let mockMapResultsService;
  let mockGlobal;
  let mockResource: { resource: { address: "2900 E Parks Hwy Wasilla, AK 99654" } };

  beforeEach(async(() => {
    mockBsModalService = jasmine.createSpyObj(['show'])

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
        MapResultsService,
        HttpClientModule,
        HttpHandler,
        { provide: BsModalService, useValue: mockBsModalService },
        { provide: Global, useValue: { role: '', shareRouteUrl: '' } }
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(OrganizationsComponent);
    component = fixture.componentInstance;
    component.resource = mockResource;
    component.searchResource = {
      resources: {}, webResources: { webPages: { value: {} } }, topIntent: ''
    };
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeDefined();
  });
});

