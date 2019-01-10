import { HttpClientModule } from "@angular/common/http";
import { inject, TestBed } from "@angular/core/testing";
import { IntakeQuestionService } from "./intake-question.service";

describe("IntakeQuestionService", () => {
  let service: IntakeQuestionService;
  const httpSpy = jasmine.createSpyObj("http", ["get", "post"]);

  beforeEach(() => {
    service = new IntakeQuestionService(httpSpy);
    httpSpy.get.calls.reset();
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [IntakeQuestionService]
    });
  });

  it("should be created", inject(
    [IntakeQuestionService],
    (service: IntakeQuestionService) => {
      expect(service).toBeTruthy();
    }
  ));
});
