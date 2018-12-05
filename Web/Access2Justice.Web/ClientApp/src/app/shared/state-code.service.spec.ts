import { TestBed, inject } from '@angular/core/testing';
import { StateCodeService } from './state-code.service';
import { HttpClientModule } from '@angular/common/http';
import { Global } from '../global';
import { Observable } from 'rxjs/Observable';

describe('StateCodeService', () => {
  let service: StateCodeService;
  let global;
  const httpSpy = jasmine.createSpyObj('http', ['get']);
  let mockStateCodes = [
    {
      "code": "HI",
      "name": "Hawaii"
    },
    {
      "code": "AK",
      "name": "Alaska"
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [StateCodeService,
        { provide: Global, useValue: global }]
    });

    service = new StateCodeService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('should be created', inject([StateCodeService], (service: StateCodeService) => {
    expect(service).toBeTruthy();
  }));

  it('should get all state codes', inject([StateCodeService], (service: StateCodeService) => {
    const mockResponse = Observable.of(mockStateCodes);
    httpSpy.get.and.returnValue(mockResponse);
    service.getStateCodes().subscribe(stateCodes => {
      service.getStateCodes();
      expect(httpSpy.get).toHaveBeenCalled();
      expect(stateCodes).toEqual(mockStateCodes);
    });
  }));

});
