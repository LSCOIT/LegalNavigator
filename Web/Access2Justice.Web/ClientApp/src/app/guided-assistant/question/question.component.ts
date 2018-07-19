import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpParams } from '@angular/common/http';
//import { Observable } from 'rxjs/Observable';
import { QuestionService } from './question.service';
import { Question } from './question';
import { Answer } from './answers';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  answer: Answer;
  question: Question;
  curatedExperienceId: string;
  fieldParam: Array<Object>;s

  constructor(private questionService: QuestionService) { }

  getQuestion(): void {
    this.curatedExperienceId = "f3daa1e4-5f20-47ce-ab6d-f59829f936fe";
    let params = new HttpParams()
      .set("curatedExperienceId", this.curatedExperienceId);
    this.questionService.getQuestion(params)
      .subscribe(question => this.question = { ...question });
  }

  createFieldParam(formValue) {
      this.fieldParam = Object.keys(formValue.value).map(key => {
        return ({
            fieldId: key,
            value: formValue.value[key]
          }
        );
      });
    }

  onSubmit(gaForm: NgForm): void {
    if(gaForm.value) {
      this.createFieldParam(gaForm);
    }

    const params = {
      "curatedExperienceId": this.question.curatedExperienceId,
      "answersDocId": this.question.answersDocId,
      "buttonId": this.question.buttons[0].id,
      "multiSelectionFieldIds": [],
      "fields": this.fieldParam
    }
    this.questionService.getNextQuestion(params)
        .subscribe(response => this.question = { ...response });
  };

  ngOnInit() {
    this.getQuestion();
  }
}
