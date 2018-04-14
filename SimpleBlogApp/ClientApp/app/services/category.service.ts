import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { CategoryViewModel } from '../models/categoryviewmodel';
import { Observable } from 'rxjs/Observable';
import { SaveCategoryViewModel } from '../models/savecategoryviewmodel';

@Injectable()
export class CategoryService {
    private readonly categoriesEndpoint = '/api/categories';
    constructor(private http: Http) { 
    }
    getCategories(): Observable<CategoryViewModel[]> {
        return this.http.get(this.categoriesEndpoint).pipe(
            map((res: any) => res.json() as CategoryViewModel[])
        );
    }
    create(savePost: SaveCategoryViewModel): Observable<number>{
        return this.http.post(this.categoriesEndpoint, savePost).pipe(
            map((res: any) => res.json() as number)
        );
    }
    delete(id: number): Observable<number>{
        return this.http.delete(`${this.categoriesEndpoint}/${id}`).pipe(
            map((res: any) => res.json() as number)
        );
    }
}
