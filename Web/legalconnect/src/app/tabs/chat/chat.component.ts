﻿import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchService } from '../../services/search.service';

@Component({
    selector: 'chat',
    templateUrl: './chat.component.html'
})
export class ChatComponent implements OnInit {
    files: FileList;
    geoCountry: any;
    lang: string = "en";
    selectedCountry: string = " ";
    isVisible: boolean = true;
    showMessage: boolean = false;
    sub: any;
    returnFlag: string = 'false';
    location: string;
    obj:any;
    rightboxes = [{
        "url": "",
        "text": "",
        "self": false
    }];
    messages = [{
        "text": "",
        "self": false,
        "time": "",
        "class": "",
    }];
    returnMessages = [{
        "text": "",
        "self": false,
        "time": "",
        "class": "",
    }]

    replyMessage = "";
    rightmessMessage = "";

    constructor(private srchServ: SearchService, private router: Router, private route: ActivatedRoute) { }

    ngOnInit() {

        this.returnFlag = localStorage.getItem('returnFlag');

        var dt = new Date();
        var time = dt.getHours() + ":" + dt.getMinutes();
        setTimeout(() => {
            time = dt.getHours() + ":" + dt.getMinutes();

            this.returnMessages.push({
                "text": "I have an eviction notice",
                "self": false,
                "time": time,
                "class": "receive",
            })
        }, 500);

        setTimeout(() => {
            time = dt.getHours() + ":" + dt.getMinutes();
            this.returnMessages.push({
                "text": "do you need assistance with it",
                "self": true,
                "time": time,
                "class": "sent",
            })
        }, 500);

        setTimeout(() => {
            time = dt.getHours() + ":" + dt.getMinutes();
            this.returnMessages.push({
                "text": "yes, I don’t understand it",
                "self": false,
                "time": time,
                "class": "receive",
            })
        }, 500);

        setTimeout(() => {
            time = dt.getHours() + ":" + dt.getMinutes();
            this.returnMessages.push({
                "text": "use the camera icon to take a picture of it",
                "self": true,
                "time": time,
                "class": "sent",
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
                    console.log(res._body);
                    var dt = new Date();
                    var time = dt.getHours() + ":" + dt.getMinutes();

                    this.messages.push({
                        "text": res._body,
                        "self": true,
                        "time": time,
                        "class": "receive"
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
      //  document.getElementById('chat_msgs').scrollIntoView();

      
        //document.getElementById('resources').scrollIntoView();
        this.isVisible = false;
        var dt = new Date();
        var time = dt.getHours() + ":" + dt.getMinutes();
        this.messages.push({
            "text": this.replyMessage,
            "self": true,
            "time": time,
            "class": "sent",
        })
        var query = this.replyMessage;
        this.showMessage = true;
        this.srchServ.getLocation().subscribe(
            (resCountry) => {
                this.geoCountry = resCountry.region;

                this.srchServ.getChatMessages(query, this.lang, this.selectedCountry == " " ? this.geoCountry : this.selectedCountry).subscribe(
                    (res) => {

                        var dt = new Date();
                        var time = dt.getHours() + ":" + dt.getMinutes();
                       
                        this.messages.push({
                            "text": res,
                            "self": true,
                            "time": time,
                            "class": "receive"
                        });
                       
                        this.showMessage = false;
                        localStorage.setItem('scrolled', 'false');
                        localStorage.setItem('resScrolled', 'false');

                    },
                    (err: any) => {
                        this.showMessage = false;

                        alert('No data found for location ' + this.geoCountry + ' choose country from dropdown');

                    });

                this.rightboxes = [];
                this.srchServ.getChatReferences(query, this.selectedCountry == " " ? this.geoCountry : this.selectedCountry).subscribe((res) => {
                    for (var i = 0; i < res.length; i++) {
                        this.rightboxes.push({
                            "url": res[i].Url,
                            "text": res[i].Name,
                            "self": true
                        })
                    }
                });
            }

        );
        this.isVisible = true;


        this.replyMessage = "";


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

    displayText(lang: string) {

        this.lang = lang;
    }

    openPop(Url: string) {

        this.location = Url;
    }

    closePop() {
        window.open(this.location, '_blank');
    }
}