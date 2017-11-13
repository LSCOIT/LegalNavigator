import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { SearchService } from '../services/search.service';
import { DomSanitizer } from "@angular/platform-browser";

@Component({
    selector: 'mrs',
    templateUrl: './mrs.component.html',
})
export class MRSComponent implements OnInit {

    text: string = "In this section you will find general legal information, self - help packets and videos on your rights as a<br />renter and what to do if you have a problem with your landlord.<a > http://www.washingtonlawhelp.org/issues/housing/tenants-rights</a>";
    aciveBdg: string = 'tenantrights';
    OneClass = "collapse";
    TwoClass = "collapse";
    ThreeClass = "collapse"
    FourClass = "collapse";
    routeUrl: string = "";
    state: string = "";
    city: string = "";
    mapsUrl: any;
    qnClicked: boolean = false;


    currentUrl: string = '';

    SubCat = [{
        "id": "tenantrights",
        "name": "tenantrights",
        "text": "In this section you will find general legal information, self-help packets and videos on your rights as a <br />renter and what to do if you have a problem with your landlord.<a> http://www.washingtonlawhelp.org/issues/housing/tenants-rights</a>"
    },
    {
        "id": "foreclosure",
        "name": "foreclosure",
        "text": "Browse the information below to find information about foreclosure prevention, foreclosure mediation and other foreclosure issues. <a>http://www.washingtonlawhelp.org/issues/housing/foreclosure-1</a>"
    },
    {
        "id": "emergencyshelter",
        "name": "emergencyshelter",
        "text": "Browse the resources below to find general information and resources on emergency shelter and assistance in Washington state.  <a>http://www.washingtonlawhelp.org/issues/housing/emergency-shelter-assistance</a>"
    },
    {
        "id": "mobileparktenants",
        "name": "mobileparktenants",
        "text": "Browse the resources below to find general information and resources on emergency shelter and assistance in Washington state.  <a>http://www.washingtonlawhelp.org/issues/housing/emergency-shelter-assistance</a>';"
    }
    ]

    Resources = [{
        "id": "1",
        "header": "Eviction in Washington State",
        "body": " Read about the rules landlords have to follow if they want you to move out.  Find resources that explain the eviction process, your rights as a tenant and how to respond if you receive an eviction notice. Start by watching our <a href='#'>short video</a>",
        "type": "button",
        "typeText": "More"
    },
    {
        "id": "2",
        "header": "Eviction in Washington State",
        "body": "For assistance with eviction go to  <a href='#'>Washington State 211</a> or 2-1-1 from a landline, 206-461-3200 or 800-621-4636 or 206-461-3610 for TTY/hearing impaired calls. The staff will tell you about agencies that can help you. They can also refer you to other resources such as financial education classes.",
        "type": "button",
        "typeText": "More"
    },
    {
        "id": "3",
        "header": "Chat",
        "body": "Chat text - 'Questions?  We can help, talk to our customer support service'",
        "type": "button",
        "typeText": "Chat Now"
    },
    {
        "id": "4",
        "header": "Eviction Help Line",
        "body": "<b>Eviction Help Line</b> (813) 991-0267. You Stop <b>Eviction</b>. Don't get locked out of your <b>home</b>, let us <b>help</b> you delay your eviction. ..."
    },

    ];
    Resources_1 = [{
        "id": "1",
        "header": "Eviction in Alaska State",
        "body": " Read about the rules landlords have to follow if they want you to move out.  Find resources that explain the eviction process, your rights as a tenant and how to respond if you receive an eviction notice. Start by watching our <a href='#'>short video</a>",
        "type": "button",
        "typeText": "More"
    },
    {
        "id": "2",
        "header": "Eviction in Alaska State",
        "body": "For assistance with eviction go to  <a href='#'>Washington State 211</a> or 2-1-1 from a landline, 206-461-3200 or 800-621-4636 or 206-461-3610 for TTY/hearing impaired calls. The staff will tell you about agencies that can help you. They can also refer you to other resources such as financial education classes.",
        "type": "button",
        "typeText": "More"
    },
    {
        "id": "3",
        "header": "Chat",
        "body": "Chat text - 'Questions?  We can help, talk to our customer support service'",
        "type": "button",
        "typeText": "Chat Now"
    },
    {
        "id": "4",
        "header": "Eviction Help Line Alaska",
        "body": "<b>Eviction Help Line</b> (813) 991-0267. You Stop <b>Eviction</b>. Don't get locked out of your <b>home</b>, let us <b>help</b> you delay your eviction. ..."
    },

    ]
    private start = false;
    constructor(private router: Router, private aRoute: ActivatedRoute, private domSanitizer: DomSanitizer, private srchServ: SearchService) { }

    ngOnInit() {

        this.currentUrl = this.router.url; // this will give you current url

        var route;
        if (this.currentUrl.indexOf('general') > 0)
            route = "general";
        else if (this.currentUrl.indexOf('chat') > 0)
            route = "chat";
        else if (this.currentUrl.indexOf('guided') > 0)
            route = "guided";
        else
            route = "general";


        var position = this.currentUrl.indexOf("/", this.currentUrl.indexOf("/") + 1);
        var str = this.currentUrl.substring(position + 1);

        var arr = this.SubCat.find(x => x.id === str);

        if (arr != null && arr != undefined) {
            this.aciveBdg = arr.id;
            this.text = arr.text;
            document.getElementById('mrlTopics').scrollIntoView();
        }

        
    }

    displayText(id: string) {
        this.aciveBdg = id;
        var route;

        if (this.currentUrl.indexOf('general') > 0) route = "general";
        else if (this.currentUrl.indexOf('chat') > 0) route = "chat";
        else if (this.currentUrl.indexOf('guided') > 0) route = "guided";
        else route = "general";
        document.getElementById('mrlTopics').scrollIntoView();
        this.router.navigate(['/' + route, id]);

        var arr = this.SubCat.find(x => x.id === id);
        console.log('arr', arr);
        if (arr != null && arr != undefined)
            this.text = arr.text;

    }
   
}