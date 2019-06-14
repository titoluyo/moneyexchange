import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CacheService } from './cache.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class ExchangeService {

    private cache = new CacheService();
    private host = 'http://localhost:5000';
    private urlExchange = '/api/exchange';

    constructor(
        private http: HttpClient
    ) {}

    public getRates(): Observable<any> {
        const self = this;
        const url = `${self.host}${self.urlExchange}`;

        return self.cache.get(url, self.http.get(url));
    }

}