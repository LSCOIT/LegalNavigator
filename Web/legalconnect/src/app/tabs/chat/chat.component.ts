import { Component, OnInit, ViewChild, Input } from '@angular/core';
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
        this.selectedCountry = 'Alaska';
        this.srchServ.getLocation().subscribe(
            (resCountry) => {
                localStorage.setItem('geoState', resCountry.region);
            });
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
        this.scenarios = [];
        this.hasScenario = false;
        localStorage.setItem('hasData','true');
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
        var query = this.replyMessage;
        this.showMessage = true;
        if (this.selectedCountry!==" ")
        localStorage.setItem('geoState', this.selectedCountry);
              
                this.rightboxes = [];
                this.srchServ.getChatReferences(query, this.selectedCountry == " " ? localStorage.getItem('geoState') : this.selectedCountry).subscribe((res) => {
                    if (res.length > 0) {
                        for (var i = 0; i < res.length; i++) {
                            this.rightboxes.push({
                                "url": res[i].Url,
                                "text": res[i].Name,
                                "self": true
                            })
                        }

                        localStorage.setItem('resScrolled', 'false');
                    }
                });
              this.isVisible = true;
   
        this.replyMessage = "";
      
        
        localStorage.setItem('sentence', query);

        this.srchServ.getCuratedContents(query, localStorage.getItem('geoState'))
            .subscribe((res) => {
                console.log('curated', res);
                var i = 0;
                if (res != null) {
                    localStorage.setItem('curatesResources', JSON.stringify(res)); //store data
                    localStorage.setItem('hasData', "true");
                    if (res.Scenarios.length > 0) {
                        this.messages.push({
                            "text": "What are you looking for?",
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

                    }
                    localStorage.setItem('scrolled', 'false');
                }
                else {
                    this.srchServ.getChatMessages(query, this.lang, this.selectedCountry == " " ? localStorage.getItem('geoState') : this.selectedCountry)
                        .subscribe((res) => {
                                if (res != null) {
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
                            }
                                localStorage.setItem('scrolled', 'false');
                        },
                        (err: any) => {
                            this.showMessage = false;
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

    showResourcesByScenario(scenarioId: string, Description:string) {
       
        localStorage.setItem('hasData', 'true');
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
                if (res != null) {
                    localStorage.setItem('curatesResources', JSON.stringify(res)); //store data
                    localStorage.setItem('hasData', "true");
                    console.log('sen by id', res);
                    if (res.Scenarios.length > 0) {
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
                        
        
                    }
                }
                else
                    localStorage.setItem('hasData', "false");
                localStorage.setItem('scrolled', 'false');
                this._mrs.ngOnInit();
            },
            (err: any) => {

                console.log('curated error', err);


            }
            );

        
    }

    openPop(Url: string) {

        this.location = Url;
    }

    closePop() {
        window.open(this.location, '_blank');
    }
}