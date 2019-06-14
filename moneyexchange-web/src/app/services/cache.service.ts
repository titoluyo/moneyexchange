//
//  Based on https://github.com/ashwin-sureshkumar/angular-cache-service-blog/blob/master/src/app/cache.service.ts
//           https://hackernoon.com/angular-simple-in-memory-cache-service-on-the-ui-with-rxjs-77f167387e39
//
//  MIT License
//
//  Copyright (c) 2017 Ashwin Sureshkumar
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
//  documentation files (the "Software"), to deal in the Software without restriction, including without limitation
//  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
//  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all copies or substantial
//  portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
//  LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
//  NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//  WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//  SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

//  We are using this implementation since the Angular http cache is replying on a later version of
//  the core libraries.  When Angular 7 or 8 is released maybe we will be able to replace this with the
//  native Angular cache.

import { Observable, Subject, throwError } from 'rxjs';
import { catchError, tap} from 'rxjs/operators';
import * as _ from 'lodash';
import {HttpErrorResponse} from '@angular/common/http';
import { environment } from '../../environments/environment';

interface CacheContent {
    expiry: number;
    value: any;
}

//  Cache Service is an observables based in-memory cache implementation
//  Keeps track of in-flight observables and sets a default expiry for cached values
//  Usage:
//       this.user = this.cacheService.get(id, this.hackerNewsService.getUser(id))
//          .subscribe(user => {
//               console.log(user);
//           });
//
export class CacheService {

    private cache: Map<string, CacheContent> =  new Map<string, CacheContent>();
    private inFlightObservables: Map<string, Subject<any>> = new Map<string, Subject<any>>();
    private cacheTimeout: number = environment.cacheTimeout * 60 * 1000; // convert milliseconds to minutes

    private options = {
        verbose: false,
        defaultCacheMs: this.cacheTimeout
    };

    constructor() {}

    public setOptions(options) {
        if (options) {
            _.extend(this.options, options);
        }
    }

    //
    //  Gets the value from cache if the key is provided.
    //  If no value exists in cache, then check if the same call exists
    //  in flight, if so return the subject. If not create a new
    //  Subject inFlightObservable and return the source observable.
    //
    public get(key: string, retrievalFunc?: Observable<any>, maxAge?: number): Observable<any> | Subject<any> {
        const self = this;
        if (self.hasValidCachedValue(key)) {
            if (self.options.verbose) {
                console.log(`%cGetting from cache ${key}`, 'color: green');
            }
            return new Observable(observer => {
                setTimeout(() => {
                    observer.next(self.cache.get(key).value);
                    observer.complete();
                });
            });
        }

        if (!maxAge) {
            maxAge = self.options.defaultCacheMs;
        }

        if (self.inFlightObservables.has(key)) {
            return self.inFlightObservables.get(key);
        } else if (retrievalFunc) {
            self.inFlightObservables.set(key, new Subject());
            if (self.options.verbose) {
                console.log(`%c Calling api for ${key}`, 'color: purple');
            }
            return retrievalFunc
                .pipe(catchError(e => {
                    if (e instanceof HttpErrorResponse) {
                        console.log(`%c Request failed for: ${key}  Status: ` + e.status + `  Message: ` + e.statusText, 'color: purple');
                    } else {
                        console.log(`%c Request failed for: ${key} ${e}`, 'color: purple');
                    }

                    self.notifyInFlightObservers(key, e);
                    // and when we have done our job, it's good idea to let the others know self event.
                    // maybe they have their stuffs need to be done too.
                    return throwError(e);
                }))
                .pipe(tap((value) => {
                    self.set(key, value, maxAge);
                    }
                ));
        } else {
            if (self.options.verbose) {
                console.log(`%c Requested key is not available in Cache ${key}`, 'color: red');
            }
            return throwError('Requested key is not available in Cache');
        }
    }

    //
    //  Sets the value with key in the cache
    //  Notifies all observers of the new value
    //
    public set(key: string, value: any, maxAge: number = this.options.defaultCacheMs): void {
        this.cache.set(key, {value: value, expiry: Date.now() + maxAge});
        this.notifyInFlightObservers(key, value);
    }

    //
    //  Checks if the a key exists in cache
    //
    public has(key: string): boolean {
        return this.cache.has(key);
    }

    //
    //  Publishes the value to all observers of the given
    //  in progress observables if observers exist.
    //
    private notifyInFlightObservers(key: string, value: any): void {
        if (this.inFlightObservables.has(key)) {
            const inFlight = this.inFlightObservables.get(key);
            const observersCount = inFlight.observers.length;
            if (observersCount) {
                if (this.options.verbose) {
                    console.log(`%cNotifying ${inFlight.observers.length} flight subscribers for ${key}`, 'color: blue');
                }
                if (value instanceof HttpErrorResponse) {
                    inFlight.error(value);
                } else {
                    inFlight.next(value);
                }
            }
            inFlight.complete();
            this.inFlightObservables.delete(key);
        }
    }

    //
    //  Checks if the key exists and   has not expired.
    //
    private hasValidCachedValue(key: string): boolean {
        if (this.cache.has(key)) {
            if (this.cache.get(key).expiry < Date.now()) {
                this.clearExpired();
                return false;
            }
            return true;
        } else {
            return false;
        }
    }

    //
    //  Remove all cached items; optionally by matching keys against a regular expression
    //
    public clear(regexp?: string) {
        const self = this;
        if (regexp) {
            const regex = new RegExp(regexp);
            const keys = Array.from(self.cache.keys());
            _.forEach(keys, key => {
                if (regex.test(key)) {
                    self.cache.delete(key);
                    if (self.inFlightObservables.has(key)) {
                        self.inFlightObservables.delete(key);
                    }
                }
            });
        } else {
            self.cache.clear();
            self.inFlightObservables.clear();
        }
    }

    //
    //  Remove expired cached items
    //
    public clearExpired() {
        const self = this;
        const now = Date.now();
        const keys = Array.from(self.cache.keys());
        _.forEach(keys, key => {
            if (self.cache.get(key).expiry < now) {
                self.cache.delete(key);
                if (self.inFlightObservables.has(key)) {
                    self.inFlightObservables.delete(key);
                }
            }
        });
    }

}
