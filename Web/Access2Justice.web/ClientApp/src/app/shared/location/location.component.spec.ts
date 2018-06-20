import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LocationComponent } from './location.component';
import { LocationService } from './location.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { TemplateRef } from '@angular/core';
import { ModalModule } from 'ngx-bootstrap';

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

  it("should call update location of location service when update location of component is called", () => {
    spyOn(locationService, 'updateLocation').and.returnValue({ locality: '', address: '' });
    spyOn(modalService, 'hide');
    component.updateLocation();
    expect(locationService.updateLocation).toHaveBeenCalled();
  });

  it("should call hide of modal ref when update location of component is called", () => {
    spyOn(locationService, 'updateLocation').and.returnValue({ locality: '', address: '' });
    spyOn(modalService, 'hide');
    let modalRefInstance = new MockBsModalRef();
    component.modalRef = modalRefInstance;
    component.updateLocation();
    expect(modalRefInstance.isHideCalled).toBeTruthy();
  });

  it("should set the address,locality and showLocation variables of component when update location of component is called", () => {
    let address = 'Sample address';
    let location = 'Sample location';
    spyOn(locationService, 'updateLocation').and.returnValue({ locality: location, address: address });
    spyOn(modalService, 'hide');
    component.showLocation = true;
    component.updateLocation();
    expect(component.address).toEqual(address);
    expect(component.locality).toEqual(location);
    expect(component.showLocation).toBeFalsy();
  });
});

