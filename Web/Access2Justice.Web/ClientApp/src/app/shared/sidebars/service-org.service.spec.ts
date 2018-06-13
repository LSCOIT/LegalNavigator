import { ServiceOrgService } from './service-org.service';
import { Observable } from 'rxjs/Rx';

describe('ServiceOrgService', () => {
  let service: ServiceOrgService;
  const httpSpy = jasmine.createSpyObj('http', ['get']);
  let state: string = 'Hawaii';


  beforeEach(() => {
    service = new ServiceOrgService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('should retrieve organizations from the API via Get', (done) => {
    let mockOrganizations = [
      { "id": "19a02209-ca38-4b74-bd67-6ea941d41518", "name": "Legal Help Organization", "type": "Housing Law Services", "description": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.", "resourceType": "Organizations", "externalUrl": "", "url": "websiteurl.com", "topicTags": [{ "id": "afabf032-72a8-4b04-81cb-c101bb1a0730" }, { "id": "3aa3a1be-8291-42b1-85c2-252f756febbc" }], "location": [{ "zipCode": "96741" }, { "state": "Hawaii", "city": "Haiku-Pauwela" }, { "state": "Alaska" }], "icon": "./assets/images/resources/resource.png", "address": "Honolulu, Hawaii 96813, United States", "telephone": "XXX-XXX-XXXX", "overview": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.", "eligibilityInformation": "Copy describing eligibility qualification lorem ipsum dolor sit amet. ", "reviewedByCommunityMember": "Quote from community member consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo. Proin sodales pulvinar tempor. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.", "reviewerFullName": "", "reviewerTitle": "", "reviewerImage": "", "createdBy": "", "createdTimeStamp": "", "modifiedBy": "", "modifiedTimeStamp": "2018-04-01T04:18:00Z", "_rid": "mwoSAJdNlwIGAAAAAAAAAA==", "_self": "dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIGAAAAAAAAAA==/", "_etag": "\"41000b36-0000-0000-0000-5b1e56600000\"", "_attachments": "attachments/", "_ts": 1528714848 }
    ];

    const mockResponse = Observable.of(mockOrganizations);

    httpSpy.get.and.returnValue(mockResponse);

    service.getOrganizationDetail(state).subscribe(organizations => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${service.getOrganizationDetailsUrl}/` + state);
      expect(organizations).toEqual(mockOrganizations);
      done();
    });
  });

});
