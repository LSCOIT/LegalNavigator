//import { Component, OnInit } from '@angular/core';

//@Component({
//  selector: 'app-topic',
//  templateUrl: './topic.component.html',
//  styleUrls: ['./topic.component.css']
//})
//export class TopicComponent implements OnInit {

//  constructor() { }

//  ngOnInit() {
//  }

//}

import { Component, OnInit, Input } from "@angular/core";
import { TopicService } from '../topics-resources/topic.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Topic } from '../topics-resources/topic';

@Component({
  selector: 'topic',
  templateUrl: './topic.component.html',
  providers: [TopicService]
})

export class TopicComponent implements OnInit {
  contents: any;
  constructor(private topicService: TopicService, private activeRoute: ActivatedRoute) {

  }
  @Input() cntresult: any;

  ngOnInit() {


    this.contents = this.topicService.getcontent(this.activeRoute.snapshot.params['title']).subscribe(data => {
      this.cntresult = data;
    });

  }

}
