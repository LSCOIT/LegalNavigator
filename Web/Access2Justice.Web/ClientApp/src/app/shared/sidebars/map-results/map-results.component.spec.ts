import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MapResultsComponent } from './map-results.component';
import { MapResultsService } from './map-results.service';
import { HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { MapLocationResult } from './map-results';
import { of } from 'rxjs/observable/of';

describe('MapResultsComponent', () => {
  let component: MapResultsComponent;
  let fixture: ComponentFixture<MapResultsComponent>;
  let mockMapResultsService;

  let sampleAddress1: MapLocationResult = {
    Address: "Address Text"
  };

  let sampleAddress2: MapLocationResult = {
    Address: "Address Text 2"
  };

  let noItemsInAddressList: Array<MapLocationResult> = [];
  let oneItemInAddressList: Array<MapLocationResult> = [sampleAddress1];
  let twoItemsInAddressList: Array<MapLocationResult> = [sampleAddress1, sampleAddress2];

  let onlyOneAddress = {
    resources: [{
      resourceType: 'organizations',
      address: 'TestAddress1'
    }]
  };

  let onlyTwoAddress = {
    resources: [{
      resourceType: 'Action plans',
      address: 'TestAddress1'
    },
    {
      resourceType: 'organizations',
      address: 'TestAddress2'
    }]
  };

  let onlyOneLocationCoordinates = {
    resourceSets: [{
      resources: [{ point: { coordinates: [111] } }]
    }]
  };

  beforeEach(() => {
    mockMapResultsService = jasmine.createSpyObj(['getMap', 'getLocationDetails', 'mapResults']);
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [MapResultsComponent],
      providers: [
        { provide: MapResultsService, useValue: mockMapResultsService } 
      ]
    });
    TestBed.compileComponents();
    fixture = TestBed.createComponent(MapResultsComponent);
    component = fixture.componentInstance;
    });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should define component", () => {
    expect(component).toBeDefined();
  });

  it("should call getAddress on ngChanges", () => {
    spyOn(component, 'getAddress');
    component.ngOnChanges();
    component.getAddress();
    expect(component.getAddress).toHaveBeenCalled();
  });
  
  it("should push 1 item into addressList when getAddress is called", () => {
    mockMapResultsService.displayMapResults.and.returnValue(of());
    component.searchResource = onlyOneAddress;
    component.getAddress();
    expect(component.addressList.length).toEqual(1);
  });

  it("should push 2 items into addressList when getAddress is called", () => {
    mockMapResultsService.getMap.and.returnValue(of());
    component.searchResource = onlyTwoAddress;
    component.getAddress();
    expect(component.addressList.length).toEqual(2);
  });

  it("should call getMapResults of the component when getAddress is called", () => {
    mockMapResultsService.getMap.and.returnValue(null);
    spyOn(component, 'getMapResults').and.returnValue(of());
    component.searchResource = onlyTwoAddress;
    component.getAddress();
    expect(component.getMapResults).toHaveBeenCalled();
  });

  it("should call getMap of mapResultsService when getMapResults is called with no address in addresslist", () => {
    component.getMapResults(noItemsInAddressList);
    expect(mockMapResultsService.getMap).toHaveBeenCalled();
  });

  it("should call displayMapResults of when getMapResults is called with addresslist", () => {
    component.getMapResults(oneItemInAddressList);
    spyOn(component, 'displayMapResults');
    expect(component.displayMapResults).toHaveBeenCalled();
  });

  it("should call getLocationDetails of service when displayMapResults is called", () => {
    mockMapResultsService.getLocationDetails.and.returnValue(of());
    component.addressList = oneItemInAddressList;
    component.displayMapResults();
    expect(mockMapResultsService.getLocationDetails).toHaveBeenCalled();
  });

  it("should push 1 item into latitudeLongitude when displayMapResults is called", () => {
    mockMapResultsService.getLocationDetails.and.returnValue(of(onlyOneLocationCoordinates));
    component.addressList = oneItemInAddressList;
    component.displayMapResults();
    expect(component.latitudeLongitude.length).toEqual(1);
  });

  it("should push 2 item into latitudeLongitude(subscribe should be called twice) when displayMapResults is called", () => {
    mockMapResultsService.getLocationDetails.and.returnValue(of(onlyOneLocationCoordinates));
    component.addressList = twoItemsInAddressList;
    component.displayMapResults();
    expect(component.latitudeLongitude.length).toEqual(2);
  });

  it("should call mapResults of mapResultsService when displayMapResults is called", () => {
    mockMapResultsService.getLocationDetails.and.returnValue(of(onlyOneLocationCoordinates));
    component.addressList = oneItemInAddressList;
    component.displayMapResults();
    expect(mockMapResultsService.mapResults).toHaveBeenCalled();
  });

});
