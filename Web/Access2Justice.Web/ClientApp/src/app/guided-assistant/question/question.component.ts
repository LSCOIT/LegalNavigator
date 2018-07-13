import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
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
  //isClicked = false;
  //maximum: number = 100;
  //dynamic: number = 0;
  //response: Question;
  //totalques: number = 5;
  //ques: number = 0;
  //showRecommendations: boolean = false;

  getQuestion(): void {
    this.curatedExperienceId = "f3daa1e4-5f20-47ce-ab6d-f59829f936fe";
    let params = new HttpParams()
      .set("curatedExperienceId", this.curatedExperienceId);
    this.questionService.getQuestion(params)
      .subscribe(question => this.question = { ...question });
  }

  onSubmit(gaForm: NgForm): void {
    const params = {
      "curatedExperienceId": "f3daa1e4-5f20-47ce-ab6d-f59829f936fe",
      "answersDocId": "c3469dee-e628-4377-b4eb-81923766b02f",
      "buttonId": "2b92e07b-a555-48e8-ad7b-90b99ebc5c96",
      "multiSelectionFieldIds": [],
      "fields": [{
        "fieldId": "",
        "value": "",
      }]
    }
    this.questionService.getNextQuestion(params)
        .subscribe(response => this.question = { ...response });
  };

  //onSubmit(empForm: NgForm): void {
  //  let answer = new HttpParams();
  //  if (this.question.questionType.toLowerCase() === 'singleselection') {
  //    answer = answer.set('choiceId', empForm.value.listOptions);
  //  }
  //  else if (this.question.questionType.toLowerCase() === 'multipleselection') {
  //    for (var i = 0; i < this.answer.answers.length; i++) {
  //      answer = answer.set('choiceId', this.answer.answers[i]);
  //    }
  //  }
  //  else if (this.question.questionType.toLowerCase() === 'yesnotype') {
  //    answer = answer.set('choiceId', this.answer.answers[0]);
  //  }

  //  this.ques = this.ques + 1;
  //  if (this.ques == this.totalques) {
  //    this.showRecommendations = true;
  //  }

  //  this.dynamic = this.dynamic + 20;
  //  this.questionService.getNextQuestion(answer)
  //    .subscribe(response => this.question = { ...response });
  //}

  constructor(private questionService: QuestionService) { }

  ngOnInit() {
    this.getQuestion();
  }

}
