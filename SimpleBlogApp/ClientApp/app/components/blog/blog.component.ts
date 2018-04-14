import { Router, ActivatedRoute } from '@angular/router';
import { PostService } from './../../services/post.service';
import { Component, OnInit } from '@angular/core';
import { Postviewmodel } from '../../models/postviewmodel';
import { PostQuery } from '../../models/postquery';
import { TagViewModel } from '../../models/tagviewmodel';
import '../../utils/extensions';

@Component({
    selector: 'app-blog',
    templateUrl: './blog.component.html',
    styleUrls: ['./blog.component.css']
})
export class BlogComponent implements OnInit {
    private readonly PAGE_SIZE = 5;
    totalItems: number = 0;
    posts: Postviewmodel[] = [];
    query: PostQuery = {
        tagId: null,
        categoryId: null,
        search: '',
        sortBy: 'DateCreated',
        isSortAscending: false,
        page: 1,
        pageSize: this.PAGE_SIZE
    };
    currentTag: string = '';
    constructor(
        private postService: PostService,
        private router: Router,
        private route: ActivatedRoute
    ) { 
    }
    public ngOnInit(): void {
        this.route.params.subscribe(p => {
            this.query.tagId = +p['id'] || null;
            this.populatePosts();
        });
    }
    public onPagination(page: number): void {
        this.query.page = page;
        this.populatePosts();
    }
    private populatePosts(): void {
        this.postService.getPostsQuery(this.query)
            .subscribe(qr => {
                this.totalItems = qr.totalItems;
                this.posts = qr.items;
                this.posts.forEach(p => p.dateCreated = p.dateCreated.toDDMMYYYY('.'));
                if (this.query.tagId && this.posts.length > 0)
                    this.currentTag = this.findTag(this.posts[0].tags);
            });
    }
    private findTag(tags: TagViewModel[]): string{
        for(var t of tags){
            if (t.id == this.query.tagId)
                return t.name;
        }
        return '';
    }
}
