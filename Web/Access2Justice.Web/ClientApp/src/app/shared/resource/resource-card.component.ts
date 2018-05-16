import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Topic } from '../../topics-resources/shared/topic';

@Component({
  selector: 'app-resource-card',
  templateUrl: './resource-card.component.html',
  styleUrls: ['./resource-card.component.css']
})
export class ResourceCardComponent implements OnInit, OnChanges {
  @Input() data: Topic;
  @Input() type: string;

  subtopicDetails: Topic;
  constructor() { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['data']) {
      this.subtopicDetails = this.data;
    }
  }

}
