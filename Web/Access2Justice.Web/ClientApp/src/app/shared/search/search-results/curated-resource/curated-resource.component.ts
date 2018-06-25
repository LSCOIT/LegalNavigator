import { Component, OnInit } from '@angular/core';
import { CuratedResource } from './curated-resource'
import { CuratedResourceService } from './curated-resource.service';

@Component({
  selector: 'app-curated-resource',
  templateUrl: './curated-resource.component.html',
  styleUrls: ['./curated-resource.component.css']
})
export class CuratedResourceComponent implements OnInit {
  curatedResource: CuratedResource[];

  constructor(private curatedResourceService: CuratedResourceService) { }

  getCuratedResource(): void {
    this.curatedResourceService.getCuratedResource()
      .subscribe(curatedResource => this.curatedResource = curatedResource);
  }

  ngOnInit() {
    this.getCuratedResource();
  }

}
