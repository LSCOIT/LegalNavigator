import { TestBed, inject } from '@angular/core/testing';
import { ResourceService } from './resource.service';
import { HttpClientModule } from '@angular/common/http';
import { Global } from '../../global';

describe('ResourceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [ResourceService, Global]
    });
  });

  it('should be created', inject([ResourceService], (service: ResourceService) => {
    expect(service).toBeTruthy();
  }));
});
