import { Component, OnInit } from '@angular/core';
import { ShareService } from '../../share-button/share.service';
import { ActivatedRoute, Router } from '@angular/router';
import { api } from '../../../../../../api/api';
import { HttpClient, HttpParams } from '@angular/common/http';
import { window } from 'rxjs/operator/window';
import { ShareView } from '../../share-button/share.model';

@Component({
  selector: 'app-share-button-route',
  templateUrl: './share-button-route.component.html',
  styleUrls: ['./share-button-route.component.css']
})
export class ShareButtonRouteComponent implements OnInit {
  profileData: ShareView = { UserId: '', UserName:'' };
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
        if (response != undefined && response.resourceLink != undefined) {
          if (response.resourceLink.indexOf("http") == -1
            || response.resourceLink.indexOf("//") == -1) {
            if (response.userId && response.userName) {
              this.profileData.UserId = response.userId;
              this.profileData.UserName = response.userName;
              this.profileData.IsShared = true;
              sessionStorage.setItem("profileData", JSON.stringify(this.profileData));
            }
            return this.router.navigateByUrl(response.resourceLink, { skipLocationChange: true });
          }
          else {
            return location.href = response.resourceLink;
          }
        }
        return this.router.navigateByUrl("/404");
      });
  }
}
