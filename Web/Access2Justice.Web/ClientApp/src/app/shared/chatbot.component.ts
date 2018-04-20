import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'chatbot',
  templateUrl: './chatbot.component.html',
  styleUrls: ['./chatbot.component.css']
})

export class ChatbotComponent implements OnInit {
  public showStyle = false;
  public show = false;

  constructor() { }

  ngOnInit() {
  }

  getStyle() {
    if (this.showStyle) {
      return '#1d0dff';
    } else {
      return '';
    }
  }

  toggleChat() {
    this.show = !this.show;
  }
  
  toggleStyle() {
    this.showStyle = !this.showStyle;
  }
}
