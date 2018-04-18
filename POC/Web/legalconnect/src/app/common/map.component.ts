import { Component, OnInit, Input  } from '@angular/core';
import { DomSanitizer } from "@angular/platform-browser";

@Component({
    selector: 'maps',
    templateUrl: './map.component.html'
})
export class MapComponent implements OnInit {
    mapUrl: any;
    constructor(private domSanitizer: DomSanitizer)
    { }


    ngOnInit() {

        this.mapUrl = this.domSanitizer.bypassSecurityTrustResourceUrl("https://www.google.com/maps/embed?pb=!1m16!1m12!1m3!1d2775832.4157781773!2d-122.40028915806761!3d47.20143890666706!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!2m1!1sLegal+help+in+Washington+states!5e0!3m2!1sen!2sin!4v1498039664011");
    }
    
}

