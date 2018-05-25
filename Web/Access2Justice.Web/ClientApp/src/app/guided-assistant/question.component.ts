import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpParams } from '@angular/common/http';
import { QuestionService } from './question.service';
import { Question } from './question';
import { Answer } from './answers';
//import { LocationMapComponent } from '../shared/location.component'

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  model = new Answer();
  isClicked = false;
  maximum: number = 100;
  dynamic: number;
  response: Question;
  question: Question;
  value: number = 0;
  totalques: number = 5;
  ques: number = 0;
  showRecommendations: boolean = false;

  getQuestion(): void {
    this.questionService.getQuestion()
      .subscribe(question => this.question = { ...question });
  }

  onSubmit(empForm: NgForm): void {
    let answer = new HttpParams();
    if (this.question.questionType.toLowerCase() === 'textbox') {
      answer = answer.set('answer', empForm.value.inputText);

    }
    else if (this.question.questionType.toLowerCase() === 'singleselection') {
      answer = answer.set('choiceId', empForm.value.listOptions);
    }
    else if (this.question.questionType.toLowerCase() === 'multipleselection') {
      for (var i = 0; i < this.model.answers.length; i++) {
        answer = answer.set('choiceId', this.model.answers[i]);
      }
    }
    else if (this.question.questionType.toLowerCase() === 'yesnotype') {
      answer = answer.set('choiceId', this.model.answers[0]);
    }

    this.ques = this.ques + 1;
    if (this.ques == this.totalques) {
      this.showRecommendations = true;
    }

    this.dynamic = this.value + 20;
    this.value = this.value + 20;
    if (answer != null) {
      this.questionService.getNextQuestion(answer)
        .subscribe(response => this.question = { ...response });
    }
  }

  validateCheckbox(element: HTMLInputElement): void {
    this.model.answers = [];
    if (element.name == "checkOptions") {
      if (this.model.answers.includes(element.id) == false && element.checked == true) {
        this.model.answers.push(element.id)
      }
      else if (this.model.answers.includes(element.id) == true && element.checked == false) {
        delete this.model.answers[this.model.answers.indexOf(element.id)];
      }
    }
    if (element.name == "btnOptions") {
      if (this.model.answers.includes(element.id) == false && element.id != "") {
        this.model.answers.push(element.id);
        this.isClicked = true;
      }
      else if (this.model.answers.includes(element.id) == true && element.id == "") {
        delete this.model.answers[this.model.answers.indexOf(element.id)];
      }
    }
    console.log(element);
  }

  back(): void { }

  save(): void { }

  exit(): void { }

  constructor(private questionService: QuestionService) { }

  ngOnInit() {
    this.getQuestion();
  }

}
