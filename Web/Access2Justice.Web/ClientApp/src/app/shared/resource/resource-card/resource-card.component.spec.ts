import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ResourceCardComponent } from './resource-card.component';
import { SaveButtonComponent } from '../user-action/save-button/save-button.component';
import { ShareButtonComponent } from '../user-action/share-button/share-button.component';
import { ResourceCardDetailComponent } from '../resource-card-detail/resource-card-detail.component';
import { Global } from '../../../global';
import { PipeModule } from '../../pipe/pipe.module';
import { BsDropdownModule } from 'ngx-bootstrap';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { StateCodeService } from '../../../shared/services/state-code.service';
import { HttpClientModule } from '@angular/common/http';

class MockBsModalRef {
  isHideCalled = false;
  hide() {
    this.isHideCalled = true;
  }
}

describe('ResourceCardComponent', () => {
  let component: ResourceCardComponent;
  let fixture: ComponentFixture<ResourceCardComponent>;
  let mockGlobal;
  let modalService: BsModalService;
  let template: TemplateRef<any>;
  let mapService;
  let mockRouter;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        ResourceCardComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        ResourceCardDetailComponent
      ],
      imports: [
        PipeModule.forRoot(),
        BsDropdownModule.forRoot(),
        HttpClientModule
        ],
      providers: [
        BsModalService,
        StateCodeService,
        {
          provide: Global,
          useValue: {
            role: '',
            shareRouteUrl: ''
          }
        },
        {
          provide: ResourceCardComponent,
          useValue: {
            id: '',
            resources: [
              {
                itemId: '',
                resourceType: '',
                resourceDetails: {}
              }]
          }
        },
        {
          provide: Router,
          useValue: mockRouter
        }
      ],
      schemas: [
        NO_ERRORS_SCHEMA,
        CUSTOM_ELEMENTS_SCHEMA
      ]
    })
      .compileComponents();

    modalService = TestBed.get(BsModalService);
    fixture = TestBed.createComponent(ResourceCardComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
