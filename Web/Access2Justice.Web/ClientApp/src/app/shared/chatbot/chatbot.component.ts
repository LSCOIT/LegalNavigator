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

  reply() {
    var query = this.replyMessage;
    this.searchService.getAnswers(query).subscribe(response => {
      this.cntresult = response;
      //this.cntresult = JSON.parse(response);

      if (this.cntresult != null && this.cntresult != '') {
        this.messages.push({
          "text": this.cntresult.Description,
          "types": this.cntresult.AbuseTypes,
          "class": "receive",
          "inputText": this.replyMessage
        })
        this.replyMessage = '';
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
