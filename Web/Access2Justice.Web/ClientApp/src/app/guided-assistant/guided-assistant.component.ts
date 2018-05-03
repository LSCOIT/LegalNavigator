import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-guided-assistant',
  templateUrl: './guided-assistant.component.html',
  styleUrls: ['./guided-assistant.component.css']
})
export class GuidedAssistantComponent implements OnInit {
  topicLength = 6;

  max: number = 200;
  dynamic: number = 100;
 
  constructor() {

  }

  ngOnInit() {
  }

  
}
