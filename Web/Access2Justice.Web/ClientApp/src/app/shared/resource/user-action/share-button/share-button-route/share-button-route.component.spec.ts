import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { ShareButtonRouteComponent } from './share-button-route.component';
import { BsModalService } from 'ngx-bootstrap/modal';
import { HttpClientModule } from '@angular/common/http';
import { ShareService } from '../share.service';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { Global } from '../../../../../global';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

describe('ShareButtonRouteComponent', () => {
  let component: ShareButtonRouteComponent;
  let fixture: ComponentFixture<ShareButtonRouteComponent>;
  let mockRouter;
  let mockGlobal;
  let mockShareService: ShareService;
  let mockToastr;
  let mockShareButtonRouteComponent;

  beforeEach(async(() => {
    mockToastr = jasmine.createSpyObj(['success']);
    mockShareService = jasmine.createSpyObj(['getResourceLink']);

    TestBed.configureTestingModule({
      imports: [
        BrowserModule,
        FormsModule,
        ModalModule.forRoot(),
        HttpClientModule,
      ],
      declarations: [ShareButtonRouteComponent],
      providers: [
        BsModalService,
        ShareService,
        HttpClientModule,
        { provide: ToastrService, useValue: mockToastr },
        { provide: Global, useValue: { role: '', shareRouteUrl: '' } },
        { provide: ActivatedRoute, useValue: { snapshot: { params: { 'id': '123' } } } },
        { provide: Router, useValue: mockRouter },
        { provide: ShareService, useValue: mockShareService },
        { provide: Router, useValue: mockRouter }
      ],
      schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA]
    })
      .compileComponents();
    fixture = TestBed.createComponent(ShareButtonRouteComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();

    let store = {};
    const mockSessionStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      },
      setItem: (key: string, value: string) => {
        store[key] = `${value}`;
      },
      removeItem: (key: string) => {
        delete store[key];
      },
      clear: () => {
        store = {};
      }
    };

    spyOn(sessionStorage, 'getItem')
      .and.callFake(mockSessionStorage.getItem);
    spyOn(sessionStorage, 'setItem')
      .and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, 'removeItem')
      .and.callFake(mockSessionStorage.removeItem);
    spyOn(sessionStorage, 'clear')
      .and.callFake(mockSessionStorage.clear);
  }));

  it('should create test', () => {
    expect(component).toBeTruthy();
  });

});


