import { Component, OnInit, Input } from '@angular/core';
import { IResourceFilter } from '../search/search-results/search-results.model';
import { ActivatedRoute } from '@angular/router';
import { PaginationService } from '../search/pagination.service';

@Component({
  selector: 'app-guided-assistant-sidebar',
  templateUrl: './guided-assistant-sidebar.component.html',
  styleUrls: ['./guided-assistant-sidebar.component.css']
})
export class GuidedAssistantSidebarComponent implements OnInit {
  location: any;
  resourceFilter: IResourceFilter;
  activeTopic: any;
  @Input() guidedAssistantId: string;

  constructor(private activeRoute: ActivatedRoute, private paginationService: PaginationService) { }

  ngOnInit() {
  }

}
