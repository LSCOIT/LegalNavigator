import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ShareButtonRouteComponent } from './share-button-route.component';
import { BsModalService } from 'ngx-bootstrap/modal';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ShareService } from '../share.service';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { Global } from '../../../../../global';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap';

describe('ShareButtonRouteComponent', () => {
  let component: ShareButtonRouteComponent;
  let fixture: ComponentFixture<ShareButtonRouteComponent>;
  let mockRouter;
  let mockGlobal;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [BrowserModule,
        FormsModule, ModalModule.forRoot(), HttpClientModule,
      ],
      declarations: [ ShareButtonRouteComponent],
      providers: [
        BsModalService,
        HttpClient,
        ShareService,
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute,
          useValue: {snapshot: {params: {'id': '123'}}}
        },
        { provide: Global, useValue: mockGlobal }
      ],
      schemas: [
        CUSTOM_ELEMENTS_SCHEMA
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShareButtonRouteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
