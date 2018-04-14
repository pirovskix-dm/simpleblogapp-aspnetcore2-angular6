import { Component, OnInit } from '@angular/core';
import { Postviewmodel } from '../../models/postviewmodel';
import { ActivatedRoute, Router } from '@angular/router';
import { PostService } from '../../services/post.service';
import '../../utils/extensions';

@Component({
    selector: 'app-postview',
    templateUrl: './postview.component.html',
    styleUrls: ['./postview.component.css']
})
export class PostviewComponent implements OnInit {
    id: number = 0;
    pageNotFound: boolean = false;
    post: Postviewmodel = {
        id: 0,
        title: '',
        content: '',
        shortContent: '',
        category: { id: 0, name: '' },
        dateCreated: '',
        tags: []
    };
    constructor(
        private postService: PostService,
        private router: Router,
        private route: ActivatedRoute
    ) { 
        route.params.subscribe(p => {
            this.id = +p['id'];
        });
    }
    public ngOnInit() {
        this.populatePost();
    }
    private populatePost(): void {
        this.postService.getPost(this.id)
            .subscribe(post => {
                this.pageNotFound = false;
                this.post = post;
                this.post.dateCreated = this.post.dateCreated.toDDMMYYYY('.');
            }, err => {
                if (err.status && err.status == 404)
                    this.pageNotFound = true;
                else
                    this.router.navigate(['/error']);
            });
    }
}
