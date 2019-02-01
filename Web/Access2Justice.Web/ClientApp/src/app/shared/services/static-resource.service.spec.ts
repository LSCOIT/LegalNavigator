import { HttpClientModule } from "@angular/common/http";
import { inject, TestBed } from "@angular/core/testing";
import { Observable } from "rxjs";

import { MapLocation } from "../map/map";
import { StaticResourceService } from "./static-resource.service";

describe("StaticResource", () => {
  let service: StaticResourceService;
  const httpSpy = jasmine.createSpyObj("http", ["get", "post"]);

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [StaticResourceService]
    });
    service = new StaticResourceService(httpSpy);
    httpSpy.get.calls.reset();
    let store = {};
    const mockSessionStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      },
      setItem: (key: string, value: string) => {
        store[key] = `${value}`;
      }
    };
    spyOn(sessionStorage, "setItem").and.callFake(mockSessionStorage.setItem);
  });

  it("should be created", inject(
    [StaticResourceService],
    (service: StaticResourceService) => {
      expect(service).toBeTruthy();
    }
  ));

  it("should set location as AK", inject(
    [StaticResourceService],
    (service: StaticResourceService) => {
      let mockMapLocation = { state: "AK" };
      service.mapLocation = mockMapLocation;
      service.getLocation();
      expect(service.location).toEqual(mockMapLocation.state);
    }
  ));

  it("should set location as HI", inject(
    [StaticResourceService],
    (service: StaticResourceService) => {
      let mockMapLocation = { state: "HI" };
      service.mapLocation = mockMapLocation;
      service.getLocation();
      expect(service.location).toEqual(mockMapLocation.state);
    }
  ));

  it("should set location as otherthan alaska and hawaii", inject(
    [StaticResourceService],
    (service: StaticResourceService) => {
      let mockMapLocation = { state: "teststate" };
      service.getLocation();
      expect(service.location).toBe("Default");
    }
  ));

  it("should set global maplocation session storage values ", () => {
    let mockMapLocation: MapLocation = {
      state: "Alaska",
      city: "",
      county: "",
      zipCode: ""
    };
    let mockMapLocationDetails = {
      mockMapLocation,
      displayLocationDetails: { locality: "", address: "Alaska" }
    };
    spyOn(sessionStorage, "getItem").and.returnValue(
      JSON.stringify(mockMapLocationDetails)
    );
    service.loadStateName();
    expect(service.locationDetails).toEqual(mockMapLocationDetails);
    expect(service.state).toEqual(
      mockMapLocationDetails.displayLocationDetails.address
    );
  });

  it("should return list of static resources ", done => {
    let mockStaticContent = [
      {
        name: "HomePage",
        location: [{ state: "Alaska" }]
      },
      {
        name: "PrivacyPromisePage",
        location: [{ state: "Alaska" }]
      },
      {
        name: "HelpAndFAQPage",
        location: [{ state: "Alaska" }]
      },
      {
        name: "Navigation",
        location: [{ state: "Alaska" }]
      },
      {
        name: "AboutPage",
        location: [{ state: "Alaska" }]
      }
    ];
    let mockMapLocation: MapLocation = {
      state: "Alaska",
      city: "",
      county: "",
      zipCode: ""
    };
    const mockResponse = Observable.of(mockStaticContent);
    httpSpy.post.and.returnValue(mockResponse);
    service.getStaticContents(mockMapLocation).subscribe(staticresource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(staticresource).toEqual(mockStaticContent);
      done();
    });
  });
});
