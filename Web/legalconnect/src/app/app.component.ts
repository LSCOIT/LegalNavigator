import { Component,OnInit } from '@angular/core';
declare var device:any;
@Component({
  selector: 'app',
  template: '<router-outlet></router-outlet>',
  //styleUrls:['../LegalAssist.scss']
})
export class AppComponent implements OnInit { 
    // name = 'Angular';
    ngOnInit() {
        document.addEventListener("deviceready", onDeviceReady, false);
        function onDeviceReady() {
            alert(device.platform);
        }
    }
}
