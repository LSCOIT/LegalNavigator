import { Component, OnInit, Input  } from '@angular/core';

@Component({
    selector: 'subCat',
    templateUrl: './subcategory.component.html'
})
export class SubCatComponent implements OnInit {
    SubCat: Array<any> = [];
    showSubCat: boolean = false;
    constructor()
    { }


    ngOnInit() {
        this.SubCat = [];
        var curResources = JSON.parse(localStorage.getItem('curatesResources'));

        if (curResources != null && curResources.RelatedIntents != null && curResources.RelatedIntents.length > 0) {
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

         }
}

