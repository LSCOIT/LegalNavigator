import { TestBed, inject } from '@angular/core/testing';
import { ResourceService } from './resource.service';
import { HttpClientModule } from '@angular/common/http';
import { Global } from '../../global';
import { MsalService } from '@azure/msal-angular';

describe('ResourceService', () => {
  let msalService;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [ResourceService, Global,
        { provide: MsalService, useValue: msalService }],

    });
  });

  it('should be created', inject([ResourceService], (service: ResourceService) => {
    expect(service).toBeTruthy();
  }));
});
