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

describe('OrganizationsComponent', () => {
  let component: OrganizationsComponent;
  let fixture: ComponentFixture<OrganizationsComponent>;
  let mockBsModalService;
  let mockGlobal;
  
  beforeEach(async(() => {
    TestBed.configureTestingModule({
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
        { provide: BsModalService, useValue: mockBsModalService },
        { provide: Global, useValue: mockGlobal }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrganizationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
