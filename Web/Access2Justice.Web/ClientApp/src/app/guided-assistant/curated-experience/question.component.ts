import { Component, OnInit } from '@angular/core';
//import { NgForm } from '@angular/forms';
//import { HttpParams } from '@angular/common/http';
import { QuestionService } from './question.service';
import { Question } from './question';
//import { Answer } from './answers';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  //answer: Answer = {  answers : [] };
  //isClicked = false;
  //maximum: number = 100;
  //dynamic: number = 0;
  //response: Question;
  question: Question;
  //totalques: number = 5;
  //ques: number = 0;
  //showRecommendations: boolean = false;

  getQuestion(): void {
    this.questionService.getQuestion()
      .subscribe(question => this.question = { ...question });
  }

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

  //validateElement(element: HTMLInputElement): void {
  //  this.answer.answers = [];
  //  if (element.name == "checkOptions") {
  //    if (this.answer.answers.includes(element.id) == false && element.checked == true) {
  //      this.answer.answers.push(element.id)
  //    }
  //    else if (this.answer.answers.includes(element.id) == true && element.checked == false) {
  //      delete this.answer.answers[this.answer.answers.indexOf(element.id)];
  //    }
  //  }
  //  if (element.name == "btnOptions") {
  //    if (this.answer.answers.includes(element.id) == false && element.id != "") {
  //      this.answer.answers.push(element.id);
  //    }
  //    else if (this.answer.answers.includes(element.id) == true && element.id == "") {
  //      delete this.answer.answers[this.answer.answers.indexOf(element.id)];
  //    }
  //  }
  //}

  //back(): void { }

  //save(): void { }

  //exit(): void { }

  constructor(private questionService: QuestionService) { }

  ngOnInit() {
    this.getQuestion();
  }

}
