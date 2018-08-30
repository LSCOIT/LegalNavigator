import { TestBed, inject } from '@angular/core/testing';
import { ResourceService } from './resource.service';
import { HttpClientModule } from '@angular/common/http';

describe('ResourceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientModule ],
      providers: [ResourceService]
    });
  });

  it('should be created', inject([ResourceService], (service: ResourceService) => {
    expect(service).toBeTruthy();
  }));
});
