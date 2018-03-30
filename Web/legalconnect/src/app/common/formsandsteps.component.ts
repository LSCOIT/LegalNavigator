import { Component, OnInit, Input  } from '@angular/core';
import { DomSanitizer } from "@angular/platform-browser";


@Component({
    selector: 'formsSteps',
    templateUrl: './formsandsteps.component.html'
})
export class FormsComponent implements OnInit {
    collapsed: string = 'collapse';
    aClicked: boolean = false;
    processes: Array<any> = [];
    showForms: boolean = false;
    
    constructor(private domSanitizer: DomSanitizer)
    { }
  

    ngOnInit() {
        var curResources = JSON.parse(localStorage.getItem('curatesResources'));
        this.collapsed = 'collapse in';
        this.aClicked = true;
        this.processes = [];
        
        if (curResources != null && curResources.Processes.length > 0) {
            console.log(curResources.Processes);
            for (var i = 0; i < curResources.Processes.length; i++) {
                if (curResources.Processes[i].Description != null) {
                    var item = this.processes.find(x => x.title == curResources.Processes[i].Title);
                    if (item != null || item != undefined)
                    {
                        
                        if (curResources.Processes[i].ActionJson != null) {
                            console.log(curResources.Processes[i].ActionJson.indexOf('class ="video"'));
                            if (curResources.Processes[i].ActionJson.indexOf('class ="video"') > 0) {
                                var ind1 = curResources.Processes[i].ActionJson.indexOf("https");
                                var ind2 = curResources.Processes[i].ActionJson.indexOf("class");
                                var str_url = curResources.Processes[i].ActionJson.substr(ind1, ind2 - ind1 - 2);
                                console.log(str_url);
                                item.url = this.domSanitizer.bypassSecurityTrustResourceUrl(str_url);
                                item.class = "video";
                                
                            }
                        }
                        else
                      
                        item.desc = curResources.Processes[i].ActionJson == null ? item.desc + '<ul><li>' + curResources.Processes[i].Description + '</li></ul>' : item.desc + '<ul><li>' + curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson + '</li></ul>'
                    }
                    else {
                        var str_url1 = "";
                        
                        if (curResources.Processes[i].ActionJson != null) {
                            console.log(curResources.Processes[i].ActionJson.indexOf('class ="video"') );
                            if (curResources.Processes[i].ActionJson.indexOf('class ="video"') > 0) {
                                var ind1 = curResources.Processes[i].ActionJson.indexOf("https");
                                var ind2 = curResources.Processes[i].ActionJson.indexOf("class");
                                str_url1 = curResources.Processes[i].ActionJson.substr(ind1, ind2 - ind1 - 2);
                                console.log(str_url1);
                                

                                this.processes.push({
                                    "id": curResources.Processes[i].stepNumber,
                                    "title": curResources.Processes[i].Title,
                                    //  "desc": curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson,
                                    "desc": '<ul><li>' + curResources.Processes[i].Description + '</li></ul>',
                                    "class": "video",
                                    "url": this.domSanitizer.bypassSecurityTrustResourceUrl(str_url1)
                                });
                            }
                            else
                            this.processes.push({
                                "id": curResources.Processes[i].stepNumber,
                                "title": curResources.Processes[i].Title,
                                //  "desc": curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson,
                                "desc": curResources.Processes[i].ActionJson == null ? '<ul><li>' + curResources.Processes[i].Description + '</li></ul>' : '<ul><li>' + curResources.Processes[i].Description + '</br>' + curResources.Processes[i].ActionJson + '</li></ul>',
                                "class": "",
                                "url": ""
                            });
                        }

                    }

                }
            }
            this.showForms = true;
         
        }
        else
            this.showForms = false;
    }
}

