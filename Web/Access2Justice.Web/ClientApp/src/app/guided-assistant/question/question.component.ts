import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { QuestionService } from './question.service';
import { Question } from './question';
import { Answer } from './answers';
import { ActivatedRoute } from '@angular/router';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { PersonalizedPlan } from '../personalized-plan/personalized-plan';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  answer: Answer;
  question: Question;
  curatedExperienceId: string;
  fieldParam: Array<Object>;
  buttonParam: string;
  @Output() sendQuestionsRemainingEvent = new EventEmitter<number>();
  @Output() sendTotalQuestionsEvent = new EventEmitter<number>();
  generatedPersonalizedPlan: PersonalizedPlan;

  constructor(
    private questionService: QuestionService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private navigateDataService: NavigateDataService
  ) { }

  sendQuestionsRemaining(questionsRemaining) {
    this.sendQuestionsRemainingEvent.emit(questionsRemaining);
  }

  sendTotalQuestions(totalQuestion) {
    this.sendTotalQuestionsEvent.emit(totalQuestion);
  }

  getQuestion(): void {
    this.curatedExperienceId = this.activeRoute.snapshot.params["id"];
    let params = new HttpParams()
      .set("curatedExperienceId", this.curatedExperienceId);
    this.questionService.getQuestion(params)
      .subscribe(question => {
        this.question = { ...question }
        this.sendTotalQuestions(this.question.questionsRemaining);
      });
  }

  createFieldParam(formValue) {
    this.fieldParam = Object.keys(formValue.value)
      .filter(key => formValue.value[key] !== "")
      .map(key => {
        if (key !== "buttonOptions") {
          if (key === "radioOptions" || key === "multiSelectOptions") {
            return ({
              fieldId: formValue.value[key]
            });
          } else {
            return ({
              fieldId: key,
              value: formValue.value[key]
            });
          }
        }
      });
  }

  onSubmit(gaForm: NgForm) {
    if (gaForm.value) {
      this.createFieldParam(gaForm);
      if (this.question.buttons.length > 1) {
        this.buttonParam = gaForm.value.buttonOptions;
      } else {
        this.buttonParam = this.question.buttons[0].id;
      }
    }

    window.scrollTo(0, 0);
    //need to work on multiselect params
    const params = {
      "curatedExperienceId": this.question.curatedExperienceId,
      "answersDocId": this.question.answersDocId,
      "buttonId": this.buttonParam,
      "fields": this.fieldParam
    }

    this.questionService.getNextQuestion(params)
      .subscribe(response => {
        this.question = { ...response }
        this.sendQuestionsRemaining(this.question.questionsRemaining);
      });
  };

  getActionPlan(): void {
    let params = new HttpParams()
      //.set("curatedExperienceId", this.curatedExperienceId)
      //.set("answersDocId", this.question.answersDocId);

    //temporarily hardcoding params for testing
      .set("curatedExperienceId", '93bcabe2-8afc-4044-b4da-59f7b94510c4')
      .set("answersDocId", '93a461fb-e9e3-4e6f-b9fd-49a185aa0da0');
    
    this.questionService.getpersonalizedPlan(params)
      .subscribe(response => {
        if (response != undefined && response.id != undefined) {
          this.generatedPersonalizedPlan = response;
          this.navigateDataService.setData(this.generatedPersonalizedPlan);
          this.router.navigate(['/plan', response.id]);
        }
      });
  }

  ngOnInit() {
    this.getQuestion();
  }
}
