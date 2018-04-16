import { AppComponent } from './../app/app.component';
import { TagViewModel } from './../../models/tagviewmodel';
import { CategoryViewModel } from './../../models/categoryviewmodel';
import { SavePostViewModel } from './../../models/savepostviewmodel';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
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
    postForm: FormGroup = new FormGroup({});
    pageTitle: string = '';
    id: number = 0;
    dateCreated: string = '';
    categories: CategoryViewModel[] = [];
    autocompleteTags: any[] = [];
    savePost: SavePostViewModel = {
        title: '',
        shortContent: '',
        content: '',
        categoryId: null,
        tags: []
    };
    formFields: any = {
        title:          ['', [Validators.required, Validators.maxLength(100)]],
        shortContent:   ['', Validators.maxLength(500)],
        content:        ['', Validators.required],
        categoryId:     [null, Validators.required],
        tags:           []
    };
    constructor(
        private postService: PostService,
        private categoryService: CategoryService,
        private tagService: TagService,
        private router: Router,
        private route: ActivatedRoute,
        private toastyService: ToastyService,
        private fb: FormBuilder
    ) {
        this.postForm = this.fb.group(this.formFields);
    }
    public ngOnInit(): void {
        this.route.params.subscribe(p => {
            this.id = +p['id'] || 0;
            this.pageTitle = this.id ? 'Edit Post' : 'New Post';
            this.populateCategories();
        });
    }
    public onSubmit(): void {
        this.setSavePost();
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
    public validateControl(controlName: string): boolean | null {
        return this.postForm.controls[controlName].errors && !this.postForm.controls[controlName].pristine;
    }
    private populateCategories(): void {
        this.categoryService.getCategories()
            .subscribe(cs => {
                this.categories = cs;
                this.populatePost();
            });
    }
    private populatePost(): void {
        if (this.id == 0) {
            return;
        }
        this.postService.getPost(this.id)
            .subscribe(post => {
                this.setForm(post)
                this.dateCreated = post.dateCreated.toDDMMYYYY('.');
            }, err => {
                this.router.navigate(['/admin']);
                this.showErrorMessage('Post not found');
            });
    }
    private setForm(postViewModel: Postviewmodel): void {
        this.postForm.controls['title'].setValue(postViewModel.title);
        this.postForm.controls['shortContent'].setValue(postViewModel.shortContent);
        this.postForm.controls['content'].setValue(postViewModel.content);
        this.postForm.controls['categoryId'].setValue(postViewModel.category.id);
        this.postForm.controls['tags'].setValue(postViewModel.tags);
    }
    private setSavePost() : void {
        let cons = this.postForm.controls;
        this.savePost.title = cons['title'].value;
        this.savePost.shortContent = cons['shortContent'].value;
        this.savePost.content = cons['content'].value;
        this.savePost.categoryId = cons['categoryId'].value;
        if (cons['tags'].value && cons['tags'].value.length > 0)
            this.savePost.tags = (cons['tags'].value as any[]).map<string>(tb => tb.name);
        else
            this.savePost.tags = [];
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
