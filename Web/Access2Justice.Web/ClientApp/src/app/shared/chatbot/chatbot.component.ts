import { Component, Input, OnInit } from "@angular/core";
import { SearchService } from "../search/search.service";

@Component({
  selector: "chatbot",
  templateUrl: "./chatbot.component.html",
  styleUrls: ["./chatbot.component.css"]
})
export class ChatbotComponent implements OnInit {
  showChatbot = false;
  contents: any;
  @Input() cntresult: any;
  isLuisCallRequired: boolean = true;
  replyMessage = "";
  messages = [];

  constructor(private searchService: SearchService) {}

  select(input: string) {
    this.isLuisCallRequired = false;
    this.getAnswers(input);
  }

  reply() {
    var query = this.replyMessage;
    var maplocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    query = query + "|" + maplocation.location.state;
    this.getAnswers(query);
  }

  getAnswers(query: string) {
    this.searchService
      .getAnswers(query, this.isLuisCallRequired)
      .subscribe(response => {
        this.cntresult = response;
        //this.cntresult = JSON.parse(response);
        if (this.cntresult.topic) {
          this.cntresult.description =
            "please browse below link for more details ";
          this.cntresult.topic = window.location + this.cntresult.topic;
        }

        if (this.cntresult != null && this.cntresult != "") {
          this.messages.push({
            text: this.cntresult.description,
            types: this.cntresult.types,
            class: "receive",
            inputText: this.replyMessage,
            topicLink: this.cntresult.topic
          });
          this.replyMessage = "";
          this.isLuisCallRequired = true;
        }
      });
  }

  toggleChat() {
    this.showChatbot = !this.showChatbot;
  }

  ngOnInit() {}
}
