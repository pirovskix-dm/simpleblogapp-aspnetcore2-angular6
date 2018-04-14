import { map } from 'rxjs/operators';
import { Login } from './../models/login';
import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class UserService {
    constructor(private http: Http, @Inject('BASE_URL') private originUrl: string ) { 
    }
    userAuthentication(loginModel: Login) {
        return this.http.post(`/api/account/login`, loginModel);
    }
    userLogOff(){
        return this.http.post(`/api/account/logoff`, null);
    }
    userIsSigned(): Observable<boolean> {
        return this.http.get(`${this.originUrl}/api/account/signed`).pipe(
            map((res: any) => res.json() as boolean)
        );
    }
}
