import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { StateCodeService } from "../../common/services/state-code.service";
import { CuratedExperienceComponent } from "./curated-experience.component";

describe("CuratedExperienceComponent", () => {
  let component: CuratedExperienceComponent;
  let fixture: ComponentFixture<CuratedExperienceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CuratedExperienceComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [StateCodeService]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CuratedExperienceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should receive questions remaining", () => {
    spyOn(component, "calculateProgress");
    component.receiveQuestionsRemaining(9);
    expect(component.questionsRemaining).toBe(9);
    expect(component.calculateProgress).toHaveBeenCalled();
  });

  it("should receive total questions", () => {
    component.receiveTotalQuestions(9);
    expect(component.maxProgress).toBe(15);
  });

  it("should calculate progress", () => {
    component.questionsRemaining = 5;
    component.totalQuestions = 12;
    component.calculateProgress();
    expect(component.questionProgress).toBe(7);
  });
});
