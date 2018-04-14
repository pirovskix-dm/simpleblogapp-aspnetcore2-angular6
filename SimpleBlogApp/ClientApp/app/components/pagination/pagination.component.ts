import { Component, OnInit, OnChanges, Input, Output, EventEmitter } from '@angular/core';
import { NgModel } from '@angular/forms';

@Component({
    selector: 'app-pagination',
    templateUrl: './pagination.component.html',
    styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnChanges {
    @Input('total-items') totalItems: number = 0;
    @Input('page-size') pageSize: number = 10;
    @Input('current-page') currentPage: number = 1;
    @Output('page-changed') pageChanged: EventEmitter<number> = new EventEmitter<number>();
    pages: number[] = [];
    constructor() { }
    ngOnInit(): void {
    }
    ngOnChanges(): void {
        this.pages = [];

        let pagesCount = Math.ceil(this.totalItems / this.pageSize); 
        for(let i = 1; i <= pagesCount; i++) {
            this.pages.push(i);
        }
    }
    changePage(page: number) : void {
        this.currentPage = page;
        this.pageChanged.emit(this.currentPage);
    }
    previous(): void {
        if (this.currentPage == 1)
            return;

        this.currentPage--;
        this.pageChanged.emit(this.currentPage);
    }
    next(): void {
        if (this.currentPage == this.pages.length)
            return;

        this.currentPage++;
        this.pageChanged.emit(this.currentPage);
    }
}
