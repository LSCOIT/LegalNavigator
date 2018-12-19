import { Injectable } from '@angular/core';
import { IntakeFormQuestionBase } from './intake-form/intake-form-question-base';
import { FormControl, Validators, FormGroup } from '@angular/forms';

@Injectable()
export class QuestionControlService {

  constructor() { }

  toFormGroup(questions: IntakeFormQuestionBase<any>[]) {
    let group: any = {};

    questions.forEach(question => {
      group[question.name] = question.isRequired ? new FormControl(question.value || '', Validators.required)
        : new FormControl(question.value || '');
    });
    return new FormGroup(group);
  }
}
