import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LocationComponent } from './location.component';
import { LocationService } from './location.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { TemplateRef } from '@angular/core';
import { ModalModule } from 'ngx-bootstrap';
import { MapLocation } from './location';

class MockBsModalRef {
  public isHideCalled = false;

  hide() {
    this.isHideCalled = true;
  }
}

describe('LocationComponent', () => {
  let component: LocationComponent;
  let fixture: ComponentFixture<LocationComponent>;
  let modalService: BsModalService;
  let template: TemplateRef<any>;
  let locationService: LocationService;
  let mockMapLocation: MapLocation = {
    state: 'Sample State',
    city : 'Sample City',
    county : 'Sample County',
    zipCode: '1009203',
    locality : 'Sample Location',
    address : 'Sample Address'
  };

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

  it("should assign session storage details to map location on ngInit", () => {
    sessionStorage.setItem("mockGlobalMapLocation", JSON.stringify(mockMapLocation));
    component.mapLocation = JSON.parse(sessionStorage.getItem("mockGlobalMapLocation"));
  });

  it("should call displayLocationDetails on ngInit", () => {
    spyOn(component, 'displayLocationDetails');
    component.ngOnInit();
    component.displayLocationDetails(mockMapLocation);
    expect(component.displayLocationDetails).toHaveBeenCalled();
  });

  it("should call modalService show when openModal is called", () => {
    spyOn(locationService, 'getMap');
    spyOn(modalService, 'show');
    component.openModal(template);
    expect(modalService.show).toHaveBeenCalled();
  });

  it("should call locationService getMap when openModal is called", () => {
    spyOn(locationService, 'getMap');
    component.openModal(template);
    expect(locationService.getMap).toHaveBeenCalled();
  });
  
  it("should call update location of location service when update location of component is called", () => {
    spyOn(locationService, 'updateLocation').and.returnValue(mockMapLocation);
    spyOn(modalService, 'hide');
    component.updateLocation();
    expect(locationService.updateLocation).toHaveBeenCalled();
  });

  it("should call hide of modal ref when update location of component is called", () => {
    spyOn(locationService, 'updateLocation').and.returnValue(mockMapLocation);
    spyOn(modalService, 'hide');
    let modalRefInstance = new MockBsModalRef();
    component.modalRef = modalRefInstance;
    component.updateLocation();
    expect(modalRefInstance.isHideCalled).toBeTruthy();
  });

  it("should call displayLocationDetails when updateLocation is called", () => {
    spyOn(locationService, 'updateLocation').and.returnValue(mockMapLocation);
    spyOn(component, 'displayLocationDetails');
    spyOn(modalService, 'hide');
    component.updateLocation();
    expect(component.displayLocationDetails).toHaveBeenCalled();
  });

  it("should set the address,locality and showLocation variables of component when displayLocationDetails is called", () => {
    spyOn(modalService, 'hide');
    component.showLocation = true;
    component.displayLocationDetails(mockMapLocation);
    expect(component.address).toEqual(mockMapLocation.address);
    expect(component.locality).toEqual(mockMapLocation.locality);
    expect(component.showLocation).toBeFalsy();
  });
  
});

