import { TestBed, inject } from '@angular/core/testing';
import { TokenInterceptor } from './token-interceptor.service';
import { MsalService } from '@azure/msal-angular';

describe('TokenInterceptor', () => {
  let msalService;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TokenInterceptor,
        { provide: MsalService, useValue: msalService }]
    });
  });

  it('should be created', inject([TokenInterceptor], (service: TokenInterceptor) => {
    expect(service).toBeTruthy();
  }));
});
