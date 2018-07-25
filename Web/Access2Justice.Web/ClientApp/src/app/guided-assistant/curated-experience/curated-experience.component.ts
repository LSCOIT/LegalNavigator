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
    this.maxProgress = Math.ceil(this.totalQuestions / 0.75);
  }

  calculateProgress() {
    if (this.questionsRemaining >= 0) {
      this.questionProgress = this.totalQuestions - this.questionsRemaining;
    } else {
      this.questionProgress = this.maxProgress;
    }
  }

  ngOnInit() {
  }

}
