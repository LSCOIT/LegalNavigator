import { async, ComponentFixture, TestBed, inject } from '@angular/core/testing';
import { LocationComponent } from './location.component';;
import { } from '@types/bingmaps';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { Injectable, TemplateRef, DebugElement } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { By } from '@angular/platform-browser';
declare var Microsoft: any;


describe('LocationComponent', () => {
  @Injectable()
  class mockModalService {
    show: (template) => {}
  }

  let component: LocationComponent;
  let fixture: ComponentFixture<LocationComponent>;
  let modalService: BsModalService;
  let template: TemplateRef<any>;

  beforeEach(
    async(() => {
      TestBed.configureTestingModule({
        imports: [],
        declarations: [LocationComponent],
        providers: [
          LocationComponent,
          { provide: BsModalService, useClass: mockModalService },
        ]
      });
      TestBed.compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(LocationComponent);
    component = fixture.componentInstance;
    modalService = fixture.debugElement.injector.get(BsModalService);
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should define component", () => {
    expect(component).toBeDefined();
  });
  
});

