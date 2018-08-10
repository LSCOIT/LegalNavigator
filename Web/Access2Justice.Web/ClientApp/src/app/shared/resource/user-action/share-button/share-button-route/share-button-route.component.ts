import { Component, OnInit } from '@angular/core';
import { ShareService } from '../../share-button/share.service';
import { ActivatedRoute, Router } from '@angular/router';
import { api } from '../../../../../../api/api';
import { HttpClient, HttpParams } from '@angular/common/http';
import { window } from 'rxjs/operator/window';

@Component({
  selector: 'app-share-button-route',
  templateUrl: './share-button-route.component.html',
  styleUrls: ['./share-button-route.component.css']
})
export class ShareButtonRouteComponent implements OnInit {

  constructor(private httpClient: HttpClient,
    private shareService: ShareService,
    private router: Router,
    private activeRoute: ActivatedRoute) { }

  ngOnInit() {
    this.getResourceLink();
  }

  getResourceLink(): void {
    let params = new HttpParams()
      .set("permaLink", this.activeRoute.snapshot.params['id']);
    this.shareService.getResourceLink(params)
      .subscribe(response => {
        if (response != undefined) {
          if (response.length > 0 && !response[0].startsWith("http" || "//")) {
            this.router.navigateByUrl(response[0]);
          }
          else {
            location.href = response[0];
          }
        }
        //ToDo - Once error branch code merged into develop branch will uncomment the below line
        //this.router.navigateByUrl("/404");
      });
  }
}
