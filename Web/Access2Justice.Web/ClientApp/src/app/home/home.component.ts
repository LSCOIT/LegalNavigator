import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  topicLength = 9;

  slides = [
    {image: ''},
    {image: ''},
    {image: ''}
  ];

  constructor() { }

  ngOnInit() {
  }

}
