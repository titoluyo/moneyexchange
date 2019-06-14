import { TestBed, getTestBed, async } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { Exchange} from '../models/exchange'
import { ExchangeService } from './exchange.service';
import { CacheService } from '../services/cache.service';
import { environment } from '../../environments/environment';

describe('ExchangeService', () => {
  let injector: TestBed;
  //let service: ExchangeService;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      declarations: [

      ],
      providers: [
        CacheService,
        ExchangeService
      ]
    });
    injector = getTestBed();
    //service = injector.get(ExchangeService);
    httpMock = injector.get(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    const service: ExchangeService = injector.get(ExchangeService);
    expect(service).toBeTruthy();
  });

  it('testing call api', (done) => {
    const source = 'USD';
    const target = 'EUR';
    const service: ExchangeService = injector.get(ExchangeService);
    service.getRates(source, target)
      .subscribe((exchange: Exchange) => {
        console.log(exchange);
        expect(exchange.rates[target]).toBe(0.9876)
        done();
      })
    
    const dummyData = {
      id:'36fd83f5-7dca-4175-96ae-cb0abb46ce9e',
      base:'USD',
      date:'2019-06-14',
      rates:{
        EUR:0.9876
      }
    };
    const urlExchange = '/latest';
    const url = `${environment.hostExchangeService}${urlExchange}?base=${source}&symbols=${target}`;
console.log(`url: ${url}`);
    const req = httpMock.expectOne(url);
    expect(req.request.method).toBe("GET");
    req.flush(dummyData)

  });

  it('testing convert currency', (done) => {
    const source = 'USD';
    const target = 'EUR';
    const amount = 1000;
    const service: ExchangeService = injector.get(ExchangeService);
    service.convertCurrency(source, target, amount)
      .subscribe((value: number) => {
        console.log(value);
        expect(value).toBe(987.6)
        done();
      })
    
    const dummyData = {
      id:'36fd83f5-7dca-4175-96ae-cb0abb46ce9e',
      base:'USD',
      date:'2019-06-14',
      rates:{
        EUR:0.9876
      }
    };
    const urlExchange = '/latest';
    const url = `${environment.hostExchangeService}${urlExchange}?base=${source}&symbols=${target}`;
console.log(`url: ${url}`);
    const req = httpMock.expectOne(url);
    expect(req.request.method).toBe("GET");
    req.flush(dummyData)

  });

});
