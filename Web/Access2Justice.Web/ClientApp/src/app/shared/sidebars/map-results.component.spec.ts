import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MapResultsComponent } from './map-results.component';
import { MapResultsService } from './map-results.service';
import { DebugElement } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

describe('MapResultsComponent', () => {
  let component: MapResultsComponent;
  let fixture: ComponentFixture<MapResultsComponent>;
  let service: MapResultsService;
  let element: DebugElement;
  let onlyOneOrganization = {
    resources: [{
      resourceType: 'organizations',
      address: 'TestAddress1'
    }]
  };

  let onlyTwoOrganization = {
    resources: [{
      resourceType: 'organizations',
      address: 'TestAddress1'
    },
    {
      resourceType: 'organizations',
      address: 'TestAddress2'
    }]
  };

  let nonOrganization = {
    resources: [{
      resourceType: 'articles',
      address: 'TestAddress1'
    }]
  };

  let onlyOneLocationCoordinates = {
    resourceSets: [{
      resources: [{ point: { coordinates: [111] } }]
    }]
  };


  beforeEach(
    () => {
      TestBed.configureTestingModule({
        imports: [HttpClientModule],
        declarations: [MapResultsComponent],
        providers: [MapResultsService]
      });
      TestBed.compileComponents();
      fixture = TestBed.createComponent(MapResultsComponent);
      component = fixture.componentInstance;
      service = TestBed.get(MapResultsService);
    });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should define component", () => {
    expect(component).toBeDefined();
  });

  it("should call getMap of mapResultsService when getAddress is called", () => {
    spyOn(service, 'getMap');
    component.searchResource = 'TestSearchResource';
    component.getAddress();
    expect(service.getMap).toHaveBeenCalled();
  });

  it("should push into organizationsList when getAddress is called", () => {
    spyOn(service, 'getMap').and.returnValue(null);
    component.searchResource = onlyOneOrganization;
    component.getAddress();
    expect(component.organizationsList.length).toEqual(1);
  });

  it("should push 2 items into organizations into list when getAddress is called", () => {
    spyOn(service, 'getMap').and.returnValue(null);
    component.searchResource = onlyTwoOrganization;
    component.getAddress();
    expect(component.organizationsList.length).toEqual(2);
  });

  it("should not push into organizationsList when getAddress is called", () => {
    spyOn(service, 'getMap').and.returnValue(null);
    component.searchResource = nonOrganization;
    component.getAddress();
    expect(component.organizationsList.length).toEqual(0);
  });

  it("should call getMapResults of the component when getAddress is called", () => {
    spyOn(service, 'getMap').and.returnValue(null);
    spyOn(component, 'getMapResults').and.returnValue(Observable.of());

    component.searchResource = onlyTwoOrganization;
    component.getAddress();
    expect(component.getMapResults).toHaveBeenCalled();
  });

  it("should call getLocationDetails of service when getMapResults is called", () => {
    spyOn(service, 'getMap').and.returnValue(null);
    spyOn(service, 'getLocationDetails').and.returnValue(Observable.of());

    component.searchResource = onlyTwoOrganization;
    component.getAddress();
    component.getMapResults(onlyTwoOrganization);
    expect(service.getLocationDetails).toHaveBeenCalled();
  });

  it("should push item into latitudeLongitude when getAddress is called", () => {
    spyOn(service, 'getMap').and.returnValue(null);
    spyOn(service, 'getLocationDetails').and.returnValue(Observable.of(onlyOneLocationCoordinates));
    spyOn(service, 'mapResults');

    component.searchResource = onlyOneOrganization;
    component.getAddress();
    expect(component.latitudeLongitude.length).toEqual(1);
  });

  it("should push 2 item into latitudeLongitude(subscribe should be called twice) when getAddress is called", () => {
    spyOn(service, 'getMap').and.returnValue(null);
    spyOn(service, 'getLocationDetails').and.returnValue(Observable.of(onlyOneLocationCoordinates));
    spyOn(service, 'mapResults');

    component.searchResource = onlyTwoOrganization;
    component.getAddress();
    expect(component.latitudeLongitude.length).toEqual(2);
  });

});
