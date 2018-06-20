import { async, ComponentFixture, TestBed, inject } from '@angular/core/testing';
import { LocationComponent } from './location.component';
import { LocationService } from './location.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { Injectable, TemplateRef, DebugElement } from '@angular/core';
declare var Microsoft: any;

const mockLocationService = {
  updateLocation: () => { }
};

describe('LocationComponent', () => {
  @Injectable()
  class mockModalService {
    show: (template) => {}
  }

  let component: LocationComponent;
  let fixture: ComponentFixture<LocationComponent>;
  let modalService: BsModalService;
  let template: TemplateRef<any>;
  let locationService: LocationService;

  beforeEach(
    async(() => {
      TestBed.configureTestingModule({
        imports: [],
        declarations: [LocationComponent],
        providers: [
          LocationComponent,
          { provide: BsModalService, useClass: mockModalService },
          { provide: LocationService, useValue: mockLocationService }
        ]
      });
      TestBed.compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(LocationComponent);
    component = fixture.componentInstance;
    modalService = fixture.debugElement.injector.get(BsModalService);
    locationService = fixture.debugElement.injector.get(LocationService);
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should define component", () => {
    expect(component).toBeDefined();
  });

});
