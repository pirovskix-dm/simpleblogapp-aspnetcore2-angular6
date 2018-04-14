import { TagViewModel } from './../../models/tagviewmodel';
import { CategoryViewModel } from './../../models/categoryviewmodel';
import { SavePostViewModel } from './../../models/savepostviewmodel';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Postviewmodel } from '../../models/postviewmodel';
import { ToastyService } from 'ng2-toasty';
import { PostService } from '../../services/post.service';
import { CategoryService } from '../../services/category.service';
import { TagService } from '../../services/tag.service';
import { TagModel } from 'ngx-chips/core/accessor';
import '../../utils/extensions';

@Component({
  selector: 'app-postform',
  templateUrl: './postform.component.html',
  styleUrls: ['./postform.component.css']
})
export class PostformComponent implements OnInit {
    private readonly TAGS_AUTOCOMPLETE_MAX = 5; 
    pageTitle: string = '';
    id: number = 0;
    dateCreated: string = '';
    categories: CategoryViewModel[] = [];
    tagsBuilder: any[] = [];
    autocompleteTags: any[] = [];
    savePost: SavePostViewModel = {
        title: '',
        shortContent: '',
        content: '',
        categoryId: null,
        tags: []
    };
    constructor(
        private postService: PostService,
        private categoryService: CategoryService,
        private tagService: TagService,
        private router: Router,
        private route: ActivatedRoute,
        private toastyService: ToastyService
    ) {
    }
    public ngOnInit(): void {
        this.route.params.subscribe(p => {
            this.id = +p['id'] || 0;
            this.pageTitle = this.id ? 'Edit Post' : 'New Post';
            this.populateCategories();
        });
    }
    public onSubmit(form: NgForm): void {
        this.savePost.tags = this.tagsBuilder.map<string>(tb => tb.name);
        if (this.id == 0)
            this.createPost();
        else
            this.updatePost();
    }
    public delete(): void {
        this.postService.delete(this.id)
            .subscribe(
                id => this.router.navigate(['/admin']),
                err => this.showErrorMessage('Unable to delete the post.'));
    }
    public onTagsChange(name: string): void {
        this.tagService.getTags(name, this.TAGS_AUTOCOMPLETE_MAX)
            .subscribe(tags => {
                this.autocompleteTags = tags;
            })
    }
    private populateCategories(): void {
        this.categoryService.getCategories()
            .subscribe(cs => {
                this.categories = cs;
                this.populatePost();
            });
    }
    private populatePost(): void {
        if (this.id == 0)
            return;
        this.postService.getPost(this.id)
            .subscribe(post => {
                this.setSavePost(post);
                this.dateCreated = post.dateCreated.toDDMMYYYY('.');
                this.tagsBuilder = post.tags;
            }, err => {
                this.router.navigate(['/admin']);
                this.showErrorMessage('Post not found');
            });
    }
    private setSavePost(postViewModel: Postviewmodel) : void{
        this.savePost.title = postViewModel.title;
        this.savePost.shortContent = postViewModel.shortContent;
        this.savePost.content = postViewModel.content;
        this.savePost.categoryId = postViewModel.category.id;
        this.savePost.tags = postViewModel.tags.map<string>(pvm => pvm.name);
    }
    private createPost(): void {
        this.postService.create(this.savePost)
            .subscribe(id => {
                this.showSuccessMessage('Post was sucessfully created.');
                this.router.navigate(['']);
            }, err => {
                this.showErrorMessage('Unable to create the post.');
            });
    }
    private updatePost(): void {
        this.postService.update(this.id, this.savePost)
            .subscribe(
                id => this.showSuccessMessage('Post was sucessfully saved.'),
                err => this.showErrorMessage('Unable to update the post.'));
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
