import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Ng2CarouselamosModule } from 'ng2-carouselamos';
// the browser platform with a compiler
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { FormsModule } from '@angular/forms';  
import { HttpModule, JsonpModule } from '@angular/http';  
import { routing } from './app.routes';
import { Ng2AutoCompleteModule } from 'ng2-auto-complete';
import { AppComponent } from './app.component';
import { SearchComponent } from './search/search.component';
import { DemoComponent } from './search/demo.component';
import { HeaderComponent } from './common/header.component';
import { MRSComponent } from './common/mrs.component'
import { FooterComponent } from './common/footer.component';
import {ResourcesComponent} from './common/resources.component';
import {FormsComponent} from './common/formsandsteps.component';
import {SubCatComponent} from './common/subcategory.component';
import {VideoComponent} from './common/video.component';
import {MapComponent} from './common/map.component';
import {NoDataComponent} from './common/feature_unavailable.component';
import { GeneralComponent } from './tabs/general/general.component';
import { GuidedComponent } from './tabs/guided/guided.component';
import { DocComponent } from './tabs/doc/doc.component';
import { ChatComponent } from './tabs/chat/chat.component';
import {ChatResComponent} from './tabs/chat/chatresources.component';
import { TLComponent } from './tabs/timeline/timeline.component';
import {SearchService} from './services/search.service';
@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        JsonpModule, 
        routing,
        Ng2AutoCompleteModule,
        Ng2CarouselamosModule
    ],
    declarations: [AppComponent,
        SearchComponent,
        DemoComponent,
        HeaderComponent,
        MRSComponent, 
        FooterComponent,
        ResourcesComponent,
        FormsComponent,
        SubCatComponent,
        VideoComponent,
        MapComponent,
        NoDataComponent,
        GeneralComponent,
        GuidedComponent,
        DocComponent,
        ChatComponent,
        ChatResComponent,
        TLComponent
    ],
    providers: [
        SearchService
    ],  
    bootstrap:    [ AppComponent ]    
})
export class AppModule { }
