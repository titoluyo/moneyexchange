import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CurrencyMaskModule } from "ng2-currency-mask";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';


import { ExchangeService } from './services/exchange.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    CurrencyMaskModule
  ],
  providers: [
    ExchangeService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
