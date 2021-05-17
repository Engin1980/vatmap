import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PureDataComponent } from './components/pure-data/pure-data.component';
import { MapComponent } from './components/map/map.component';
import {TooltipModule as NgxTooltipModule} from 'ngx-bootstrap/tooltip';
import {PopoverModule as NgxPopoverModule} from 'ngx-bootstrap/popover';


@NgModule({
  declarations: [
    AppComponent,
    PureDataComponent,
    MapComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    NgxTooltipModule.forRoot(),
    NgxPopoverModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
