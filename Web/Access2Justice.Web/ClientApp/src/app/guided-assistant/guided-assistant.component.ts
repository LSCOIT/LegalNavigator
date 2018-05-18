import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-guided-assistant',
  templateUrl: './guided-assistant.component.html',
  styleUrls: ['./guided-assistant.component.css']
})
export class GuidedAssistantComponent implements OnInit {

  max: number = 200;
  dynamic: number = 100;
 
  constructor() {

  }

  ngOnInit() {
  }

  
}
