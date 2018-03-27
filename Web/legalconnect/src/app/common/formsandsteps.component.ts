import { Component, OnInit, Input  } from '@angular/core';


@Component({
    selector: 'formsSteps',
    templateUrl: './formsandsteps.component.html'
})
export class FormsComponent implements OnInit {
    collapsed: string = 'collapse';
    aClicked: boolean = false;
    processes: Array<any> = [];
    showForms: boolean = false;
    
    constructor()
    { }
  

    ngOnInit() {
        var curResources = JSON.parse(localStorage.getItem('curatesResources'));
        this.collapsed = 'collapse in';
        this.aClicked = true;
        this.processes = [];
        
        if (curResources != null && curResources.Processes.length > 0) {
            
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
            this.showForms = true;
         
        }
        else
            this.showForms = false;
    }
}

