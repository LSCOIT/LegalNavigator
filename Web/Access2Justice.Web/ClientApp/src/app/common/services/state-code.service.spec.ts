import { HttpClientModule } from "@angular/common/http";
import { inject, TestBed } from "@angular/core/testing";
import { Global } from "../../global";
import { StateCodeService } from "./state-code.service";

describe("StateCodeService", () => {
  let service: StateCodeService;
  let global;
  const httpSpy = jasmine.createSpyObj("http", ["get"]);

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [
        StateCodeService,
        {
          provide: Global,
          useValue: global
        }
      ]
    });

    service = new StateCodeService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it("should be created", inject(
    [StateCodeService],
    (service: StateCodeService) => {
      expect(service).toBeTruthy();
    }
  ));
});
