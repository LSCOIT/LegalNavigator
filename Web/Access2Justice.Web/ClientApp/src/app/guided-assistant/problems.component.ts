import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-problems',
  templateUrl: './problems.component.html',
  styleUrls: ['./problems.component.css']
})
export class ProblemsComponent implements OnInit {

  max: number = 200;
  showWarning: boolean;
  dynamic: number;
  type: string;
 
  constructor() {
    this.random();
  }
 
  random(): void {
    let value = Math.floor(Math.random() * 100 + 1);
    let type: string;
 
    if (value < 25) {
      type = 'success';
    } else if (value < 50) {
      type = 'info';
    } else if (value < 75) {
      type = 'warning';
    } else {
      type = 'danger';
    }
 
    this.dynamic = value;
    this.type = type;
  }

  ngOnInit() {
  }

}
