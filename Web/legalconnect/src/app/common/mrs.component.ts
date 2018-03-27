import { Component, OnInit, ViewChild,Input  } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { SearchService } from '../services/search.service';
import {ResourcesComponent} from './resources.component';
import {SubCatComponent} from './subcategory.component';
import {VideoComponent} from './video.component';

@Component({
    selector: 'mrs',
    templateUrl: './mrs.component.html',
})
export class MRSComponent implements OnInit {
    @ViewChild(ResourcesComponent) private _res: ResourcesComponent;
    @ViewChild(SubCatComponent) private _sbCat: SubCatComponent;
    @ViewChild(VideoComponent) private _vdo: VideoComponent;
    hasData: boolean = true;
   
    aciveBdg: string = 'tenantrights';
    OneClass = "collapse";
    TwoClass = "collapse";
    ThreeClass = "collapse"
    FourClass = "collapse";
    routeUrl: string = "";
    state: string = "";
       
    collapsed: string = 'collapse';
    aClicked: boolean = false;
    sentence: string = "";
    currentUrl: string = '';
    geoCountry: string = "";
    page: string = "chat";
    showWashingData: boolean = false;
    constructor(private router: Router, private aRoute: ActivatedRoute, private srchServ: SearchService)
    { }

    ngOnInit() {
        
        var route: any;
        this.currentUrl = this.router.url; // this will give you current url
        this.page = "chat";
        if (this.currentUrl.indexOf('general') > 0) {
            route = "general";
            this.page = "";
        }
        else if (this.currentUrl.indexOf('chat') > 0) {
            route = "chat";
            this.page = "chat";
        }
        else if (this.currentUrl.indexOf('guided') > 0) {
            route = "guided";
            this.page = "";
        }
        else if (this.currentUrl.indexOf('doc') > 0) {
            route = "doc";
            this.page = "";
        }
        else
            route = "general";

        
        this.sentence = localStorage.getItem('sentence');
        var i: number = 0;
        var j: number;
        var curResources = JSON.parse(localStorage.getItem('curatesResources'));
        var hasData = localStorage.getItem('hasData');
        this.state = localStorage.getItem('geoState');
        
        
            
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
              
            var curResources = JSON.parse(localStorage.getItem('curatesResources'));
            console.log(localStorage.getItem("chatInit"));
            if (curResources != null && this.page === 'chat' && localStorage.getItem("chatInit")!="true") {
                this._res.ngOnInit();
                this._sbCat.ngOnInit();
                this._vdo.ngOnInit();
                localStorage.setItem("chatInit", "true");
            }
                this.hasData = true;
               
               
          
        }
        
        
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
                    
                }
                else {
                    this.hasData = false;
                    localStorage.setItem('hasData', "false");

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
        
        document.getElementById('mrlTopics').scrollIntoView();
      //  this.router.navigate(['/' + route, linkName]);

        

    }

    
       
}