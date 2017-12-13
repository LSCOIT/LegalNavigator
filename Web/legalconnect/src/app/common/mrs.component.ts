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
        if (hasData==="true" && ((curResources != null && curResources.Description === null) || curResources === null)) {  //if local storage is null call service
        
            this.srchServ.getCuratedContents(this.sentence, localStorage.getItem('geoState'))
                .subscribe((res) => {
                    var i = 0;
                    if (res != null) {
                        
                        localStorage.setItem('curatesResources', JSON.stringify(res)); //store data
                        localStorage.setItem('hasData', "true"); 
                        this.hasData = true;
                        if (res.Resources.length > 0) {
                            console.log(res.Resources);
                            this.items = [];
                            while (i < res.Resources.length) {
                                var Resources: Array<any> = [];
                                for (var j = i; (j < i + 4 && j < res.Resources.length); j++) {
                                    if (res.Resources[j].Action!='Title')
                                    {
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
                                            "body": res.Resources[j].LawCategory.Description,
                                            

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
                                // if (res.Processes[i].ActionJson == "Title") {
                                this.processes.push({
                                    "id": res.Processes[i].Id,
                                    "title": res.Processes[i].ActionJson,
                                    "desc": res.Processes[i].Description,
                                });
                                //}
                            }
                        }
                    }
                    else {
                        this.hasData = false;
                        localStorage.setItem('hasData', "false"); 
                           this.items = [];
                        var Resources: Array<any> = [{
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
                        this.items.push({ Resources, class: "active" });
                        this.SubCat = [{
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
                        this.processes = [];
                        
                    }
                },
                (err: any) => {

                    console.log('curated error', err);


                }
                );
        }
        else if (hasData == "true"){  //display ui from local storage
            var curResources = JSON.parse(localStorage.getItem('curatesResources'));
            console.log(curResources);
            this.hasData = true;
            if (curResources.Resources.length > 0) {
                this.items = [];
                while (i < curResources.Resources.length) {
                    var Resources: Array<any> = [];
                    for (var j = i; (j < i + 4 && j < curResources.Resources.length); j++) {
                        if (curResources.Resources[j].Action != 'Title') {
                            if (curResources.Resources[j].Action == 'Url') 
                                {
                                    Resources.push({

                                        "header": curResources.Resources[j].Title,
                                        "body": curResources.Resources[j].ResourceJson == "" ? curResources.Resources[j].LawCategory.Description : curResources.Resources[j].LawCategory.Description + '<br /><br /><b>' + curResources.Resources[j].Action + '</b><br /><div class="topboxUrl" title="' + curResources.Resources[j].ResourceJson+'"><a href="' + curResources.Resources[j].ResourceJson + '"target="_blank">' + curResources.Resources[j].ResourceJson+'</a></div>',

                                    });
                                }
                                else{
                                    Resources.push({

                                        "header": curResources.Resources[j].Title,
                                        "body": curResources.Resources[j].ResourceJson == "" ? curResources.Resources[j].LawCategory.Description : curResources.Resources[j].LawCategory.Description + '<br /><br /><b>' + curResources.Resources[j].Action + '</b><br />' + curResources.Resources[j].ResourceJson,

                                    });
                                }
                        }
                        else {
                            Resources.push({

                                "header": curResources.Resources[j].Title,
                                "body": curResources.Resources[j].LawCategory.Description,

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
                this.SubCat = [];
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
                    // if (res.Processes[i].ActionJson == "Title") {
                    this.processes.push({
                        "id": curResources.Processes[i].Id,
                        "title": curResources.Processes[i].ActionJson,
                        "desc": curResources.Processes[i].Description,
                    });
                    //}
                }
            }
        }
        else if(hasData=="false")
        {
            this.hasData = false;
            this.items = [];
            var Resources: Array<any> = [{
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
            this.items.push({ Resources, class: "active" });
            this.SubCat = [{
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
            this.processes = [];}
            
        
       
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
        console.log('cat click function', linkName);
        localStorage.setItem('linkName', linkName);
        
        this.aciveBdg = linkName;
        var route:string;

        if (this.currentUrl.indexOf('general') > 0) route = "general";
        else if (this.currentUrl.indexOf('chat') > 0) route = "chat";
        else if (this.currentUrl.indexOf('guided') > 0) route = "guided";
        else route = "general";
        document.getElementById('mrlTopics').scrollIntoView();
    
        this.srchServ.getCuratedContents(linkName, localStorage.getItem('geoState'))
            .subscribe((res) => {
                var i = 0;
                if (res != null) {

                    localStorage.setItem('curatesResources', JSON.stringify(res)); //store data
                    localStorage.setItem('hasData', "true");
                    this.hasData = true;
                    if (res.Resources.length > 0) {
                        console.log(res.Resources);
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
                                        "body": res.Resources[j].LawCategory.Description,


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
                            // if (res.Processes[i].ActionJson == "Title") {
                            this.processes.push({
                                "id": res.Processes[i].Id,
                                "title": res.Processes[i].ActionJson,
                                "desc": res.Processes[i].Description,
                            });
                            //}
                        }
                    }
                }
                else {
                    this.hasData = false;
                    localStorage.setItem('hasData', "false");
                    this.items = [];
                    var Resources: Array<any> = [{
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
                    this.items.push({ Resources, class: "active" });
                    this.SubCat = [{
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
                    this.processes = [];

                }
            },
            (err: any) => {

                console.log('curated error', err);


            }
            );
        
        this.router.navigate(['/' + route, linkName]);

        

    }
       
}