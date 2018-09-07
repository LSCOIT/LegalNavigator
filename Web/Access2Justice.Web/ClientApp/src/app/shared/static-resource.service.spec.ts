import { HttpClientModule } from '@angular/common/http';
import { inject, TestBed } from '@angular/core/testing';
import { StaticResourceService } from './static-resource.service';

describe('StaticResource', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientModule ],
      providers: [StaticResourceService]
    });
  });

  it('should be created', inject([StaticResourceService], (service: StaticResourceService) => {
    expect(service).toBeTruthy();
  }));
});
