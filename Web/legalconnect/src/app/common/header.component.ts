import { Component } from '@angular/core';
import { Router } from '@angular/router';
//import { Popup } from 'ng2-opd-popup';
@Component({
    selector: 'headerDiv',
    templateUrl: './header.component.html'
})
export class HeaderComponent {
    showDiv: boolean = false;
    constructor(private router: Router) { }
    dispalyDiv() {
        this.showDiv = true;
    }
    returnToChat()
    {
        localStorage.setItem('returnFlag', 'true');
       // this.router.navigate(['/chat'] , { queryParams: { returnFlag: true } });
    }
}

