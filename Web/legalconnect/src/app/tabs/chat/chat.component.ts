﻿import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchService } from '../../services/search.service';
import {MRSComponent} from '../../common/mrs.component';

@Component({
    selector: 'chat',
    templateUrl: './chat.component.html'
         
})
export class ChatComponent implements OnInit {
    @ViewChild(MRSComponent) private _mrs:
    MRSComponent;
    files: FileList;
    geoCountry: any;
    lang: string = "en";
    selectedCountry: string = " ";
    isVisible: boolean = true;
    showMessage: boolean = false;
    sub: any;
    returnFlag: string = 'false';
    location: string;
    obj: any;
    scenarios: Array<any> = [];
    hasScenario: boolean = false;
    rightboxes = [{
        "url": "",
        "text": "",
        "self": false
    }];
    messages = [{
        "text": "",
        "self": "false",
        "time": "",
        "class": "",
        "classIcon": "",
        "from":"",
        "scenarios": [{}]
    }];
    returnMessages = [{
        "text": "",
        "self": "false",
        "time": "",
        "class": "",
    }]

    replyMessage = "";
    rightmessMessage = "";
    query: string = "";
    constructor(private srchServ: SearchService, private router: Router, private route: ActivatedRoute) { }

    ngOnInit() {
        localStorage.setItem('scrolled', 'true');
      
        if (localStorage.getItem('geoState') != null)
            this.selectedCountry = localStorage.getItem('geoState');
        else
            this.selectedCountry = "Alaska";

        var msgs = JSON.parse(localStorage.getItem('messages'));
        
        if (msgs != null) {
            for (var m = 0; m < msgs.length;m++)
            this.messages.push({
                "text": msgs[m].text,
                //"self": true,
                "self": msgs[m].self,
                "time": time,
                "class": msgs[m].class,
                "classIcon": msgs[m].classIcon,
                "from": "You",
                "scenarios": msgs[m].scenarios
                })
            localStorage.setItem('scrolled', 'false');
        }
        var ref = JSON.parse(localStorage.getItem('references'));
        console.log('ref history', ref);
        if (ref != null) {
            for (var r = 0; r < ref.length; r++)
                this.rightboxes.push({
                    "url": ref[r].url,
                    "text": ref[r].text,
                    "self": true
                })
            localStorage.setItem('resScrolled', 'false');
        }
        //if (localStorage.getItem('geoState') == null)
        //this.srchServ.getLocation().subscribe(
        //    (resCountry) => {
        //        localStorage.setItem('geoState', resCountry.region);
        //    });
        localStorage.setItem('sentence', "");
        localStorage.setItem('linkName', "");
        this.query = this.replyMessage;
        this.returnFlag = localStorage.getItem('returnFlag') == null ? 'false' : localStorage.getItem('returnFlag');
        
        var dt = new Date();
        var time = dt.getHours() + ":" + dt.getMinutes();
        setTimeout(() => {
            time = dt.getHours() + ":" + dt.getMinutes();

            this.returnMessages.push({
                "text": "I have an eviction notice",
                "self": "sentTrue",
                "time": time,
                "class": "sent",
            })
        }, 500);

        setTimeout(() => {
            time = dt.getHours() + ":" + dt.getMinutes();
            this.returnMessages.push({
                "text": "do you need assistance with it",
                "self": "true",
                "time": time,
                "class": "receive",
            })
        }, 500);

        setTimeout(() => {
            time = dt.getHours() + ":" + dt.getMinutes();
            this.returnMessages.push({
                "text": "yes, I don’t understand it",
                "self": "sentTrue",
                "time": time,
                "class": "sent",
            })
        }, 500);

        setTimeout(() => {
            time = dt.getHours() + ":" + dt.getMinutes();
            this.returnMessages.push({
                "text": "use the camera icon to take a picture of it",
                "self": "true",
                "time": time,
                "class": "receive",
            })
        }, 500);
    }

    onChange(files: FileList) {
        this.files = files;
        var file: File;

        for (var i = 0; i < files.length; i++) {
            file = files[i];
            this.rightboxes.push({
                "url": "",
                "text": files[i].name,
                "self": true
            })
            this.showMessage = true;
            var reader: FileReader = new FileReader();
            var data: FormData = new FormData();
            data.append('file-' + i, file);

            this.srchServ.getfileUpload(data).subscribe(
                (res) => {
                    
                    var dt = new Date();
                    var time = dt.getHours() + ":" + dt.getMinutes();

                    this.messages.push({
                        "text": res._body,
                        "self": "true",
                        "time": time,
                        "class": "receive",
                        "classIcon": "receiveIcon",
                        "from": "LA",
                        "scenarios": [{}]
                    })

                    this.showMessage = false;


                },
                (err: any) => {
                    this.showMessage = false;
                    console.log(err);

                });

        }


    }

    reply() {
        localStorage.setItem('showProcess', 'false');
        this.scenarios = [];
        this.hasScenario = false;
        
        localStorage.setItem('curatesResources', null);
        this.isVisible = false;
        var dt = new Date();
        var time = dt.getHours() + ":" + dt.getMinutes();
        this.messages.push({
            "text": this.replyMessage,
            //"self": true,
            "self": "sentTrue",
            "time": time,
            "class": "sent",
            "classIcon": "sentIcon",
            "from": "You",
            "scenarios": this.scenarios
        })  
        localStorage.setItem('messages', JSON.stringify(this.messages));
        var query = this.replyMessage;
        this.showMessage = true;
        if (this.selectedCountry!==" ")
        localStorage.setItem('geoState', this.selectedCountry);
              
                
                this.srchServ.getChatReferences(query, this.selectedCountry == " " ? localStorage.getItem('geoState') : this.selectedCountry).subscribe((res) => {
                   
                    if (res != null && res.length > 0) {
                        this.rightboxes = [];
                        for (var i = 0; i < res.length; i++) {
                            this.rightboxes.push({
                                "url": res[i].Url,
                                "text": res[i].Name,
                                "self": true
                            })
                        }
                        localStorage.setItem('references', JSON.stringify(this.rightboxes));
                        localStorage.setItem('resScrolled', 'false');
                    }
                });
              this.isVisible = true;
   
        this.replyMessage = "";
      
        
        localStorage.setItem('sentence', query);

        if (localStorage.getItem('geoState') != 'Washington') {
            this.srchServ.getCuratedContents(query, localStorage.getItem('geoState'))
                .subscribe((res) => {
                 
                    var i = 0;
                    if (res != null && res.Description != null) {
                        localStorage.setItem('curatesResources', JSON.stringify(res)); //store data
                        localStorage.setItem('hasData', "true");
                        if (res.Scenarios.length > 0) {
                            this.messages.push({
                                "text": "Which one of these best matches your situation?",
                                "self": "true",
                                "time": time,
                                "class": "receive",
                                "classIcon": "receiveIcon",
                                "from": "LA",
                                "scenarios": this.scenarios
                            })

                            this.scenarios = res.Scenarios;

                            this.messages.push({
                                "text": "",
                                "self": "true",
                                "time": "",
                                "class": "tag-badge",
                                "classIcon": "",
                                "from": "",
                                "scenarios": this.scenarios
                            })
                            this.hasScenario = true;
                            localStorage.setItem('messages', JSON.stringify(this.messages));
                        }

                        localStorage.setItem('scrolled', 'false');
                    }
                    else {
                        this.srchServ.getChatMessages(query, this.lang, this.selectedCountry == " " ? localStorage.getItem('geoState') : this.selectedCountry)
                            .subscribe((res) => {
                              
                                if (res != null && res != '') {
                                    var dt = new Date();
                                    var time = dt.getHours() + ":" + dt.getMinutes();

                                    this.messages.push({
                                        "text": res,
                                        "self": "true",
                                        "time": time,
                                        "class": "receive",
                                        "classIcon": "receiveIcon",
                                        "from": "LA",
                                        "scenarios": this.scenarios

                                    });

                                    this.showMessage = false;
                                    localStorage.setItem('messages', JSON.stringify(this.messages));
                                }
                                else {
                                    this.messages.push({
                                        "text": "Please try with other option",
                                        "self": "sentTrue",
                                        "time": "",
                                        "class": "sent",
                                        "classIcon": "sentIcon",
                                        "from": "You",
                                        "scenarios": this.scenarios
                                    })
                                    localStorage.setItem('messages', JSON.stringify(this.messages));
                                }
                                localStorage.setItem('scrolled', 'false');
                            },
                            (err: any) => {
                                this.showMessage = false;
                                this.messages.push({
                                    "text": "Please try with other option",
                                    "self": "sentTrue",
                                    "time": "",
                                    "class": "sent",
                                    "classIcon": "sentIcon",
                                    "from": "You",
                                    "scenarios": this.scenarios
                                })
                                localStorage.setItem('messages', JSON.stringify(this.messages));
                                console.log('error', err);

                            });
                        localStorage.setItem('hasData', "false");

                    }


                    this._mrs.ngOnInit();
                },
                (err: any) => {

                    console.log('curated error', err);


                }
                );
        }
        else if (localStorage.getItem('geoState') == 'Washington') {
            localStorage.setItem('hasData', "true");
            this.srchServ.getChatMessages(query, this.lang, this.selectedCountry == " " ? localStorage.getItem('geoState') : this.selectedCountry)
                .subscribe((res) => {
                    console.log('result', res);
                    if (res != null && res != '') {
                        var dt = new Date();
                        var time = dt.getHours() + ":" + dt.getMinutes();

                        this.messages.push({
                            "text": res,
                            "self": "true",
                            "time": time,
                            "class": "receive",
                            "classIcon": "receiveIcon",
                            "from": "LA",
                            "scenarios": this.scenarios

                        });
                        localStorage.setItem('messages', JSON.stringify(this.messages));
                        this.showMessage = false;
                    }
                    else {
                        this.messages.push({
                            "text": "Please try with other option",
                            "self": "sentTrue",
                            "time": "",
                            "class": "sent",
                            "classIcon": "sentIcon",
                            "from": "You",
                            "scenarios": this.scenarios
                        })
                        localStorage.setItem('messages', JSON.stringify(this.messages));
                    }
                    localStorage.setItem('scrolled', 'false');
                    this._mrs.ngOnInit();
                },
                (err: any) => {
                    this.showMessage = false;
                    this.messages.push({
                        "text": "Please try with other option",
                        "self": "sentTrue",
                        "time": "",
                        "class": "sent",
                        "classIcon": "sentIcon",
                        "from": "You",
                        "scenarios": this.scenarios
                    })
                    localStorage.setItem('messages', JSON.stringify(this.messages));
                    console.log('error', err);

                });
        }
                    this.showMessage = false;

                    setInterval(this.updateScroll, 100);
                    setInterval(this.updateResScroll, 100);
                
        
    }

    updateScroll() {

        if (localStorage.getItem('scrolled') == 'false') {
            var element = document.getElementById('chat_msgs');
            element.scrollTop = element.scrollHeight;
        }
    }

    updateResScroll() {

        if (localStorage.getItem('resScrolled') == 'false') {
            var element = document.getElementById('chat_resources');
            element.scrollTop = element.scrollHeight;
        }
    }
    onChatScroll() {
       
        localStorage.setItem('scrolled', 'true');

    }

    onResScroll() {
       
        localStorage.setItem('resScrolled', 'true');

    }

    showResourcesByScenario(scenarioId: string, Description: string) {
      
        localStorage.setItem('showProcess', 'true');
        localStorage.setItem('curatesResources', null);
        this.srchServ.getCurScenarios(scenarioId, localStorage.getItem('geoState'))
            .subscribe((res) => {
                var i = 0;
                this.messages.push({
                    "text": Description,
                    "self": "sentTrue",
                    "time": "",
                    "class": "sent",
                    "classIcon": "sentIcon",
                    "from": "You",
                    "scenarios": this.scenarios
                })
                localStorage.setItem('messages', JSON.stringify(this.messages));
                if (res != null && res.Description != null) {
                    localStorage.setItem('curatesResources', JSON.stringify(res)); //store data
                    localStorage.setItem('hasData', "true");
                    console.log('sen by id', res);
                   
                  //  if (res.Scenarios.length > 0) {
                        this.messages.push({
                            "text": res.Outcome,
                            "self": "true",
                            "time": "",
                            "class": "receive",
                            "classIcon": "receiveIcon",
                            "from": "LA",
                            "scenarios": this.scenarios

                    });
                        localStorage.setItem('messages', JSON.stringify(this.messages));
                    //    this.scenarios = res.Scenarios;

                        //this.messages.push({
                        //    "text": "",
                        //    "self": "true",
                        //    "time": "",
                        //    "class": "tag-badge",
                        //    "classIcon": "",
                        //    "from": "",
                        //    "scenarios": this.scenarios
                        //})
                        this.hasScenario = true;


                  //  }
                    this.messages.push({
                        "text": "Please Scroll Down To See Resources & Forms/Steps",
                        "self": "sentTrue",
                        "time": "",
                        "class": "sent",
                        "classIcon": "sentIcon",
                        "from": "You",
                        "scenarios": this.scenarios
                        })
                    localStorage.setItem('messages', JSON.stringify(this.messages));
                }
                else {
                    localStorage.setItem('hasData', "false");
                   
                    this.messages.push({
                        "text": "Please try with other option",
                        "self": "sentTrue",
                        "time": "",
                        "class": "sent",
                        "classIcon": "sentIcon",
                        "from": "You",
                        "scenarios": this.scenarios
                    })
                    localStorage.setItem('messages', JSON.stringify(this.messages));
                }
                localStorage.setItem('scrolled', 'false');
                this._mrs.ngOnInit();
            },
            (err: any) => {
                this.messages.push({
                    "text": "Please try with other option",
                    "self": "sentTrue",
                    "time": "",
                    "class": "sent",
                    "classIcon": "sentIcon",
                    "from": "You",
                    "scenarios": this.scenarios
                })
                localStorage.setItem('messages', JSON.stringify(this.messages));
                console.log('curated error', err);


            }
            );

        
    }

    stateOnChange(newValue:string) {
        
        localStorage.setItem('hasData', "false");
        localStorage.setItem('geoState', newValue);
        
    }

    openPop(Url: string) {

        this.location = Url;
    }

    closePop() {
        window.open(this.location, '_blank');
    }
}