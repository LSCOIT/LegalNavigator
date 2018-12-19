import { Component, OnInit, Input } from '@angular/core';

import { SearchService } from '../search/search.service';

@Component({
  selector: 'chatbot',
  templateUrl: './chatbot.component.html',
  styleUrls: ['./chatbot.component.css']
})

export class ChatbotComponent implements OnInit {
  public showStyle = false;
  public show = false;
  contents: any;
  @Input() cntresult: any;
  isLuisCallRequired: boolean = true;
  replyMessage = "";
  messages = [];

  constructor(private searchService: SearchService) { }
  
  getStyle() {
    if (this.showStyle) {
      return '#1d0dff';
    } else {
      return '';
    }
  }

  select(input: string)
  {
    this.replyMessage = input;
    this.isLuisCallRequired = false;
    this.reply();
  }

  reply() {
    var query = this.replyMessage;
    this.searchService.getAnswers(query, this.isLuisCallRequired).subscribe(response => {
      this.cntresult = response;
      //this.cntresult = JSON.parse(response);

      if (this.cntresult != null && this.cntresult != '') {
        this.messages.push({
          "text": this.cntresult.description,
          "types": this.cntresult.types,
          "class": "receive",
          "inputText": this.replyMessage
        })
        this.replyMessage = '';
        this.isLuisCallRequired = true;
      }
    });

  }

  toggleChat() {
    this.show = !this.show;
  }
  
  toggleStyle() {
    this.showStyle = !this.showStyle;
  }

  ngOnInit() {
  }

}
