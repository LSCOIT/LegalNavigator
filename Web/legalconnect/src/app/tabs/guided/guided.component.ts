import { Component, OnInit, ElementRef } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'guided',
    templateUrl: './guided.component.html',
    
})
export class GuidedComponent implements OnInit{
    text: string = "In this section you will find general legal information, self - help packets and videos on your rights as a<br />renter and what to do if you have a problem with your landlord.<a > http://www.washingtonlawhelp.org/issues/housing/tenants-rights</a>";
    aciveBdg: string = 'tenant';
    OneClass = "collapse";
    TwoClass = "collapse";
    ThreeClass = "collapse"
    FourClass = "collapse";
    routeUrl: string = "";

    currentUrl: string = '';

    activeSld: string = "Slide1";
    myClass: string = "aciveSlide";
    ctrType: string = "button";
    steps: { "id": "Slide2", "question": "Have you received and eviction notice ?", "type": "button", "title":"evictionnotice", "options": [{ "name": "Yes" }, { "name": "No" }] }    //{
    
    constructor(private router: Router, private _el: ElementRef) { }

    ngOnInit() {
        localStorage.setItem('linkName', "");
        this.currentUrl = this.router.url; // this will give you current url

        if (this.currentUrl == '/guided/assist/evictionnotice')
              this.activeSld = "Slide2";
       else if (this.currentUrl == '/guided/assist/housingproblem')
            this.activeSld = "Slide5";
        else if (this.currentUrl == '/guided/assist/begin')
            this.activeSld = "Slide1";
        else if (this.currentUrl == '/guided/assist/aboutnotice') 
            this.activeSld = "Slide3";
        else if (this.currentUrl == '/guided/assist/assistnotice')
            this.activeSld = "Slide4";
           
      
       
    }
        
    ShowNextNavigation(sld:string,title:string)
    {

        this.activeSld = sld;
        this.router.navigate(['/guided/assist', title]);

     
      
    }   

    ShowSkipNavigation(sld: string, title: string)
   {
        this.activeSld = sld;
        this.router.navigate(['/guided/assist', title]);
    }

    ValidateContent(param:string)
    {   
        this.myClass =param;
       
    }
    redirectToForms() {
        document.getElementById('forms_steps').scrollIntoView();
        this.router.navigate(['/guided', 'forms']);
    }
}

