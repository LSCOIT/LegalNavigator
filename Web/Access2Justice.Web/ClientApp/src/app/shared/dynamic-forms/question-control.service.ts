import { Injectable } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { IntakeFormQuestionBase } from "./intake-form/intake-form-question-base";

@Injectable()
export class QuestionControlService {
  constructor() {}

  toFormGroup(questions: IntakeFormQuestionBase<any>[]) {
    let group: any = {};

    questions.forEach(question => {
      group[question.name] = new FormControl(question.value || "");
    });
    return new FormGroup(group);
  }
}
