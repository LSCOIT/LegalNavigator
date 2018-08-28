import { MapResultsService } from './map-results.service';
import { Observable } from 'rxjs/Rx';

describe('MapResults Service', () => {
  let service: MapResultsService;
  let mockMapResult = {
    "authenticationResultCode": "ValidCredentials",
    "brandLogoUri": "http:\/\/dev.virtualearth.net\/Branding\/logo_powered_by.png",
    "copyright": "Copyright © 2018 Microsoft and its suppliers. All rights reserved. This API cannot be accessed and the content and any results may not be used, reproduced or transmitted in any manner without express written permission from Microsoft Corporation.",
    "resourceSets": [
      {
        "estimatedTotal": 1,
        "resources": [
          {
            "__type": "Location:http:\/\/schemas.microsoft.com\/search\/local\/ws\/rest\/v1",

            "bbox": [21.2903099060059, -157.869766235352, 21.3385944366455, -157.810363769531],
            "name": "96813, HI", "point": { "type": "Point", "coordinates": [21.3048686981201, -157.858184814453] },
            "address": { "adminDistrict": "HI", "adminDistrict2": "Honolulu County", "countryRegion": "United States", "formattedAddress": "96813, HI", "locality": "Honolulu", "postalCode": "96813" },
            "confidence": "High", "entityType": "Postcode1",
            "geocodePoints": [{
              "type": "Point",
              "coordinates": [21.3048686981201, -157.858184814453], "calculationMethod": "Rooftop",
              "usageTypes": ["Display"]
            }], "matchCodes": ["Ambiguous"]
          }]
      }], "statusCode": 200, "statusDescription": "OK", "traceId": "93c6f159e49040469858aea8b1213926|BN20300154|7.7.0.0|Ref A: 71844B79CA2B49C39955089F6B01A541 Ref B: BLUEDGE0521 Ref C: 2018-06-13T08:42:10Z"
  }
  const httpSpy = jasmine.createSpyObj('http', ['get']);
  
  beforeEach(() => {
    service = new MapResultsService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('should create map Results service', () => {
    expect(service).toBeTruthy();
  });

  it('should define map Results service', () => {
    expect(service).toBeDefined();
  });

  it('should return map results', (done) => {
    const mockResponse = Observable.of(mockMapResult);
    let mockAddress = "Test Address";
    let mockCredentials = "Test Credentials";
    httpSpy.get.and.returnValue(mockResponse);
    service.getLocationDetails(mockAddress, mockCredentials).subscribe(mapResults => {
      expect(httpSpy.get).toHaveBeenCalled();
      expect(mapResults).toEqual(mockMapResult);
      done();
    });
  });

});
