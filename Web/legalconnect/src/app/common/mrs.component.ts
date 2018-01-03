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
    SubCat:Array<any> = [];
    Resources:Array<any> = [];
    items: Array<any> = [];
    processes: Array<any> = [];
    private start = false;
    geoCountry: string = "";
    constructor(private router: Router, private aRoute: ActivatedRoute, private domSanitizer: DomSanitizer, private srchServ: SearchService)
    { }

    ngOnInit() {
        this.sentence = localStorage.getItem('sentence');
        var i: number = 0;
        var j: number;
        var curResources = JSON.parse(localStorage.getItem('curatesResources'));
        var hasData = localStorage.getItem('hasData');
        if (localStorage.getItem('linkName') === "") {
            
             if (hasData == "true") {  //display ui from local storage
                this.showMrs = true;
                this.items = [];
                this.SubCat = [];
                this.processes = [];
                var curResources = JSON.parse(localStorage.getItem('curatesResources'));
                this.hasData = true;
                if (curResources.Resources.length > 0) {
                    
                    while (i < curResources.Resources.length) {
                        var Resources: Array<any> = [];
                        for (var j = i; (j < i + 4 && j < curResources.Resources.length); j++) {
                            if (curResources.Resources[j].Action != 'Title') {
                                if (curResources.Resources[j].Action == 'Url') {
                                    Resources.push({

                                        "header": curResources.Resources[j].Title,
                                        "body": '<b>' + curResources.Resources[j].Action + '</b><br /><div class="topboxUrl" title="' + curResources.Resources[j].ResourceJson + '"><a href="' + curResources.Resources[j].ResourceJson + '"target="_blank">' + curResources.Resources[j].ResourceJson + '</a></div>',

                                    });
                                }
                                else {
                                    Resources.push({

                                        "header": curResources.Resources[j].Title,
                                        "body": '<b>' + curResources.Resources[j].Action + '</b><br />' + curResources.Resources[j].ResourceJson,

                                    });
                                }
                            }
                            else {
                                Resources.push({

                                    "header": curResources.Resources[j].Title,
                                    "body": curResources.Resources[j].ResourceJson,

                                });
                            }

                        }
                        if (i == 0)
                            this.items.push({ Resources, class: "active" });
                        else
                            this.items.push({ Resources, class: "" });
                        i = i + 4;

                    }

                     
                }
                if (curResources.RelatedIntents.length > 0) {
                    
                    for (var i = 0; i < curResources.RelatedIntents.length; i++) {
                        this.SubCat.push({
                            "id": curResources.RelatedIntents[i],
                            "name": curResources.RelatedIntents[i],
                        });
                    }
                }
                if (curResources.Processes.length > 0) {
                    this.processes = [];
                    for (var i = 0; i < curResources.Processes.length; i++) {
                     
                        var item = this.processes.find(x => x.title == curResources.Processes[i].Title);
                        if (item != null || item != undefined)
                            item.desc = item.desc + curResources.Processes[i].Description
                        else
                            this.processes.push({
                                "id": curResources.Processes[i].stepNumber,
                                "title": curResources.Processes[i].Title,
                                "desc": curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson,
                                "class": ""
                            });
                       
                        //if (i == curResources.Processes.length-1)
                        //this.processes.push({
                        //    "id": curResources.Processes[i].stepNumber,
                        //    "title": curResources.Processes[i].Title,
                        //    "desc": curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson,
                        //    "class":"padding0"
                        //    });
                        //else
                        //    this.processes.push({
                        //        "id": curResources.Processes[i].stepNumber,
                        //        "title": curResources.Processes[i].Title,
                        //        "desc": curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson,
                        //        "class": ""
                        //    });
                    }

                    
                    console.log('processes',this.processes);
                }
            }
            else if (hasData == "false") {
                this.showMrs = false;
                this.hasData = false;
                this.items = [];
                this.SubCat = [];
            
            }
        }
        this.currentUrl = this.router.url; // this will give you current url

        var route:any;
        
        if (this.currentUrl.indexOf('general') > 0)
            route = "general";
        else if (this.currentUrl.indexOf('chat') > 0)
            route = "chat";
        else if (this.currentUrl.indexOf('guided') > 0)
            route = "guided";
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

                    localStorage.setItem('curatesResources', JSON.stringify(res)); //store data
                    localStorage.setItem('hasData', "true");
                    this.hasData = true;
                    if (res.Resources.length > 0) {
                        
                        this.items = [];
                        while (i < res.Resources.length) {
                            var Resources: Array<any> = [];
                            for (var j = i; (j < i + 4 && j < res.Resources.length); j++) {
                                if (res.Resources[j].Action != 'Title') {
                                    if (res.Resources[j].Action == 'Url') {
                                        Resources.push({

                                            "header": res.Resources[j].Title,
                                            "body": res.Resources[j].ResourceJson == "" ? res.Resources[j].LawCategory.Description : res.Resources[j].LawCategory.Description + '<br /><br /><b>' + res.Resources[j].Action + '</b><br /><div class="topboxUrl" title="' + res.Resources[j].ResourceJson + '"><a href="' + res.Resources[j].ResourceJson + '"target="_blank">' + res.Resources[j].ResourceJson + '</a></div>',

                                        });
                                    }
                                    else {
                                        Resources.push({

                                            "header": res.Resources[j].Title,
                                            "body": res.Resources[j].ResourceJson == "" ? res.Resources[j].LawCategory.Description : res.Resources[j].LawCategory.Description + '<br /><br /><b>' + res.Resources[j].Action + '</b><br />' + res.Resources[j].ResourceJson,

                                        });
                                    }
                                }
                                else {

                                    Resources.push({

                                        "header": res.Resources[j].Title,
                                        "body": res.Resources[j].ResourceJson,


                                    });
                                }

                            }
                            if (i == 0)
                                this.items.push({ Resources, class: "active" });
                            else
                                this.items.push({ Resources, class: "" });
                            i = i + 4;

                        }
                        
                    }
                    if (res.RelatedIntents.length > 0) {
                        this.SubCat = [];
                        for (var i = 0; i < res.RelatedIntents.length; i++) {
                            this.SubCat.push({
                                "id": res.RelatedIntents[i],
                                "name": res.RelatedIntents[i],
                            });
                        }
                    }
                    if (res.Processes.length > 0) {
                        this.processes = [];
                        for (var i = 0; i < res.Processes.length; i++) {
                            if (i == res.Processes.length - 1)
                                this.processes.push({
                                    "id": res.Processes[i].Id,
                                    "title": res.Processes[i].Title,
                                    "desc": res.Processes[i].Description + '</br>' + res.Processes[i].ActionJson,
                                    "class": "padding0"
                                });
                            else
                                this.processes.push({
                                    "id": res.Processes[i].Id,
                                    "title": res.Processes[i].Title,
                                    "desc": res.Processes[i].Description + '</br>' + res.Processes[i].ActionJson,
                                    "class": ""
                                });
                        }
                    }
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
        this.router.navigate(['/' + route, linkName]);

        

    }
       
}