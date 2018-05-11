import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  onSubmit(searchForm: NgForm): void {
    console.log(searchForm.value.inputText);
  }

  constructor() { }

  ngOnInit() {
  }

}
