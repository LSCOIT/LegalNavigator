import { ServiceOrgService } from './service-org.service';
import { Observable } from 'rxjs/Rx';
import { api } from '../../../api/api';
import { HttpHeaders } from '@angular/common/http';


describe('ServiceOrgService', () => {
  let service: ServiceOrgService;
  const mockHttpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  let mockOrganizations = [
    {
      "id": "19a02209-ca38-4b74-bd67-6ea941d41518",
      "name": "Legal Help Organization",
      "type": "Housing Law Services",
      "description": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.", "resourceType": "Organizations", "externalUrl": "", "url": "websiteurl.com", "topicTags": [{ "id": "afabf032-72a8-4b04-81cb-c101bb1a0730" }, { "id": "3aa3a1be-8291-42b1-85c2-252f756febbc" }], "location": [{ "zipCode": "96741" }, { "state": "Hawaii", "city": "Haiku-Pauwela" }, { "state": "Alaska" }], "icon": "./assets/images/resources/resource.png", "address": "Honolulu, Hawaii 96813, United States", "telephone": "XXX-XXX-XXXX", "overview": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.", "eligibilityInformation": "Copy describing eligibility qualification lorem ipsum dolor sit amet. ", "reviewedByCommunityMember": "Quote from community member consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo. Proin sodales pulvinar tempor. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.", "reviewerFullName": "", "reviewerTitle": "", "reviewerImage": "", "createdBy": "", "createdTimeStamp": "", "modifiedBy": "", "modifiedTimeStamp": "2018-04-01T04:18:00Z", "_rid": "mwoSAJdNlwIGAAAAAAAAAA==", "_self": "dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIGAAAAAAAAAA==/", "_etag": "\"41000b36-0000-0000-0000-5b1e56600000\"", "_attachments": "attachments/", "_ts": 1528714848
    }
  ];
  let mockResponse = Observable.of(mockOrganizations);
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);

  beforeEach(() => {
    httpSpy.get.calls.reset();
  });

  it('should create service organization service component', () => {
    expect(service).toBeTruthy();
  });

  it('should define service organization service component', () => {
    expect(service).toBeDefined();
  });

  it('should retrieve organizations from the API when the location value is state', (done) => {
    let mockMapLocation =
      {
        state: "sample state",
        city: undefined,
        county: undefined,
        zipCode: undefined
      };
    httpSpy.post.and.returnValue(mockResponse);
    service.getOrganizationDetails(mockMapLocation).subscribe(organizations => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(organizations).toEqual(mockOrganizations);
      done();
    });
  });

  it('should retrieve organizations from the API when the location value is city', (done) => {
    let mockMapLocation = {
      state: undefined,
      city: "sample city",
      county: undefined,
      zipCode: undefined
    };
    httpSpy.post.and.returnValue(mockResponse);
    service.getOrganizationDetails(mockMapLocation).subscribe(organizations => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(organizations).toEqual(mockOrganizations);
      done();
    });
  });

  it('should retrieve organizations from the API when the location value is county', (done) => {
    let mockMapLocation = {
      state: undefined,
      city: undefined,
      county: "sample county",
      zipCode: undefined
    };
    httpSpy.post.and.returnValue(mockResponse);
    service.getOrganizationDetails(mockMapLocation).subscribe(organizations => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(organizations).toEqual(mockOrganizations);
      done();
    });
  });

  it('should retrieve organizations from the API  when the location value is zipcode', (done) => {
    let mockMapLocation = {
      state: undefined,
      city: undefined,
      county: undefined,
      zipCode: '96701'
    };
    httpSpy.post.and.returnValue(mockResponse);
    service.getOrganizationDetails(mockMapLocation).subscribe(organizations => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(organizations).toEqual(mockOrganizations);
      done();
    });
  });

  it('should retrieve organizations from the API when the location value is country/city/state/zip', (done) => {
    let mockMapLocation = {
      state: 'Hawaii',
      city: 'Honolulu',
      county: 'Honolulu',
      zipCode: '96514'
    };
    httpSpy.post.and.returnValue(mockResponse);
    service.getOrganizationDetails(mockMapLocation).subscribe(organizations => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(organizations).toEqual(mockOrganizations);
      done();
    });
  });

  it('should return null from the API if no value is passed', (done) => {
    let mockMapLocation = {
      state: undefined,
      city: undefined,
      county: undefined,
      zipCode: undefined
    };
    httpSpy.post.and.returnValue(mockResponse);
    service.getOrganizationDetails(mockMapLocation).subscribe(organizations => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(organizations).toBeNull;
      done();
    });
  });
});
