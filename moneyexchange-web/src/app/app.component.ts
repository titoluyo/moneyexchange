import { Component, OnInit } from '@angular/core';
import { Exchange } from './models/exchange';
import { ExchangeService } from './services/exchange.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  model = {
    usd: 0,
    eur: 0
  }

  constructor(private exchangeService: ExchangeService) { }

  ngOnInit(): void {
    
  }

  onCalculate() {
    const self = this;
    self.exchangeService.convertCurrency('USD', 'EUR', this.model.usd)
      .subscribe((value: number) => {
        this.model.eur = value;
      });
  }
}
