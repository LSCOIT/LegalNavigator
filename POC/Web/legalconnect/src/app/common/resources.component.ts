import { Component, OnInit, Input  } from '@angular/core';

@Component({
    selector: 'resources',
    templateUrl: './resources.component.html'
})
export class ResourcesComponent implements OnInit{
    constructor()
    { }
    Resources: Array<any> = [];
    items: Array<any> = [];
    showMrs: boolean = false;
    ngOnInit() {
        
                this.items = [];
                this.Resources = [];
                var curResources = JSON.parse(localStorage.getItem('curatesResources'));
                
                if (curResources != null && curResources.Resources != undefined && curResources.Resources != null && curResources.Resources.length > 0) {
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
                            if (curResources.Resources[i].Action != 'Title' && curResources.Resources[i].Action != null) {
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

                    if (this.Resources.length >= 4) {
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

                    if (this.Resources.length < 4) {

                        var Resources: Array<any> = [];

                        for (var j = i; j < this.Resources.length; j++) {

                            Resources.push({
                                "header": this.Resources[j].Title,
                                "body": this.Resources[j].ResourceJson,

                            });

                        }



                        this.items.push({ Resources, class: "active" });



                    }


                }
                else if (curResources != null && curResources.RelatedResources != undefined && curResources.RelatedResources != null && curResources.RelatedResources.length > 0) {
                    this.showMrs = true;
                    for (var i = 0; i < curResources.RelatedResources.length; i++) {
                        if (curResources.RelatedResources[i].Action == 'Title')
                            this.Resources.push({
                                "Title": curResources.RelatedResources[i].Title,
                                "ResourceJson": curResources.RelatedResources[i].ResourceJson,
                                // "ResourceJson": json,
                            });

                    }

                    for (var i = 0; i < curResources.RelatedResources.length; i++) {

                        var item = this.Resources.find(x => x.Title == curResources.RelatedResources[i].Title);
                        if (item != null && item != undefined) {
                            if (curResources.RelatedResources[i].Action != 'Title' && curResources.RelatedResources[i].Action != null) {
                                if (curResources.RelatedResources[i].ResourceJson != null && curResources.RelatedResources[i].ResourceJson != "") {
                                    if (curResources.RelatedResources[i].Action == 'Url') {
                                        if (curResources.RelatedResources[i].ResourceJson.indexOf('<a') >= 0)
                                            item.ResourceJson = item.ResourceJson + '<br /><br /><b> ' + curResources.RelatedResources[i].Action + ': </b><br/><div class="topboxUrl">' + curResources.RelatedResources[i].ResourceJson + '</div>'
                                        else
                                            item.ResourceJson = item.ResourceJson + '<br /><br /><b> ' + curResources.RelatedResources[i].Action + ': </b><br/><div class="topboxUrl" title="' + curResources.RelatedResources[i].ResourceJson + '"><a href="' + curResources.RelatedResources[i].ResourceJson + '"target="_blank">' + curResources.RelatedResources[i].ResourceJson + '</a></div>'
                                    }

                                    else {

                                        item.ResourceJson = item.ResourceJson + '<br /><br /> <b> ' + curResources.RelatedResources[i].Action + ': </b><br/>' + curResources.RelatedResources[i].ResourceJson
                                    }

                                }
                            }
                        }


                    }
                    i = 0;

                    if (this.Resources.length >= 4) {
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

                    if (this.Resources.length < 4) {

                        var Resources: Array<any> = [];

                        for (var j = i; j < this.Resources.length; j++) {

                            Resources.push({
                                "header": this.Resources[j].Title,
                                "body": this.Resources[j].ResourceJson,

                            });

                        }



                        this.items.push({ Resources, class: "active" });



                    }


                }
                else
                    this.showMrs = false;
                
    }
}

