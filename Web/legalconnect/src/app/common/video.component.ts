import { Component, OnInit, Input  } from '@angular/core';
import { DomSanitizer } from "@angular/platform-browser";
import { SearchService } from '../services/search.service';

@Component({
    selector: 'videoTemplate',
    templateUrl: './video.component.html'
})
export class VideoComponent implements OnInit {
    videoUrl: any;
    videos: Array<any> = [];
    showVideo: boolean = false;
    constructor(private domSanitizer: DomSanitizer, private srchServ: SearchService)
    { }


    ngOnInit() {

        this.videos = [];
        var curResources = JSON.parse(localStorage.getItem('curatesResources'));

        if (curResources != null && curResources.RelatedVideos != null && curResources.RelatedVideos.length > 0) {
            this.showVideo = true;
            for (var i = 0; i < curResources.RelatedVideos.length; i++) {
                var str_url = "";
                console.log(curResources.RelatedVideos[i].Url);

                if (curResources.RelatedVideos[i].Url.indexOf("https") > 0) {
                    var ind1 = curResources.RelatedVideos[i].Url.indexOf("https");
                    var ind2 = curResources.RelatedVideos[i].Url.indexOf("frameborder");
                    str_url = curResources.RelatedVideos[i].Url.substr(ind1, ind2 - ind1 - 2);
                    console.log(str_url);
                }
                else {
                    console.log(str_url);
                    str_url = curResources.RelatedVideos[i].Url.replace("watch?v=", "embed/");
                }
                this.videos.push({
                    "id": curResources.RelatedVideos[i].VideoId,
                    "Title": curResources.RelatedVideos[i].Title,
                    "Url": this.domSanitizer.bypassSecurityTrustResourceUrl(str_url),
                    "ActionType": curResources.RelatedVideos[i].ActionType,
                });

            }
        }
        else
            this.showVideo = false;
    }
    videoOpen(event: any) {
        if (localStorage.getItem("play") != "true") {
            this.videoUrl = this.domSanitizer.bypassSecurityTrustResourceUrl("https://www.youtube.com/embed/c_vYlZK6rvw?autoplay=1");
            localStorage.setItem("play", "true");
        }

    }
}

