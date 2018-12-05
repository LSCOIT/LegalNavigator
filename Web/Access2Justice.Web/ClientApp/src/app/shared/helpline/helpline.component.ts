import { Component, OnInit, Input } from '@angular/core';
import { HelpText } from '../../home/home';

@Component({
  selector: 'app-helpline',
  templateUrl: './helpline.component.html',
  styleUrls: ['./helpline.component.css']
})
export class HelplineComponent implements OnInit {
  @Input() helpText: HelpText;

  constructor() { }

  ngOnInit() {
  }

}
