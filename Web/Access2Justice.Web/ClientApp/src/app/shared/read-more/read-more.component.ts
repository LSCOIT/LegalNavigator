import { Component, Input, ElementRef, OnChanges } from '@angular/core';

@Component({
  selector: 'app-read-more',
  template: `
  <div class="p" [innerHTML]="currentText"></div>
  <a class="link p" [class.d-none]="hideToggle" (click)="toggleView()">Read {{isCollapsed? 'more':'less'}}</a>`,
  styles: [`
    a {
        cursor: pointer;
        color: #0D3FBB;
      }
  `]
})
export class ReadMoreComponent implements OnChanges {
  @Input() text: string;
  @Input() maxLength: number;
  currentText: string;
  hideToggle: boolean = true;
  isCollapsed: boolean = true;

  constructor(private elementRef: ElementRef) { }

  toggleView() {
    this.isCollapsed = !this.isCollapsed;
    this.determineView();
  }
  
  determineView() {
    if (this.text.length && this.text.length <= this.maxLength) {
      this.currentText = this.text;
      this.isCollapsed = false;
      this.hideToggle = true;
      return;
    }
    this.hideToggle = false;
    if (this.isCollapsed) {
      this.currentText = this.text.substring(0, this.maxLength) + "...";
    } else {
      this.currentText = this.text;
    }
  }

  ngOnChanges() {
    this.determineView();       
  }

}
