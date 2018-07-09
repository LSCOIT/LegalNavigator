import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-upper-nav',
  templateUrl: './upper-nav.component.html',
  styleUrls: ['./upper-nav.component.css']
})

export class UpperNavComponent implements OnInit {

  constructor(private http: HttpClient) {

  }
  externalLogin() {
    var form = document.createElement('form');
    form.setAttribute('method', 'GET');
    form.setAttribute('action', 'http://localhost:61726/login');
    document.body.appendChild(form);
    form.submit();
  }

  ngOnInit() {

  }


}
