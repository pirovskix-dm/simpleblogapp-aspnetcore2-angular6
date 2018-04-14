import { map } from 'rxjs/operators';
import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { TagViewModel } from '../models/tagviewmodel';

@Injectable()
export class TagService {
    private readonly tagsEndpoint = '/api/tags';
    constructor(private http: Http) { 
    }
    getTags(name: string, records: number): Observable<TagViewModel[]> {
        return this.http.get(`${this.tagsEndpoint}?name=${name}&records=${records}`).pipe(
            map((res: any) => res.json() as TagViewModel[])
        );
    }
}
