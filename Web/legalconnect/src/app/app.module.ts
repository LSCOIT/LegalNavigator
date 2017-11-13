import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
// the browser platform with a compiler
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { FormsModule } from '@angular/forms';  
import { HttpModule, JsonpModule } from '@angular/http';  
import { routing } from './app.routes';
import { Ng2AutoCompleteModule } from 'ng2-auto-complete';
import { AppComponent } from './app.component';
import { SearchComponent } from './search/search.component';
import { DemoComponent } from './demo/demo.component';
import { HeaderComponent } from './common/header.component';
import { MRSComponent } from './common/mrs.component'
import { FooterComponent } from './common/footer.component'; 
import { GeneralComponent } from './tabs/general/general.component';
import { GuidedComponent } from './tabs/guided/guided.component';
import { DocComponent } from './tabs/doc/doc.component';
import { ChatComponent } from './tabs/chat/chat.component';
import { TLComponent } from './tabs/timeline/timeline.component';
import {SearchService} from './services/search.service';
@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        JsonpModule, 
        routing,
        Ng2AutoCompleteModule
    ],
    declarations: [AppComponent,
        SearchComponent,
        DemoComponent,
        HeaderComponent,
        MRSComponent, 
        FooterComponent,
        GeneralComponent,
        GuidedComponent,
        DocComponent,
        ChatComponent,
        TLComponent
    ],
    providers: [
        SearchService
    ],  
    bootstrap:    [ AppComponent ]    
})
export class AppModule { }
