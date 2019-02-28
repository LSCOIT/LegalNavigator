import { TestBed } from "@angular/core/testing";
import { Observable } from "rxjs";

import { Global } from "../../global";
import { MapLocation } from "../../common/map/map";
import { TopicService } from "./topic.service";

describe("TopicService", () => {
  let service: TopicService;
  const httpSpy = jasmine.createSpyObj("http", ["get", "post"]);
  let global: Global = jasmine.createSpyObj(["shareRouteUrl"]);

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: Global,
          useValue: { role: "", shareRouteUrl: "/share", userId: "UserId" }
        }
      ]
    });
    service = new TopicService(httpSpy, global);
    httpSpy.get.calls.reset();
    let store = {};
    const mockSessionStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      },
      setItem: (key: string, value: string) => {
        store[key] = `${value}`;
      }
    };
    spyOn(sessionStorage, "setItem").and.callFake(mockSessionStorage.setItem);
  });

  it("should return list of topics", done => {
    let mockTopics = [
      { id: "1", title: "Housing", icon: "" },
      { id: "2", title: "Family", icon: "" },
      { id: "3", title: "Public Benefit", icon: "" }
    ];
    let mockMapLocation: MapLocation = {
      state: "test",
      city: "",
      county: "",
      zipCode: ""
    };
    let mockLocationDetails = { location: mockMapLocation };
    spyOn(sessionStorage, "getItem").and.returnValue(
      JSON.stringify(mockLocationDetails)
    );
    const mockResponse = Observable.of(mockTopics);
    httpSpy.post.and.returnValue(mockResponse);
    service.getTopics().subscribe(topics => {
      service.loadStateName();
      expect(httpSpy.post).toHaveBeenCalled();
      expect(topics).toEqual(mockTopics);
      expect(service.mapLocation).toEqual(mockMapLocation);
      done();
    });
  });

  it("should return list of subtopics", done => {
    let mockSubtopics = {
      id: "1",
      title: "Housing",
      subtopics: [
        {
          subtopicId: "2",
          title: "Subtopic1 Name",
          icon: "./ assets / images / topics / topic2.png"
        },
        {
          subtopicId: "3",
          title: "Subtopic2 Name",
          icon: "./ assets / images / topics / topic3.png"
        }
      ]
    };
    const mockResponse = Observable.of(mockSubtopics);

    httpSpy.post.and.returnValue(mockResponse);

    service.getSubtopics(1).subscribe(subtopics => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(subtopics).toEqual(mockSubtopics);
      done();
    });
  });

  it("should return selected topic details", done => {
    let mockDocument = {
      id: "1",
      title: "Family",
      icon: "",
      organizationalUnit: "Hawaii",
      overview: "",
      parentTopicId: null,
      resourceType: "Topics"
    };
    const mockResponse = Observable.of(mockDocument);
    httpSpy.post.and.returnValue(mockResponse);
    service.getDocumentData(1).subscribe(document => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(document).toEqual(mockDocument);
      done();
    });
  });

  it("should return details for the subtopic", done => {
    let mockSubtopicDetail = {
      title: "Eviction",
      overview:
        "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.",
      resources: []
    };
    const mockResponse = Observable.of(mockSubtopicDetail);
    httpSpy.post.and.returnValue(mockResponse);
    service.getSubtopicDetail(1).subscribe(subtopicDetail => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(subtopicDetail).toEqual(mockSubtopicDetail);
      done();
    });
  });
});
