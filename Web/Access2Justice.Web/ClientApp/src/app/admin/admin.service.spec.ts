import { HttpClientModule } from "@angular/common/http";
import { inject, TestBed } from "@angular/core/testing";
import { AdminService } from "./admin.service";

describe("AdminService", () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [AdminService]
    });
  });

  it("should be created", inject([AdminService], (service: AdminService) => {
    expect(service).toBeTruthy();
  }));
});
