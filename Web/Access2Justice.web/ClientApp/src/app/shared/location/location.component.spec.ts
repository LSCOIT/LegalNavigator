import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LocationComponent } from './location.component';
import { LocationService } from './location.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { TemplateRef, DebugElement } from '@angular/core';
import { ModalModule } from 'ngx-bootstrap';
declare var Microsoft: any;

describe('LocationComponent', () => {
  let component: LocationComponent;
  let fixture: ComponentFixture<LocationComponent>;
  let modalService: BsModalService;
  let template: TemplateRef<any>;
  let locationService: LocationService;
  let element: DebugElement;

  beforeEach(
    () => {
      TestBed.configureTestingModule({
        imports: [ModalModule.forRoot()],
        declarations: [LocationComponent],
        providers: [
          BsModalService,
          LocationService
        ]
      });
      TestBed.compileComponents();
      fixture = TestBed.createComponent(LocationComponent);
      component = fixture.componentInstance;
      locationService = TestBed.get(LocationService);
      modalService = TestBed.get(BsModalService);
    });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should define component", () => {
    expect(component).toBeDefined();
  });

  it("should call locationService getMap when openModal is called", () => {
    spyOn(locationService, 'getMap');
    component.openModal(template);
    expect(locationService.getMap).toHaveBeenCalled();
  });

  it("should call modalService show when openModal is called", () => {
    spyOn(locationService, 'getMap');
    spyOn(modalService, 'show');
    component.openModal(template);
    expect(modalService.show).toHaveBeenCalled();
  });

  //it("should call update location service", () => {
  //  let mockMapLocation = {
  //    locality: "Sample Locality",
  //    address: "Sample Address"
  //  }
  //  component.mapLocation = mockMapLocation;
  //  spyOn(locationService, 'updateLocation');
  //  component.updateLocation();
  //  expect(locationService.updateLocation).toHaveBeenCalled();
  //});

});

