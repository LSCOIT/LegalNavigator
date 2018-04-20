import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  slides = [
    {image: 'https://www.treehugger.com/natural-sciences/trees-talk-each-other-and-recognize-their-offspring.html'},
    {image: 'https://www.treehugger.com/natural-sciences/trees-talk-each-other-and-recognize-their-offspring.html'},
    {image: 'https://www.treehugger.com/natural-sciences/trees-talk-each-other-and-recognize-their-offspring.html'}
  ];

  constructor() { }

  ngOnInit() {
  }

}
