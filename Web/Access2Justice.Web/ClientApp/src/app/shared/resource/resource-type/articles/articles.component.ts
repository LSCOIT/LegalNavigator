import { Component, OnInit, Input } from '@angular/core';
import { ShowMoreService } from '../../../sidebars/show-more/show-more.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-articles',
  templateUrl: './articles.component.html',
  styleUrls: ['./articles.component.css']
})
export class ArticlesComponent implements OnInit {
  @Input() resource;
  activeResource: any;

  constructor(
    private showMoreService: ShowMoreService,
    private activeRoute: ActivatedRoute
  ) { }

  clickSeeMoreOrganizationsFromArticles(resourceType: string) {
    this.activeResource = this.activeRoute.snapshot.params['id'];
    this.showMoreService.clickSeeMoreOrganizations(resourceType, this.activeResource);
  }

  ngOnInit() {}
}
