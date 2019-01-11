import { HttpClientModule } from "@angular/common/http";
import { inject, TestBed } from "@angular/core/testing";
import { MsalService } from "@azure/msal-angular";
import { Global } from "../../global";
import { ResourceService } from "./resource.service";

describe("ResourceService", () => {
  let msalService;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [
        ResourceService,
        Global,
        {
          provide: MsalService,
          useValue: msalService
        }
      ]
    });
  });

  it("should be created", inject(
    [ResourceService],
    (service: ResourceService) => {
      expect(service).toBeTruthy();
    }
  ));
});
