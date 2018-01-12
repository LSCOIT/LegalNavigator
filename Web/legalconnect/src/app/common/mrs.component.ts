import { Component, OnInit, Input  } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { SearchService } from '../services/search.service';
import { DomSanitizer } from "@angular/platform-browser";

@Component({
    selector: 'mrs',
    templateUrl: './mrs.component.html',
})
export class MRSComponent implements OnInit {
    hasData: boolean = true;
    showMrs: boolean = false;
    showForms: boolean = false;
    showSubCat: boolean = false;
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
    collapsed: string = 'collapse';
    aClicked: boolean = false;
    sentence: string = "";
    currentUrl: string = '';
    SubCat: Array<any> = [];
    Resources:Array<any> = [];
    items: Array<any> = [];
    processes: Array<any> = [];
    private start = false;
    geoCountry: string = "";
    page: string = "chat";
    showWashingData: boolean = false;
    constructor(private router: Router, private aRoute: ActivatedRoute, private domSanitizer: DomSanitizer, private srchServ: SearchService)
    { }

    ngOnInit() {
        this.sentence = localStorage.getItem('sentence');
        var i: number = 0;
        var j: number;
        var curResources = JSON.parse(localStorage.getItem('curatesResources'));
        var hasData = localStorage.getItem('hasData');
        this.state = localStorage.getItem('geoState');
        console.log('state', this.state);
        if (this.state == 'Washington')
        {
            this.collapsed = 'collapse in';
            this.aClicked = true;
            if (hasData == 'true')
                this.showWashingData = true;
            else
                this.showWashingData = false;
        }
        else {
            this.collapsed = 'collapse';
            this.aClicked = false;
        
        if (localStorage.getItem('linkName') === "") {
            
             if (hasData == "true") {  //display ui from local storage
                
                this.items = [];
                this.SubCat = [];
                this.processes = [];
                this.Resources = [];
                var curResources = JSON.parse(localStorage.getItem('curatesResources'));
                this.hasData = true;
                console.log('cur resources', curResources.Resources);
                if (curResources.Resources != null && curResources.Resources.length > 0) {
                    this.showMrs = true;
                    for (var i = 0; i < curResources.Resources.length; i++) {
                        if (curResources.Resources[i].Action == 'Title')
                            this.Resources.push({
                                "Title": curResources.Resources[i].Title,
                                "ResourceJson": curResources.Resources[i].ResourceJson,
                            });
                    }
                    for (var i = 0; i < curResources.Resources.length; i++) {

                        var item = this.Resources.find(x => x.Title == curResources.Resources[i].Title);
                        if (item != null && item != undefined) {
                            if (curResources.Resources[i].Action != 'Title') {
                                if (curResources.Resources[i].ResourceJson != null && curResources.Resources[i].ResourceJson != "") {
                                    if (curResources.Resources[i].Action == 'Url')
                                        item.ResourceJson = item.ResourceJson + '<br /><br /><b> ' + curResources.Resources[i].Action + ': </b><br/><div class="topboxUrl" title="' + curResources.Resources[i].ResourceJson + '"><a href="' + curResources.Resources[i].ResourceJson + '"target="_blank">' + curResources.Resources[i].ResourceJson + '</a></div>'

                                    else
                                        item.ResourceJson = item.ResourceJson + '<br /><br /> <b> ' + curResources.Resources[i].Action + ': </b><br/>' + curResources.Resources[i].ResourceJson

                                }
                            }
                        }


                    }

                    i = 0;

                    while (i < this.Resources.length) {
                        var Resources: Array<any> = [];

                        for (var j = i; (j < i + 4 && j < this.Resources.length); j++) {

                            Resources.push({
                                "header": this.Resources[j].Title,
                                "body": this.Resources[j].ResourceJson,

                            });

                        }
                        if (i == 0)
                            this.items.push({ Resources, class: "active" });
                        else
                            this.items.push({ Resources, class: "" });
                        i = i + 4;

                    }


                }
                else
                    this.showMrs = false;
                
                if (curResources.RelatedIntents != null && curResources.RelatedIntents.length > 0) {
                    this.showSubCat = true;
                    for (var i = 0; i < curResources.RelatedIntents.length; i++) {
                        this.SubCat.push({
                            "id": curResources.RelatedIntents[i],
                            "name": curResources.RelatedIntents[i],
                        });
                    }
                }
                else
                    this.showSubCat = false;

                if (curResources.Processes != null && curResources.Processes.length > 0) {
                    if (localStorage.getItem('showProcess') == 'true')
                        this.showForms = true;
                    else
                        this.showForms = false;
                    this.collapsed = 'collapse in';
                    this.aClicked = true;
                    this.processes = [];
                    for (var i = 0; i < curResources.Processes.length; i++) {
                        if (curResources.Processes[i].Description != null) {
                            var item = this.processes.find(x => x.title == curResources.Processes[i].Title);
                            if (item != null || item != undefined)
                                //item.desc = item.desc + curResources.Processes[i].Description
                                item.desc = curResources.Processes[i].ActionJson == null ? item.desc + '<ul><li>' + curResources.Processes[i].Description + '</li></ul>' : item.desc + '<ul><li>' + curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson + '</li></ul>'
                            else {

                                this.processes.push({
                                    "id": curResources.Processes[i].stepNumber,
                                    "title": curResources.Processes[i].Title,
                                    //  "desc": curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson,
                                    "desc": curResources.Processes[i].ActionJson == null ? '<ul><li>' + curResources.Processes[i].Description + '</li></ul>' : '<ul><li>' + curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson + '</li></ul>',
                                    "class": ""
                                });

                            }

                        }
                    }

                    console.log('this processes', this.processes);
                }
                else
                    this.showForms = false;
            }
            else if (hasData == "false") {
                 this.showMrs = false;
                 this.showForms = false;
                 this.showSubCat = false;
                this.hasData = false;
                this.items = [];
                this.SubCat = [];
            
            }
            }

        }
        this.currentUrl = this.router.url; // this will give you current url

        var route:any;
        this.page = "chat";
        if (this.currentUrl.indexOf('general') > 0)
        {
            route = "general";
            this.page = "";
        }
        else if (this.currentUrl.indexOf('chat') > 0)
        {
            route = "chat";
            this.page = "chat";
        }
        else if (this.currentUrl.indexOf('guided') > 0)
        {
            route = "guided";
            this.page = "";
        }
        else if (this.currentUrl.indexOf('doc') > 0) {
            route = "doc";
            this.page = "";
        }
        else
            route = "general";
        
        if (localStorage.getItem('linkName') != null && localStorage.getItem('linkName') != undefined && localStorage.getItem('linkName')!="") {
            this.aciveBdg = localStorage.getItem('linkName');
            if (route != "chat")
            document.getElementById('mrlTopics').scrollIntoView();
        }

        
        if (this.currentUrl == '/' + route + '/forms') {
            document.getElementById('forms_steps').scrollIntoView();
            this.aClicked = true;
            if (this.collapsed == 'collapse in')
                this.collapsed = 'collapse';
            else
                this.collapsed = 'collapse in'
        }
        
    }

    displayText(linkName: string) {

        localStorage.setItem('hasData', 'true');
        localStorage.setItem('curatesResources', null);
        
        localStorage.setItem('linkName', linkName);
        localStorage.setItem('sentence', linkName);
        
        this.aciveBdg = linkName;
        var route:string;
        this.srchServ.getCuratedContents(linkName, localStorage.getItem('geoState'))
            .subscribe((res) => {
                
                var i = 0;
                if (res != null) {
                    console.log('by link click', res);
                    localStorage.setItem('curatesResources', JSON.stringify(res)); //store data
                    localStorage.setItem('hasData', "true");
                    this.hasData = true;
                    if (res.Resources!=null && res.Resources.length > 0 ) {
                        this.showMrs = true;
                        this.items = [];
                        for (var i = 0; i < res.Resources.length; i++) {
                            if (res.Resources[i].Action == 'Title')
                                this.Resources.push({
                                    "Title": res.Resources[i].Title,
                                    "ResourceJson": res.Resources[i].ResourceJson,
                                });
                        }
                        for (var i = 0; i < res.Resources.length; i++) {

                            var item = this.Resources.find(x => x.Title == res.Resources[i].Title);
                            if (item != null && item != undefined) {
                                if (res.Resources[i].Action != 'Title') {
                                    if (res.Resources[i].ResourceJson != null && res.Resources[i].ResourceJson != "") {
                                        if (res.Resources[i].Action == 'Url')
                                            item.ResourceJson = item.ResourceJson + '<br /> <b> ' + res.Resources[i].Action + ': </b><br/><div class="topboxUrl" title="' + res.Resources[i].ResourceJson + '"><a href="' + res.Resources[i].ResourceJson + '"target="_blank">' + res.Resources[i].ResourceJson + '</a></div>'

                                        else
                                            item.ResourceJson = item.ResourceJson + '<br /> <b> ' + res.Resources[i].Action + ': </b><br/>' + res.Resources[i].ResourceJson

                                    }
                                }
                            }


                        }

                        i = 0;

                        while (i < res.Resources.length) {
                            var Resources: Array<any> = [];
                            for (var j = i; (j < i + 4 && j < res.Resources.length); j++) {

                                Resources.push({

                                    "header": res.Resources[j].Title,
                                    "body": res.Resources[j].ResourceJson,


                                });


                            }
                            if (i == 0)
                                this.items.push({ Resources, class: "active" });
                            else
                                this.items.push({ Resources, class: "" });
                            i = i + 4;

                        }

                    }
                    else
                        this.showMrs = false;
                    if (res.RelatedIntents != null && res.RelatedIntents.length > 0) {
                        this.showSubCat = true;
                        this.SubCat = [];
                        for (var i = 0; i < res.RelatedIntents.length; i++) {
                            this.SubCat.push({
                                "id": res.RelatedIntents[i],
                                "name": res.RelatedIntents[i],
                            });
                        }
                    }
                    else
                        this.showSubCat = false;
                    if (res.Processes != null && res.Processes.length > 0) {
                        this.processes = [];
                        this.showForms = true;
                        for (var i = 0; i < res.Processes.length; i++) {
                            if (res.Processes[i].Description != null) {
                                var item = this.processes.find(x => x.title == res.Processes[i].Title);
                                if (item != null || item != undefined)
                                    //item.desc = item.desc + curResources.Processes[i].Description
                                    item.desc = res.Processes[i].ActionJson == null ? item.desc + '<ul><li>' + res.Processes[i].Description + '</li></ul>' : item.desc + '<ul><li>' + res.Processes[i].Description + '</br>' + res.Processes[i].ActionJson + '</li></ul>'
                                else {

                                    this.processes.push({
                                        "id": res.Processes[i].stepNumber,
                                        "title": res.Processes[i].Title,
                                        //  "desc": curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson,
                                        "desc": res.Processes[i].ActionJson == null ? '<ul><li>' + res.Processes[i].Description + '</li></ul>' : '<ul><li>' + res.Processes[i].Description + '</br>' + res.Processes[i].ActionJson + '</li></ul>',
                                        "class": ""
                                    });

                                }

                            }
                        }
                    }
                    else
                         
                    this.showForms = false;
                }
                else {
                    this.hasData = false;
                    localStorage.setItem('hasData', "false");
                    this.items = [];
                 
                    this.SubCat = []
                    this.processes = [];

                }
            },
            (err: any) => {

                console.log('curated error', err);


            }
            );
        if (this.currentUrl.indexOf('general') > 0) route = "general";
        else if (this.currentUrl.indexOf('chat') > 0) route = "chat";
        else if (this.currentUrl.indexOf('guided') > 0) route = "guided";
        else route = "general";
        console.log('route', '/' + route+'/'+linkName);
        document.getElementById('mrlTopics').scrollIntoView();
      //  this.router.navigate(['/' + route, linkName]);

        

    }
       
}