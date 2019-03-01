import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { MapLocationResult } from './map-results';
import { MapResultsComponent } from './map-results.component';
import { MapResultsService } from './map-results.service';

describe('MapResultsComponent', () => {
  let component: MapResultsComponent;
  let fixture: ComponentFixture<MapResultsComponent>;
  let mockMapResultsService;
  let sampleAddress1: MapLocationResult = {
    Address: 'Address Text'
  };
  let sampleAddress2: MapLocationResult = {
    Address: 'Address Text 2'
  };
  let oneItemInAddressList: Array<MapLocationResult> = [sampleAddress1];
  let twoItemsInAddressList: Array<MapLocationResult> = [
    sampleAddress1,
    sampleAddress2
  ];
  let onlyTwoAddress = {
    resources: [
      {
        resourceType: 'Action plans',
        address: 'TestAddress1'
      },
      {
        resourceType: 'organizations',
        address: 'TestAddress2'
      }
    ]
  };
  let onlyOneLocationCoordinates = {
    resourceSets: [
      {
        resources: [
          {
            point: {
              coordinates: [111]
            }
          }
        ]
      }
    ]
  };

  beforeEach(() => {
    mockMapResultsService = jasmine.createSpyObj([
      'getMap',
      'getLocationDetails',
      'mapResults'
    ]);
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [MapResultsComponent],
      providers: [
        {
          provide: MapResultsService,
          useValue: mockMapResultsService
        }
      ]
    });
    TestBed.compileComponents();
    fixture = TestBed.createComponent(MapResultsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should define component', () => {
    expect(component).toBeDefined();
  });

  it('should call getAddress on ngChanges', () => {
    spyOn(component, 'getAddress');
    component.ngOnChanges();
    component.getAddress();
    expect(component.getAddress).toHaveBeenCalled();
  });

  it('should call getMapResults of the component when getAddress is called', () => {
    mockMapResultsService.getMap.and.returnValue(null);
    spyOn(component, 'getMapResults').and.returnValue(of());
    component.searchResource = onlyTwoAddress;
    component.getAddress();
    expect(component.getMapResults).toHaveBeenCalled();
  });

  it('should call displayMapResults of when getMapResults is called with addresses list', () => {
    mockMapResultsService.getLocationDetails.and.returnValue(of());
    component.getMapResults(['test']);
    spyOn(component, 'getMapResults').and.returnValue(of());
    component.displayMapResults();
    expect(component.displayMapResults).toBeTruthy();
  });

  it('should call getLocationDetails of service when displayMapResults is called', () => {
    component.validAddress = ['123 Main St, Seattle WA 98911'];
    mockMapResultsService.getLocationDetails.and.returnValue(of());
    component.displayMapResults();
    expect(mockMapResultsService.getLocationDetails).toHaveBeenCalled();
  });

  it('should push 1 item into latitudeLongitude when displayMapResults is called', () => {
    mockMapResultsService.getLocationDetails.and.returnValue(
      of(onlyOneLocationCoordinates)
    );
    component.validAddress = oneItemInAddressList;
    component.displayMapResults();
    expect(component.latitudeLongitude.length).toEqual(1);
  });

  it('should push 2 item into latitudeLongitude(subscribe should be called twice) when displayMapResults is called', () => {
    mockMapResultsService.getLocationDetails.and.returnValue(
      of(onlyOneLocationCoordinates)
    );
    component.validAddress = twoItemsInAddressList;
    component.displayMapResults();
    expect(component.latitudeLongitude.length).toEqual(2);
  });

  it('should call mapResults of mapResultsService when displayMapResults is called', () => {
    mockMapResultsService.getLocationDetails.and.returnValue(
      of(onlyOneLocationCoordinates)
    );
    component.validAddress = oneItemInAddressList;
    component.displayMapResults();
    expect(mockMapResultsService.mapResults).toHaveBeenCalled();
  });
});
