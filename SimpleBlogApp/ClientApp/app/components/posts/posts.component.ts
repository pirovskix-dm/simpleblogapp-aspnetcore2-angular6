import { ToastyService } from 'ng2-toasty';
import { Component, OnInit } from '@angular/core';
import { PostService } from './../../services/post.service';
import { CategoryService } from './../../services/category.service';
import { Postviewmodel } from '../../models/postviewmodel';
import { QueryResultPost } from '../../models/queryresultpost';
import { PostQuery } from '../../models/postquery';
import { CategoryViewModel } from '../../models/categoryviewmodel';
import { TagViewModel } from '../../models/tagviewmodel';
import { forEach } from '@angular/router/src/utils/collection';
import '../../utils/extensions';

@Component({
    selector: 'app-posts',
    templateUrl: './posts.component.html',
    styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {
    totalItems: number = 0;
    searchString: string = '';
    posts: Postviewmodel[] = [];
    categories: CategoryViewModel[] = [];
    pageSizeFileter: number[] = [ 5, 10, 15, 20 ]
    query: PostQuery = {
        tagId: null,
        categoryId: null,
        search: '',
        sortBy: 'DateCreated',
        isSortAscending: false,
        page: 1,
        pageSize: this.pageSizeFileter[0]
    };
    columns = [
        { title: 'Id', key: 'Id', isSortable: true },
        { title: 'Title', key: '', isSortable: false },
        { title: 'Category', key: 'Category.Name', isSortable: true },
        { title: 'Tags', key: '', isSortable: false },
        { title: 'Created', key: 'DateCreated', isSortable: true }
    ];
    constructor(
        private postService: PostService,
        private categoryService: CategoryService,
        private toastyService: ToastyService
    ) { 
    }
    public ngOnInit(): void {
        this.populateCategories();
        this.populatePosts();
    }
    public delete(post: Postviewmodel): void {
        this.postService.delete(post.id)
            .subscribe(id => {
                if (this.posts.length < 2 && this.query.page > 1)
                    this.query.page--;
                this.populatePosts();
                this.showSuccessMessage('Post was sucessfully deleted.');
            }, err => this.showErrorMessage('Unable to delete the post.'));
    }
    public onPagination(page: number): void {
        this.query.page = page;
        this.populatePosts();
    }
    public sortBy(column: string): void {
        if (this.query.sortBy === column) {
            this.query.isSortAscending = !this.query.isSortAscending;
        } else {
            this.query.sortBy = column;
            this.query.isSortAscending = true;
        }
        this.populatePosts();
    }
    public onCategoriesFilterChange(): void {
        this.query.page = 1;
        this.populatePosts();
    }
    public onSearch(): void {
        if (this.searchString.length > 3) {
            this.query.search = this.searchString;
            this.populatePosts();
        } else if (this.query.search.length > 3) {
            this.query.search = '';
            this.populatePosts();
        }
    }
    public tagsToString(tags: TagViewModel[]): string {
        return tags
            .slice(0, 6)
            .map<string>(t => t.name)
            .join(', ')
            .concat(tags.length > 6 ? ', ...' : '');
    }
    private populatePosts(): void {
        this.postService.getAdminQuery(this.query)
            .subscribe(qr => {
                this.totalItems = qr.totalItems;
                this.posts = qr.items;
                this.posts.forEach(post => post.dateCreated = post.dateCreated.toDDMMYYYY('.'));
            });
    }
    private populateCategories(): void {
        this.categoryService.getCategories()
            .subscribe(cs => { this.categories = cs; });
    }
    private showSuccessMessage(message: string): void {
        this.toastyService.success({
          title: 'Success', 
          msg: message,
          theme: 'bootstrap',
          showClose: false,
          timeout: 3000
        });
    }
    private showErrorMessage(message: string): void {
        this.toastyService.error({
          title: 'Error', 
          msg: message,
          theme: 'bootstrap',
          showClose: false,
          timeout: 3000
        });
    }
}