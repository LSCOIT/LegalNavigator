import { Component, OnInit, Input  } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { SearchService } from '../../services/search.service';


@Component({
    selector: 'chatResources',
    templateUrl: './chatresources.component.html'
})
export class ChatResComponent implements OnInit {
    rightboxes = [{
        "url": "",
        "text": "",
        "self": false
    }];
    location: string;
    constructor(private router: Router, private aRoute: ActivatedRoute, private srchServ: SearchService)
    { }


    ngOnInit() {
        var ref = JSON.parse(localStorage.getItem('references'));

        if (ref!=undefined && ref != null) {
            for (var r = 0; r < ref.length; r++)
                this.rightboxes.push({
                    "url": ref[r].url,
                    "text": ref[r].text,
                    "self": true
                })

        }
        else {

            this.srchServ.getChatReferences(localStorage.getItem('sentence'), localStorage.getItem('geoState')).subscribe((res) => {

                if (res != null && res.length > 0) {
                    this.rightboxes = [];
                    for (var i = 0; i < res.length; i++) {
                        this.rightboxes.push({
                            "url": res[i].Url,
                            "text": res[i].Name,
                            "self": true
                        })
                    }
                    localStorage.setItem('references', JSON.stringify(this.rightboxes));
                    
                }
            });
        }
        localStorage.setItem('resScrolled', 'false');
        setInterval(this.updateResScroll, 100);
    }

    updateResScroll() {

        if (localStorage.getItem('resScrolled') == 'false') {
            var element = document.getElementById('chat_resources');
            element.scrollTop = element.scrollHeight;
        }
    }

    openPop(Url: string) {

        this.location = Url;
    }

    closePop() {
        window.open(this.location, '_blank');
    }
}

