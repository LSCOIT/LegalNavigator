import { Component, OnInit, Input } from '@angular/core';
import { ServiceOrgService } from '../../../sidebars/service-org.service';
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
    private serviceOrgService: ServiceOrgService,
    private activeRoute: ActivatedRoute
  ) { }

  clickSeeMoreOrganizationsFromArticles(resourceType: string) {
    this.activeResource = this.activeRoute.snapshot.params['id'];
    this.serviceOrgService.clickSeeMoreOrganizations(resourceType, this.activeResource);
  }
  ngOnInit() {
  }

}
