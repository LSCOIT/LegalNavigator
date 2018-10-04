import { TestBed, inject } from '@angular/core/testing';
import { LoginService } from './login.service';
import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { IUserProfile } from './user-profile.model';
import { Observable } from 'rxjs/Observable';

describe('LoginService', () => {

  let service: LoginService;
  const mockHttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
  let userProfile: IUserProfile = {
    oId: '5645466',
    name: 'testName',
    firstName: 'fname',
    lastName: 'lname',
    eMail: 'fnln@email.com',
    isActive: 'true',
    createdBy: 'testuser',
    createdTimeStamp: '20180910',
    modifiedBy: 'testuser',
    modifiedTimeStamp: '20180910'
  };
  let mockResponse = Observable.of(userProfile);
  let mockOid = '765657';
  let mockEmail = 'testmail@mail.com'
  beforeEach(() => {
    service = new LoginService(httpSpy);
    httpSpy.get.calls.reset();

    TestBed.configureTestingModule({
      providers: [LoginService, HttpClient, HttpHandler]
    });
  });

  it('should be created', inject([LoginService], (service: LoginService) => {
    expect(service).toBeTruthy();
  }));

  it('should be created', inject([LoginService], (service: LoginService) => {
    expect(service).toBeDefined();
  }));

  it('should be Inerted the user profile details', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    service.upsertUserProfile(userProfile).subscribe(updatedProfile => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(updatedProfile).toEqual(userProfile);
      done();
    });
  });

  it('should be updated the user details for given IOD', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    userProfile.oId = mockOid;
    service.upsertUserProfile(userProfile).subscribe(updatedProfile => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(updatedProfile).toEqual(userProfile);
      done();
    });
  });

  it('should be updated the user active status', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    userProfile.isActive = "true";
    service.upsertUserProfile(userProfile).subscribe(updatedProfile => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(updatedProfile).toEqual(userProfile);
      done();
    });
  });

  it('should be updated the user email address', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    userProfile.eMail = mockEmail;
    service.upsertUserProfile(userProfile).subscribe(updatedProfile => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(updatedProfile).toEqual(userProfile);
      done();
    });
  });

});
