import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-curated-experience",
  templateUrl: "./curated-experience.component.html",
  styleUrls: ["./curated-experience.component.css"]
})
export class CuratedExperienceComponent implements OnInit {
  questionsRemaining: number;
  totalQuestions: number;
  maxProgress: number;
  questionProgress: number;
  prevQuestionProgress: number = 0;
  newQuestionProgress: number;

  constructor() {}

  receiveQuestionsRemaining($event) {
    this.questionsRemaining = $event;
    this.calculateProgress();
  }

  receiveTotalQuestions($event) {
    this.totalQuestions = $event;
    this.maxProgress = Math.ceil(this.totalQuestions / 0.6);
  }

  calculateProgress() {
    if (this.questionsRemaining > 1) {
      this.newQuestionProgress = this.totalQuestions - this.questionsRemaining;
      if (this.newQuestionProgress > this.prevQuestionProgress) {
        this.questionProgress = this.newQuestionProgress;
      } else {
        this.questionProgress = this.prevQuestionProgress;
      }
    } else {
      this.questionProgress = this.maxProgress;
    }
    this.prevQuestionProgress = this.questionProgress;
  }

  ngOnInit() {}
}
