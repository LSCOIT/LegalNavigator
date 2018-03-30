import { Component, OnInit, Input  } from '@angular/core';

@Component({
    selector: 'noData',
    templateUrl: './feature_unavailable.component.html'
})
export class NoDataComponent implements OnInit {
    showMrs: boolean = true;
    showForms: boolean = true;
    showSubCat: boolean = true;
    currentUrl: string = '';
    page: string = '';
    constructor()
    { }


    ngOnInit() {
        
        this.showMrs = true;
        this.showForms = true;
        this.showSubCat = true;
        var curResources = JSON.parse(localStorage.getItem('curatesResources'));
     
        if (curResources === undefined || curResources==null) {
            this.showMrs = false;
            this.showForms = false;
            this.showSubCat = false;
        }
        if (curResources != null) {
            
            if (curResources.Resources!=undefined && (curResources.Resources === null || curResources.Resources.length < 0))
                this.showMrs = false;
            if (curResources.RelatedResources != undefined && (curResources.RelatedResources === null || curResources.RelatedResources.length < 0))
                this.showMrs = false;
            if (curResources.Processes === null || curResources.Processes.length < 0)
                this.showForms = false;
            if (curResources.RelatedIntents === null || curResources.RelatedIntents.length < 0)
                this.showSubCat = false;
        }
       
    }
}

