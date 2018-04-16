import { GlobalEventsService } from './../../services/globalevents.service';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Login } from '../../models/login';
import { UserService } from '../../services/user.service';
import { NgForm } from '@angular/forms';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    fail: boolean = false;
    loginForm: FormGroup = new FormGroup({});
    loginModel: Login = {
        name: '',
        password: ''
    };
    formFields: any = {
        name: ['', [Validators.required, Validators.maxLength(100)]],
        password: ['', [Validators.required, Validators.maxLength(100)]],
    };
    constructor(
        private userService: UserService,
        private router: Router,
        private globalEventsService: GlobalEventsService,
        private fb: FormBuilder
    ) {
        this.loginForm = this.fb.group(this.formFields);
    }
    public ngOnInit(): void {
    }
    public onSubmit(): void{
        this.loginModel.name = this.loginForm.controls['name'].value;
        this.loginModel.password = this.loginForm.controls['password'].value;
        this.userService.userAuthentication(this.loginModel)
            .subscribe(
                _ => {
                    this.globalEventsService.showAdminNavBar.emit(true);
                    this.router.navigate(['/admin']);
                },
                err => {
                    this.globalEventsService.showAdminNavBar.emit(false);
                    this.fail = true;
                });
    }
    public validateControl(controlName: string): boolean | null {
        return this.loginForm.controls[controlName].errors && !this.loginForm.controls[controlName].pristine;
    }
}
