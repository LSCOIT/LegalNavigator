import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-did-you-know',
  templateUrl: './did-you-know.component.html',
  styleUrls: ['./did-you-know.component.css']
})
export class DidYouKnowComponent implements OnInit {
  @Input() learn: any;

  constructor() { }

  ngOnInit() {
  }

}
