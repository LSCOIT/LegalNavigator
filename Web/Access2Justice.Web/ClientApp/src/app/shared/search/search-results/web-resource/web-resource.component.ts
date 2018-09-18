import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-web-resource',
  templateUrl: './web-resource.component.html',
  styleUrls: ['./web-resource.component.css']
})
export class WebResourceComponent implements OnInit {
  @Input() webResult: any;
  type: string = "WebResources";
  
  constructor() { }

  ngOnInit() {}
}
