import { GlobalEventsService } from './../../services/globalevents.service';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Login } from '../../models/login';
import { UserService } from '../../services/user.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    fail: boolean = false;
    loginModel: Login = {
        name: '',
        password: ''
    }
    constructor(
        private userService: UserService,
        private router: Router,
        private globalEventsService: GlobalEventsService
    ) { 
    }
    public ngOnInit(): void {
    }
    public onSubmit(form: NgForm): void{
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
}
