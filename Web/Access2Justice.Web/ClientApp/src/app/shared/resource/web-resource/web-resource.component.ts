import { Component, OnInit, Input } from '@angular/core';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-web-resource',
  templateUrl: './web-resource.component.html',
  styleUrls: ['./web-resource.component.css']
})
export class WebResourceComponent implements OnInit {

  @Input() webResult: any;

  constructor() {  }

  ngOnInit() {  }

}
