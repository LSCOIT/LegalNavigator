import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpParams } from '@angular/common/http';
import { QuestionService } from './question.service';
import { Question } from './question';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  response: Question;

  question: Question;

  getQuestion(): void {
    this.questionService.getQuestion()
      .subscribe(question => this.question = {...question});
  }


  onSubmit(gaForm: NgForm): void {
    let answer = new HttpParams();
    if (this.question.questionType.toLowerCase() === 'textbox') {
      answer = answer.set('answer', gaForm.value.inputText);
    } else {
      answer = answer.set('choiceId', gaForm.value.listOptions);
    }
    

    this.questionService.getNextQuestion(answer)
      .subscribe(response => this.question = {...response});
  }

  constructor(private questionService: QuestionService) { }

  ngOnInit() {
    this.getQuestion();
  }

}
