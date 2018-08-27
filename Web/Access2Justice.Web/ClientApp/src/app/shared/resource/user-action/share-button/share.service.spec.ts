import { TestBed, inject } from '@angular/core/testing';
import { ShareService } from './share.service';
import { HttpClientModule } from '@angular/common/http';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { api } from '../../../../../api/api';
describe('ShareService', () => {
  let httpTestingController: HttpTestingController;
  let service: ShareService;
  let mockExpected = "CED9B90";


  let mockInput = {
    ResourceId: "bdc07e7a-1f06-4517-88d8-9345bb87c3cf",
    UserId: "ACB833BB3F817C2FBE5A72CE37FE7AB9CD977E58580B9832B9…64748F3098C9FE3374106B6F4A2B157FE091CA6332C88A89B",
    Url: "/topics/bdc07e7a-1f06-4517-88d8-9345bb87c3cf"
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ShareService]
    });
    httpTestingController = TestBed.get(HttpTestingController);
    service = TestBed.get(ShareService);
  });

  it('should be created', inject([ShareService], (service: ShareService) => {
    expect(service).toBeTruthy();
  }));

  it('should call get with the correct URL', () => {
    service.getResourceLink('789').subscribe();
    const req = httpTestingController.expectOne(api.getResourceLink + '?789');
    req.flush(mockExpected);
    httpTestingController.verify();
  });

  it('should call post with the correct URL', () => {
    service.generateLink(mockInput).subscribe();
    const req = httpTestingController.expectOne(api.shareUrl);
    req.flush(mockExpected);
    httpTestingController.verify();
  });

  it('should call post with the correct URL', () => {
    service.checkLink(mockInput).subscribe();
    const req = httpTestingController.expectOne(api.checkPermaLink);
    req.flush(mockExpected);
    httpTestingController.verify();
  });

  it('should call post with the correct URL', () => {
    service.removeLink(mockInput).subscribe();   
    const req = httpTestingController.expectOne(api.unShareUrl);
    req.flush(mockExpected);
    httpTestingController.verify();
  });

});

