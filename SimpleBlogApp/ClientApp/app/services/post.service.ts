import { map } from 'rxjs/operators';
import { PostQuery } from './../models/postquery';
import { Http } from '@angular/http';
import { Injectable, Output, Inject } from '@angular/core';
import { Postviewmodel } from '../models/postviewmodel';
import { Observable } from 'rxjs/Observable';
import { SavePostViewModel } from '../models/savepostviewmodel';
import { QueryResultPost } from '../models/queryresultpost';

@Injectable()
export class PostService {
    private readonly vehiclesEndpoint = '/api/posts';
    constructor(private http: Http) {
    }
    getPosts(): Observable<Postviewmodel[]> {
        return this.http.get(this.vehiclesEndpoint).pipe(
                map((res: any) => res.json() as Postviewmodel[])
            );
    }
    getPostsQuery(postQury: PostQuery): Observable<QueryResultPost> {
        return this.http.get(`${this.vehiclesEndpoint}/query?${this.toQueryString(postQury)}`).pipe(
                map((res: any) => res.json() as QueryResultPost)
            );
    }
    getAdminQuery(postQury: PostQuery): Observable<QueryResultPost> {
        return this.http.get(`${this.vehiclesEndpoint}/admin?${this.toQueryString(postQury)}`).pipe(
                map((res: any) => res.json() as QueryResultPost)
            );
    }
    getPost(id: number): Observable<Postviewmodel> {
        return this.http.get(`${this.vehiclesEndpoint}/${id}`).pipe(
                map((res: any) => res.json() as Postviewmodel)
            );
    }
    create(savePost: SavePostViewModel): Observable<number>{
        return this.http.post(this.vehiclesEndpoint, savePost).pipe(
            map((res: any) => res.json() as number)
        );
    }
    delete(id: number): Observable<number>{
        return this.http.delete(`${this.vehiclesEndpoint}/${id}`).pipe(
                map((res: any) => res.json() as number)
            );
    }
    update(id: number, post: SavePostViewModel): Observable<number> {
        return this.http.put(`${this.vehiclesEndpoint}/${id}`, post).pipe(
                map((res: any) => res.json() as number)
            );
    }
    private toQueryString(obj: any) {
        let parts: string[] = [];
        for(let prop in obj) {
            let val = obj[prop];
            if (val != null && val != undefined)
                parts.push(encodeURIComponent(prop) + '=' + encodeURIComponent(val));
        }
        return parts.join('&');
    }
}