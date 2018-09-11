import { TestBed, inject } from '@angular/core/testing';

import { Token.InterceptorService } from './token.interceptor.service';

describe('Token.InterceptorService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [Token.InterceptorService]
    });
  });

  it('should be created', inject([Token.InterceptorService], (service: Token.InterceptorService) => {
    expect(service).toBeTruthy();
  }));
});
