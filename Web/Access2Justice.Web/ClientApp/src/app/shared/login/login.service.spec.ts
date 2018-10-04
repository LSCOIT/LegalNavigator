import { TestBed, inject } from '@angular/core/testing';
import { LoginService } from './login.service';
import { HttpClient, HttpHandler } from '@angular/common/http';

describe('LoginService', () => {

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LoginService, HttpClient, HttpHandler]
    });
  });

  it('should be created', inject([LoginService], (service: LoginService) => {
    expect(service).toBeTruthy();
  }));
});
