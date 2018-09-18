import { Component, Input, ElementRef, OnChanges } from '@angular/core';

@Component({
  selector: 'app-read-more',
  template: `
  <div [innerHTML]="currentText"></div>
  <a [class.hidden]="hideToggle" (click)="toggleView()">Read {{isCollapsed? 'more':'less'}}</a>`,
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
    if (this.text.length <= this.maxLength) {
      this.currentText = this.text;
      this.isCollapsed = false;
      this.hideToggle = true;
      return;
    }
    this.hideToggle = false;
    if (this.isCollapsed == true) {
      this.currentText = this.text.substring(0, this.maxLength) + "...";
    } else if(this.isCollapsed == false)  {
      this.currentText = this.text;
    }
  }

  ngOnChanges() {
    this.determineView();       
  }

}
