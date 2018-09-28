import { TopicService } from './topic.service';
import { Observable } from 'rxjs/Rx';
import { api } from '../../../api/api';
import { Global, UserStatus } from '../../global';
import { Subject } from 'rxjs';

describe('TopicService', () => {
  let service: TopicService;
  let globalService: Global = { 
    role: UserStatus.Anonymous, 
    shareRouteUrl: "/share",
    showShare: false,
    showRemove: false,
    showMarkComplete: false,
    showDropDown: false,
    showSetting: false,
    profileRouteUrl: "/profile",
    data: "",
    notifyStaticData: new Subject<any>(),
    topicsData: "",
    externalLogin: () => {},
    getData: () => {},
    setData: () => { }
  };
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
  beforeEach(() => {
    service = new TopicService(httpSpy, globalService);
    httpSpy.get.calls.reset();
  });

  it('should return list of topics', (done) => {
    let mockTopics = [
      { id: '1', title: 'Housing', icon: '' },
      { id: '2', title: 'Family', icon: '' },
      { id: '3', title: 'Public Benefit', icon: '' }
    ];

    const mockResponse = Observable.of(mockTopics);
    httpSpy.post.and.returnValue(mockResponse);
    service.getTopics().subscribe(topics => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(topics).toEqual(mockTopics);
      done();
    });

  });

  it('should return list of subtopics', (done) => {
    let mockSubtopics = {
        id: "1",
        title: "Housing",
        subtopics: [
          {subtopicId: "2", title:"Subtopic1 Name", icon:"./ assets / images / topics / topic2.png"},
          {subtopicId: "3", title:"Subtopic2 Name", icon:"./ assets / images / topics / topic3.png"}
        ]
    }
    const mockResponse = Observable.of(mockSubtopics);

    httpSpy.post.and.returnValue(mockResponse);

    service.getSubtopics(1).subscribe(subtopics => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(subtopics).toEqual(mockSubtopics);
      done();
    });
  });

  it('should return details for the subtopic', (done) => {
    let mockSubtopicDetail = {
      title: "Eviction",
      overview:
        "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.",
      resources: []
    }

    const mockResponse = Observable.of(mockSubtopicDetail);

    httpSpy.post.and.returnValue(mockResponse);

    service.getSubtopicDetail(1).subscribe(subtopicDetail => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(subtopicDetail).toEqual(mockSubtopicDetail);
      done();
    });

  });
});
