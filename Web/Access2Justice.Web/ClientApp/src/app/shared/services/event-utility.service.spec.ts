import { inject, TestBed } from "@angular/core/testing";
import { EventUtilityService } from "./event-utility.service";

describe("EventUtilityService", () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EventUtilityService]
    });
  });

  it("should be created", inject(
    [EventUtilityService],
    (service: EventUtilityService) => {
      expect(service).toBeTruthy();
    }
  ));
});
