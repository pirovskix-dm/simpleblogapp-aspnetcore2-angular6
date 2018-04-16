import { SaveCategoryViewModel } from './../../models/savecategoryviewmodel';
import { CategoryViewModel } from './../../models/categoryviewmodel';
import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../services/category.service';
import { ToastyService } from 'ng2-toasty';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

@Component({
    selector: 'app-categories',
    templateUrl: './categories.component.html',
    styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
    categoryForm: FormGroup = new FormGroup({});
    saveCategory: SaveCategoryViewModel = {
        name: ''
    };
    formFields: any = {
        name: ['', [Validators.required, Validators.maxLength(100)]]
    }
    categories: CategoryViewModel[] = [];
    constructor(
        private categoryService: CategoryService,
        private toastyService: ToastyService,
        private fb: FormBuilder
    ) {
        this.categoryForm = this.fb.group(this.formFields);
    }
    public ngOnInit(): void {
        this.populateCategories();
    }
    public create(): void {
        this.saveCategory.name = this.categoryForm.controls['name'].value;
        this.categoryService.create(this.saveCategory)
            .subscribe(id => {
                this.showSuccessMessage('Category was sucessfully created.');
                this.populateCategories();
            }, err => {
                this.showErrorMessage('Unable to create the category.');
            });
    }
    public delete(id: number): void{
        this.categoryService.delete(id)
            .subscribe( id => {
                    this.populateCategories();
                    this.showSuccessMessage('Category was sucessfully deleted.');
                }, err => this.showErrorMessage('Unable to delete the category.')
            );
    }
    private populateCategories(): void{
        this.categoryService.getCategories()
            .subscribe(categories => this.categories = categories);
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
