import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-curated-experience',
  templateUrl: './curated-experience.component.html',
  styleUrls: ['./curated-experience.component.css']
})
export class CuratedExperienceComponent implements OnInit {
  questionsRemaining: number;
  totalQuestions: number;
  maxProgress: number;
  questionProgress: number;

  constructor() { }

  receiveQuestionsRemaining($event) {
    this.questionsRemaining = $event;
    this.calculateProgress();
  }

  receiveTotalQuestions($event) {
    this.totalQuestions = $event;
    console.log(this.totalQuestions);
    this.maxProgress = Math.ceil(this.totalQuestions / 0.85);
  }

  calculateProgress() {
    this.questionProgress = this.totalQuestions -this.questionsRemaining;
  }

  ngOnInit() {
  }

}
