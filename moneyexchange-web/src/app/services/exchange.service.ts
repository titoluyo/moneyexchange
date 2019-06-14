import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CacheService } from './cache.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Exchange } from '../models/exchange';
import { environment } from '../../environments/environment';

@Injectable()
export class ExchangeService {

    private host = environment.hostExchangeService;
    private urlExchange = '/latest';

    constructor(
        private cache: CacheService,
        private http: HttpClient
    ) {}

    public getRates(source: string, target: string): Observable<Exchange> {
        const self = this;
        const url = `${self.host}${self.urlExchange}?base=${source}&symbols=${target}`;

        return self.cache.get(url, self.http.get(url));
    }

    public convertCurrency(source: string, target: string, amount: number): Observable<number> {
        const self = this;
        var converter = new Observable<number>((observer) => {
            self.getRates(source, target).subscribe((exchange: Exchange) => {
                var value = amount * exchange.rates[target];
                observer.next(value);
            });
            return { unsubscribe() {}};
        });
        return converter;
    }

}